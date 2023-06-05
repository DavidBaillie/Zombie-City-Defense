using Assets.Core.Abstract;
using Assets.Tags.Channels;
using Assets.Tags.Models;
using Assets.Utilities.Extensions;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Core.Controllers
{
    public class GameplayCanvasController : AExtendedMonobehaviour
    {
        [SerializeField, Required]
        private CanvasGroup unitPlacementSelectionView = null;

        [SerializeField, Required]
        private GameObject unitCardPrefab = null;


        private bool isShowingUnitOptions = false;
        private PlayerUnitCollectionTag UnitCollection = null;
        private SurvivalGameplayChannelTag gameplayChannel = null;


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
        }

        /// <summary>
        /// Assigns a provided gameplay channel to this Canvas 
        /// </summary>
        /// <param name="channel">Channel to use</param>
        public void SetupReferences(SurvivalGameplayChannelTag channel)
        {
            this.gameplayChannel = channel;
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
            gameplayChannel.RaiseOnUserSelectedEntityInGui(unit);
        }
    }
}
