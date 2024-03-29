﻿using Assets.Core.Abstract;
using Assets.Core.Controllers;
using Assets.Core.DataTracking;
using Assets.Core.Managers.Static;
using Assets.Core.Models;
using Assets.Core.StaticChannels;
using Assets.Tags.Abstract;
using Assets.Tags.Channels;
using Assets.Tags.Models;
using Assets.Tags.Processors;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tags.GameMode
{
    [CreateAssetMenu(menuName = AssetMenuGameModeName + "Survival", fileName = "Survival Game Mode Tag")]
    public class SurvivalGameMode : AGameMode
    {
        [SerializeField, Required, AssetsOnly]
        private GameObject gameplayCanvas = null;

        [SerializeField, Required, InlineEditor, FoldoutGroup("Data Store")]
        private PlayerUnitCollectionTag playerUnitCollection = null;

        [SerializeField, Required, InlineEditor, FoldoutGroup("Data Store")]
        private PlayerEconomyTag economyTag = null;

        [SerializeField, Required, InlineEditor, FoldoutGroup("Processors")]
        private GridVisualsProcessorTag gridVisuals = null;

        [SerializeField, Required, InlineEditor, FoldoutGroup("Processors")]
        private StaticUnitPlacementProcessorTag unitPlacementProcessor = null;

        [SerializeField, Required, InlineEditor, FoldoutGroup("Processors")]
        private AInputProcessor inputProcessor = null;


        [ShowInInspector, ReadOnly]
        private string selectedUnit { get => selectedUnitFromCanvas == null ? "null" : 
                (string.IsNullOrEmpty(selectedUnitFromCanvas.DisplayName) ? selectedUnitFromCanvas.Id.ToString() : selectedUnitFromCanvas.DisplayName); }
        [ShowInInspector, ReadOnly]
        private string selectedPosition { get => selectedWorldPosition == null || selectedWorldPosition.Value.Id == Guid.Empty ? "null" : selectedWorldPosition.Value.Coordinate.ToString(); }


        private GameplayCanvasController canvasControllerInstance = null;
        private AUnitTag selectedUnitFromCanvas = null;
        private WorldPosition? selectedWorldPosition = null;

        private Dictionary<AUnitTag, AEntityController> spawnedUnits = new();



        /// <summary>
        /// Sets up the game mode
        /// </summary>
        /// <param name="context">Scene that started the initialization</param>
        public override void InitializeGameMode(GameObject context)
        {
            GameplayInputChannel.EnableInput();

            //Try to setup the canvas
            if (!Instantiate(gameplayCanvas).TryGetComponent(out canvasControllerInstance))
            {
                LogError($"Failed to set up the gameplay canvas, could not find a component of type {nameof(GameplayCanvasController)}!");
            }

            //Setup data
            canvasControllerInstance.SetupUnitCollection(playerUnitCollection);

            selectedUnitFromCanvas = null;
            selectedWorldPosition = null;
            spawnedUnits = new();

            //Setup Tags
            playerUnitCollection.InitializeTag();
            gridVisuals.InitializeTag();
            unitPlacementProcessor.InitializeTag();
            inputProcessor.InitializeTag();
            economyTag.InitializeTag();

            //Register events
            SurvivalGameplayChannel.OnUserSelectedEntityInGui += OnUserSelectedEntityInGui;
            SurvivalGameplayChannel.OnStaticUnitDeath += OnStaticEntityDeath;

            PlayerActionChannel.OnPlayerSelectedWorldPosition += OnPlayerSelectedWorldPosition;
            PlayerActionChannel.OnPlayerSelectedInvalidPosition += OnPlayerSelectedInvalidWorldPosition;
            SurvivalGameplayChannel.OnResourceGathered += OnPlayerCollectedScrap;

            //Let others know we've set up
            LogInformation($"Started Game Mode [{name}]");
            SurvivalGameplayChannel.RaiseOnGameModeSetupComplete(this);
        }

        /// <summary>
        /// Called to cleanup actions from mode
        /// </summary>
        public override void EndGameMode()
        {
            GameplayInputChannel.DisableInput();

            DestroyImmediate(canvasControllerInstance);

            //Deregister events
            SurvivalGameplayChannel.OnUserSelectedEntityInGui -= OnUserSelectedEntityInGui;
            SurvivalGameplayChannel.OnStaticUnitDeath -= OnStaticEntityDeath;
            SurvivalGameplayChannel.OnResourceGathered -= OnPlayerCollectedScrap;
            PlayerActionChannel.OnPlayerSelectedWorldPosition -= OnPlayerSelectedWorldPosition;
            PlayerActionChannel.OnPlayerSelectedInvalidPosition -= OnPlayerSelectedInvalidWorldPosition;
            
            //Cleanup tags
            playerUnitCollection.CleanupTag();
            gridVisuals.CleanupTag();
            unitPlacementProcessor.CleanupTag();
            inputProcessor.CleanupTag();
            economyTag.CleanupTag();

            //Let others know where done
            LogInformation($"Ended Game Mode [{name}]");
            SurvivalGameplayChannel.RaiseOnGameModeCleanupComplete(this);
        }

        /// <summary>
        /// Called whenever a static entity dies to process the event and update state correctly
        /// </summary>
        /// <param name="unitController">Controller instance representing entity</param>
        /// <param name="tag">Entity tag with data</param>
        private void OnStaticEntityDeath(AEntityController unitController, AUnitTag tag)
        {
            switch (tag)
            {
                case ObjectiveUnitTag objectiveTag:
                    if (objectiveTag.isPrimaryObjective)
                        OnPrimaryObjectiveDeath(unitController, objectiveTag);
                    else 
                        OnOptionalObjectiveDeath(unitController, objectiveTag);
                    break;
                case StaticUnitTag unitTag:
                    OnUnitDeath(unitController, unitTag);
                    break;
                default:
                    LogWarning($"Survival Game Mode failed to process a static entity death because it is not a known type!");
                    break;
            }
        }

        /// <summary>
        /// Called when a primary objective dies
        /// </summary>
        /// <param name="unitController">Scene controller</param>
        /// <param name="objectiveTag">Unit tag</param>
        private void OnPrimaryObjectiveDeath(AEntityController unitController, ObjectiveUnitTag objectiveTag)
        {
            //Only process fail objectives
            if (!objectiveTag.isPrimaryObjective)
                return;

            GameManagers.LogicProcessor.BroadcastGameModeFailState();
            SurvivalGameplayChannel.RaiseOnGameModeObjectiveFailed();
        }

        /// <summary>
        /// Called when an optional objective is killed
        /// </summary>
        /// <param name="unitController">Controller in scene</param>
        /// <param name="objectiveTag">Objective killed</param>
        private void OnOptionalObjectiveDeath(AEntityController unitController, ObjectiveUnitTag objectiveTag)
        {
            //TODO
        }

        /// <summary>
        /// Called when a static unit dies
        /// </summary>
        /// <param name="unitController">Scene controller</param>
        /// <param name="unitTag">Unit tag</param>
        private void OnUnitDeath(AEntityController unitController, StaticUnitTag unitTag)
        {
            playerUnitCollection.AvailableUnits.Remove(unitTag);
            playerUnitCollection.DeadUnits.Add(unitTag);
            //playerUnitCollection.TrySaveUnitsToStorage();
        }

        /// <summary>
        /// Called when the user taps on a unit in the GUI
        /// </summary>
        /// <param name="unit">Unit selected</param>
        private void OnUserSelectedEntityInGui(AUnitTag unit)
        {
            //Check to see if the player selected a spawned unit
            if(spawnedUnits.ContainsKey(unit))
            {
                LogInformation($"User selected a unit that has already been spawned -> {unit.DisplayName}");
                return;
            }

            LogInformation($"User selected a unit that can be placed -> {unit.DisplayName}");
            selectedUnitFromCanvas = unit;
        }

        /// <summary>
        /// Called when the player selects a valid world coordinate
        /// </summary>
        /// <param name="position">Coordinate about where they tapped</param>
        private void OnPlayerSelectedWorldPosition(Vector3 position)
        {
            if (AGridDataProvider.ActiveInstance == null)
            {
                LogWarning($"Survival Game Mode could not process the player selecting a valid world position because no grid data provider has been registered!");
                return;
            }

            //Tap point isn't close to any coordinate, cancel actions
            if (!AGridDataProvider.ActiveInstance.TryGetClosestGridPosition(position, out WorldPosition closestPosition, GameSettings.InputSettings.TapSelectionRange))
            {
                //LogInformation($"Tapped position is not close enough to any coordinate");
                gridVisuals.HideVisual();
                selectedWorldPosition = null;
                SurvivalGameplayChannel.RaiseOnPlayerResetUnitSelection();
            }
            //Clicked the same spot twice
            else if (selectedUnitFromCanvas != null && selectedUnitFromCanvas.Id != Guid.Empty 
                && selectedWorldPosition != null && selectedWorldPosition.Value == closestPosition)
            {
                //Try to place the unit and process data if it could be placed
                if (selectedUnitFromCanvas is StaticUnitTag && 
                    unitPlacementProcessor.TryPlaceStaticUnitAtWorldPosition(selectedWorldPosition.Value, (StaticUnitTag)selectedUnitFromCanvas, out var controller))
                {
                    //Process event
                    spawnedUnits.Add(selectedUnitFromCanvas, controller);
                    SurvivalGameplayChannel.RaiseOnStaticEntitySpawned(controller, selectedUnitFromCanvas);
                }
                //Couldn't place the unit for some reason
                else
                {
                    LogWarning($"A unit could be placed at world coordinate [{selectedWorldPosition.Value.Coordinate}] when attemtping placement.");
                }

                //Reset data
                selectedWorldPosition = null;
                selectedUnitFromCanvas = null;
                gridVisuals.HideVisual();
                SurvivalGameplayChannel.RaiseOnPlayerResetUnitSelection();
            }
            //Player clicked a new location
            else
            {
                selectedWorldPosition = closestPosition;
                gridVisuals.ShowVisual(closestPosition.Coordinate);
            }
        }

        /// <summary>
        /// Called when the player selected an invalid world position
        /// </summary>
        /// <param name="screenPoint">Where on the screen they tapped</param>
        private void OnPlayerSelectedInvalidWorldPosition(Vector2 screenPoint)
        {
            selectedWorldPosition = null;
            gridVisuals.HideVisual();
        }

        /// <summary>
        /// Called when scrap/resources need to be added during gameplay
        /// </summary>
        /// <param name="amount">Amount of resource added</param>
        private void OnPlayerCollectedScrap(int amount)
        {
            economyTag.AvailableScrap += amount;
        }
    }
}
