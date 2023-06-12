using Assets.Core.Abstract;
using Assets.Core.Managers.Static;
using Assets.Tags.Channels;
using Assets.Tags.Models;
using Assets.Utilities.Extensions;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Assets.Core.Controllers
{
    public class GameplayCanvasController : AExtendedMonobehaviour
    {
        [SerializeField, Required]
        private CanvasGroup unitPlacementSelectionView = null;

        [SerializeField, Required]
        private GameObject unitCardPrefab = null;

        [SerializeField, Required]
        private SceneReference fallbackScene = null;

        private bool isShowingUnitOptions = false;
        private PlayerUnitCollectionTag UnitCollection = null;

        private Dictionary<Guid, UnitSelectionCanvasController> loadedControllers = new();

        /// <summary>
        /// Called when the component is enabled
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            SurvivalGameplayChannel.OnStaticEntitySpawned += OnEntitySpawned;
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
        private void OnEntitySpawned(AStaticEntityController sceneController, AStaticUnitInstance unitData)
        {
            if (sceneController == null)
            {
                LogError($"Gameplay canvas controller failed to process entity spawn event because a null value was passed!:\n" +
                    $"Scene Controller: {sceneController}\nUnit Instance: {unitData}");
                return;
            }

            LogInformation($"Detected entity spawn on the canvas for unit {unitData.DisplayName}");

            if (!loadedControllers.ContainsKey(unitData.Id))
            {
                LogWarning($"Could not find a unit canvas controller for the provided unit!");
                return;
            }

            var controller = loadedControllers[unitData.Id];
            LogInformation($"Marking visual as used for controller {controller}");
            controller.MarkVisualAsUsed();
        }

        /// <summary>
        /// Takes the input collection of units and builds a visual representation of them in the UI with callbacks
        /// </summary>
        /// <param name="collection">Collection to represent</param>
        public void SetupUnitCollection(PlayerUnitCollectionTag collection)
        {
            UnitCollection = collection;
            UnitSelectionCanvasController controller = null;

            foreach (var unit in collection.availableUnits)
            {
                if (Instantiate(unitCardPrefab, unitPlacementSelectionView.transform).TryGetComponent(out controller))
                {
                    controller.AssignUnit(unit, this);
                    loadedControllers.Add(unit.Id, controller);
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
        public void OnUserPressedUnitButton(AStaticUnitInstance unit)
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
    }
}
