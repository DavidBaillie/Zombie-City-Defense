using Assets.Core.Models;
using Assets.Tags.Channels;
using Assets.Tags.Common;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Core.Controllers
{
    public class GameplayCanvasController : AExtendedMonobehaviour
    {
        [SerializeField, Required]
        private CanvasGroup canvasContentsView = null;

        [SerializeField, Required]
        private CanvasGroup unitPlacementSelectionView = null;

        [SerializeField, Required]
        private GameplayCanvasChannel gameplayChannel = null;

        [SerializeField, Required]
        private PlayerActionChannel actionChannel = null;


        private bool isShowingUnitOptions = false;


        /// <summary>
        /// Called when the GameObject is created
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            unitPlacementSelectionView.alpha = 0;
        }

        /// <summary>
        /// Called when the object is being destroyed
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        /// <summary>
        /// Shows all canvas contents
        /// </summary>
        public void ShowCanvas()
        {
            canvasContentsView.alpha = 1;
            canvasContentsView.blocksRaycasts = true;
            canvasContentsView.interactable = true;
        }

        /// <summary>
        /// Hides all canvas contents
        /// </summary>
        public void HideCanvas()
        {
            canvasContentsView.alpha = 0;
            canvasContentsView.blocksRaycasts = false;
            canvasContentsView.interactable = false;
        }


        /// <summary>
        /// Called via Unity when the user presses the "units" button
        /// </summary>
        public void OnClickUnitSelectionButton()
        {
            isShowingUnitOptions = !isShowingUnitOptions;
            unitPlacementSelectionView.alpha = isShowingUnitOptions ? 1 : 0;
        }

        /// <summary>
        /// Called via Unity when the user presses one of the unit placement buttons
        /// </summary>
        /// <param name="id">Unit to place</param>
        public void OnUserSelectedUnitToPlace(StaticEntityIdentifier id)
        {
            if (id == null)
            {
                LogError($"Could not process UI request to place a static entity because the provided id was null.");
                return;
            }

            OnClickUnitSelectionButton();
            gameplayChannel.RaiseOnUserSelectedStaticEntityPlacement(id);
        }
    }
}
