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
    }
}
