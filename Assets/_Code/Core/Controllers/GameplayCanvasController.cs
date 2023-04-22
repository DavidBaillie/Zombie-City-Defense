using Assets.Core.Models;
using Assets.Tags.Channels;
using Assets.Tags.Common;
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
        private GameplayCanvasChannel gameplayChannel = null;

        [SerializeField, Required]
        private PlayerActionChannel actionChannel = null;


        private WorldPosition? lastReceivedPosition = null;


        /// <summary>
        /// Called when the GameObject is created
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            unitPlacementSelectionView.alpha = 0;

            gameplayChannel.OnDisplayPlacementSelectionView += OnDisplayPlacementSelectionView;
            actionChannel.OnPlayerSelectedInvalidPosition += OnPlayerSelectedInvalidPosition;
        }

        /// <summary>
        /// Called when the user presses on a screen point that does not yield a valid world coordinate
        /// </summary>
        /// <param name="screenPosition">Where on the screen the user pressed</param>
        private void OnPlayerSelectedInvalidPosition(Vector2 screenPosition)
        {
            lastReceivedPosition = null;
            unitPlacementSelectionView.alpha = 0;
        }

        /// <summary>
        /// Called when the object is being destroyed
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            gameplayChannel.OnDisplayPlacementSelectionView -= OnDisplayPlacementSelectionView;
        }

        /// <summary>
        /// Called via the gameplay channel when the static unit selection display needs to be shown
        /// </summary>
        /// <param name="positionData">Where the unit will be placed</param>
        private void OnDisplayPlacementSelectionView(WorldPosition positionData)
        {
            unitPlacementSelectionView.alpha = 1;
            lastReceivedPosition = positionData;
        }

        /// <summary>
        /// Called by Canvas button when the user selects a unit to place
        /// </summary>
        /// <param name="identifier"></param>
        public void SubmitUnitPlacementSelection(StaticEntityIdentifier identifier)
        {
            //Action was cancelled, don't process 
            if (lastReceivedPosition == null)
                return;
                
            //Select the unit for placement and reset
            gameplayChannel.RaiseOnUserSelectedStaticEntityPlacement(lastReceivedPosition.Value, identifier);    
            lastReceivedPosition = null;
            unitPlacementSelectionView.alpha = 0;
        }
    }
}
