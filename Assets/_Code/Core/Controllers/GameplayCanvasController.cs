﻿using Assets.Core.Abstract;
using Assets.Core.Managers.Static;
using Assets.Core.StaticChannels;
using Assets.Tags.Abstract;
using Assets.Tags.Channels;
using Assets.Tags.Models;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Core.Controllers
{
    public class GameplayCanvasController : AExtendedMonobehaviour
    {
        [SerializeField, Required, BoxGroup("Visuals")]
        private CanvasGroup unitPlacementSelectionView = null;

        [SerializeField, Required, AssetsOnly, BoxGroup("Reference")]
        private GameObject unitCardPrefab = null;

        [SerializeField, Required, AssetsOnly, BoxGroup("References")]
        private SceneReference fallbackScene = null;

        [SerializeField, Required, BoxGroup("Visuals")]
        private Image objectiveHealth = null;

        private bool isShowingUnitOptions = false;
        private PlayerUnitCollectionTag UnitCollection = null;

        private Dictionary<AUnitTag, UnitSelectionCanvasController> loadedControllers = new();

        /// <summary>
        /// Called when the component is enabled
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            SurvivalGameplayChannel.OnStaticEntitySpawned += OnEntitySpawned;
            SurvivalGameplayChannel.OnUnitHealthChanged += OnObjectiveHealthChanged;
        }

        /// <summary>
        /// Called when the component is disabled
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();
            SurvivalGameplayChannel.OnStaticEntitySpawned -= OnEntitySpawned;
        }


        /// <summary>
        /// Called when the GameObject is created
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            unitPlacementSelectionView.alpha = 1;
        }

        /// <summary>
        /// Called when the object is being destroyed
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            SurvivalGameplayChannel.OnStaticEntitySpawned -= OnEntitySpawned;
        }

        /// <summary>
        /// Called when a unit is spawned into the game world
        /// </summary>
        /// <param name="sceneController">Scene controller for unit</param>
        /// <param name="unitData">Base data for the spawned unit</param>
        private void OnEntitySpawned(AEntityController sceneController, AUnitTag unitData)
        {
            if (sceneController == null)
            {
                LogError($"Gameplay canvas controller failed to process entity spawn event because a null value was passed!:\n" +
                    $"Scene Controller: {sceneController}\nUnit Instance: {unitData}");
                return;
            }

            //LogInformation($"Detected entity spawn on the canvas for unit {unitData.DisplayName}");

            if (!loadedControllers.ContainsKey(unitData))
            {
                LogWarning($"Could not find a unit canvas controller for the provided unit!");
                return;
            }

            var controller = loadedControllers[unitData];
            LogInformation($"Marking visual as used for controller {controller}");
            controller.MarkVisualAsUsed();
        }

        /// <summary>
        /// Takes the input collection of units and builds a visual representation of them in the UI with callbacks
        /// </summary>
        /// <param name="collection">Collection to represent</param>
        public void SetupUnitCollection(PlayerUnitCollectionTag collection)
        {
            //LogInformation($"Canvas controller setting up collection:\n{string.Join("\n", collection.availableUnits.Select(x => x.DisplayName))}");

            UnitCollection = collection;
            UnitSelectionCanvasController controller = null;

            foreach (var unit in collection.LivingUnits)
            {
                if (Instantiate(unitCardPrefab, unitPlacementSelectionView.transform).TryGetComponent(out controller))
                {
                    controller.AssignUnit(unit, this);
                    loadedControllers.Add(unit, controller);
                }
                else
                {
                    LogError($"Gameplay canvas failed to spawn a card the a given unit because the prefab is missing the {nameof(UnitSelectionCanvasController)} component");
                }
            }
        }

        /// <summary>
        /// Called from the sub-controller when a unit is selected
        /// </summary>
        /// <param name="unit">Unit user selected</param>
        public void OnUserPressedUnitButton(AUnitTag unit)
        {
            SurvivalGameplayChannel.RaiseOnUserSelectedEntityInGui(unit);
        }

        /// <summary>
        /// Called from the canvas when the user presses the retreat button
        /// </summary>
        public void OnUserPressedRetreatButton()
        {
            GameManagers.SceneManager.LoadScene(fallbackScene);
        }

        /// <summary>
        /// Called whenever a unit's health changes, only processes objective healths
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="currentHealth"></param>
        /// <param name="maxHealth"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnObjectiveHealthChanged(AUnitTag tag, float currentHealth, float maxHealth)
        {
            if (!(tag is ObjectiveUnitTag))
                return;

            //Assign value to UI
            objectiveHealth.fillAmount = currentHealth / maxHealth;
        }
    }
}
