using Assets.Core.Interfaces;
using Assets.Core.StaticChannels;
using Assets.Tags.Abstract;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.Processors
{
    [CreateAssetMenu(menuName = InputProcessorAssetMenuBaseName + "Hub Input", fileName = "Hub Input Processor")]
    public class HubInputProcessor : AMovementInputProcessor
    {
        [SerializeField, BoxGroup("Options")]
        private LayerMask interactableMask;

        /// <summary>
        /// Called when the user taps the screen
        /// </summary>
        /// <param name="screenPosition">Position the user tapped</param>
        protected override void OnPlayerTappedScreen(Vector2 screenPosition) 
        {
            base.OnPlayerTappedScreen(screenPosition);

            //Raycast from tap to world, check if it's a scene interactable
            if (Physics.Raycast(Camera.main.ScreenPointToRay(screenPosition), out var hit, float.MaxValue, interactableMask, QueryTriggerInteraction.Ignore)
                && hit.collider.gameObject.TryGetComponent(out ISceneInteractable interactable))
            {
                LogInformation($"Interacted with scene object");
                interactable.OnInteract();
            }
            //Hit nothing, tap is invalid
            else
            {
                LogInformation($"Interacted with nothing [{hit.point}]");
                PlayerActionChannel.RaiseOnPlayerSelectedInvalidPosition(screenPosition);
            }
        }
    }
}
