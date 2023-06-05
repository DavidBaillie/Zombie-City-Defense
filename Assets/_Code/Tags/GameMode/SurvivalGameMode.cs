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
using UnityEngine;

namespace Assets.Tags.GameMode
{
    [CreateAssetMenu(menuName = AssetMenuGameModeName + "Survival", fileName = "Survival Game Mode Tag")]
    public class SurvivalGameMode : AGameMode
    {
        [SerializeField, Required, AssetsOnly]
        private GameObject gameplayCanvas = null;

        [SerializeField, Required, InlineEditor, FoldoutGroup("Processors")]
        private PlayerUnitCollectionTag playerUnitCollection = null;

        [SerializeField, Required, InlineEditor, FoldoutGroup("Processors")]
        private GridVisualsProcessorTag gridVisuals = null;

        [SerializeField, Required, InlineEditor, FoldoutGroup("Processors")]
        private StaticUnitPlacementProcessorTag unitPlacementProcessor = null;

        [SerializeField, Required, FoldoutGroup("Channels")]
        private SurvivalGameplayChannelTag gameplayChannel = null;


        private GameplayCanvasController canvasControllerInstance = null;

        private AStaticUnitInstance selectedUnit = null;
        private WorldPosition? selectedWorldPosition = null;



        /// <summary>
        /// Sets up the game mode
        /// </summary>
        /// <param name="context">Scene that started the initialization</param>
        public override void InitializeGameMode(GameObject context)
        {
            //Try to setup the canvas
            if (!Instantiate(gameplayCanvas).TryGetComponent(out canvasControllerInstance))
            {
                LogError($"Failed to set up the gameplay canvas, could not find a component of type {nameof(GameplayCanvasController)}!");
            }

            //Setup data
            canvasControllerInstance.SetupUnitCollection(playerUnitCollection);
            playerUnitCollection.TryLoadUnitsFromStorage();

            //Setup Tags
            gridVisuals.InitializeTag();
            unitPlacementProcessor.InitializeTag();

            //Register events
            gameplayChannel.OnUserSelectedEntityInGui += OnUserSelectedEntityInGui;
            PlayerActionChannel.OnPlayerSelectedWorldPosition += OnPlayerSelectedWorldPosition;
            PlayerActionChannel.OnPlayerSelectedInvalidPosition += OnPlayerSelectedInvalidWorldPosition;

            //Let others know we've set up
            LogInformation($"Started Game Mode [{name}]");
            gameplayChannel.RaiseOnGameModeSetupComplete(this);
        }

        /// <summary>
        /// Called to cleanup actions from mode
        /// </summary>
        public override void EndGameMode()
        {
            Destroy(canvasControllerInstance);
            playerUnitCollection.TrySaveUnitsToStorage();

            //Deregister events
            gameplayChannel.OnUserSelectedEntityInGui -= OnUserSelectedEntityInGui;
            PlayerActionChannel.OnPlayerSelectedWorldPosition -= OnPlayerSelectedWorldPosition;
            PlayerActionChannel.OnPlayerSelectedInvalidPosition -= OnPlayerSelectedInvalidWorldPosition;

            //Cleanup tags
            gridVisuals.CleanupTag();
            unitPlacementProcessor.CleanupTag();

            //Let others know where done
            LogInformation($"Ended Game Mode [{name}]");
            gameplayChannel.RaiseOnGameModeCleanupComplete(this);
        }

        /// <summary>
        /// Called when the user taps on a unit in the GUI
        /// </summary>
        /// <param name="unit">Unit selected</param>
        private void OnUserSelectedEntityInGui(AStaticUnitInstance unit)
        {
            //Entity has been spawned into the game world
            if (StaticEntityTracker.TryGetPositionByInstance(unit, out var position))
            {

            }
            //Entity has not been spawned into game world
            else
            {
                selectedUnit = unit;
            }
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
            }
            //Clicked the same spot twice
            else if (selectedWorldPosition != null && selectedWorldPosition.Value == closestPosition)
            {
                gridVisuals.HideVisual();
                //LogInformation($"Player tapped a second time");
                //TODO - Place unit

                selectedWorldPosition = null;
            }
            //Player clicked a new location
            else
            {
                //LogInformation($"Player tapped {closestPosition}");
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
    }
}
