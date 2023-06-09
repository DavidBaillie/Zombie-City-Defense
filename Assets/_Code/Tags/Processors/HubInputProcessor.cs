using Assets.Core.StaticChannels;
using Assets.Tags.Abstract;
using Assets.Tags.Common;
using Assets.Utilities.Worker;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.Processors
{
    [CreateAssetMenu(menuName = InputProcessorAssetMenuBaseName + "Hub Input", fileName = "Hub Input Processor")]
    public class HubInputProcessor : AMovementInputProcessor
    {
        [SerializeField, FoldoutGroup("Options")]
        private LayerMask interactableMask;

        /// <summary>
        /// Called when the user taps the screen
        /// </summary>
        /// <param name="screenPosition">Position the user tapped</param>
        protected override void OnPlayerTappedScreen(Vector2 screenPosition)
        {
            //Raycast from tap to world, if hit a collider then it's a valid tap
            if (Physics.Raycast(Camera.main.ScreenPointToRay(screenPosition), out var hit, float.MaxValue, interactableMask, QueryTriggerInteraction.Ignore))
            {
                PlayerActionChannel.RaiseOnPlayerSelectedInteractableObject(hit.collider.gameObject);
            }
            //Hit nothing, tap is invalid
            else
            {
                PlayerActionChannel.RaiseOnPlayerSelectedInvalidPosition(screenPosition);
            }
        }
    }
}
