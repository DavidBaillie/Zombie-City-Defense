using Assets.Core.StaticChannels;
using Assets.Tags.Abstract;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.Processors
{
    [CreateAssetMenu(menuName = InputProcessorAssetMenuBaseName + "Combat Processor")]
    public class CombatInputProcessor : AMovementInputProcessor
    {
        [SerializeField, BoxGroup("Options")]
        private LayerMask playspaceMask;

        /// <summary>
        /// Called when the user taps the screen
        /// </summary>
        /// <param name="screenPosition">Position the user tapped</param>
        protected override void OnPlayerTappedScreen(Vector2 screenPosition)
        {
            base.OnPlayerTappedScreen(screenPosition);

            //Raycast from tap to world, if hit a collider then it's a valid tap
            if (Physics.Raycast(Camera.main.ScreenPointToRay(screenPosition), out var hit, float.MaxValue, playspaceMask, QueryTriggerInteraction.Ignore))
            {
                PlayerActionChannel.RaiseOnPlayerSelectedWorldPosition(hit.point);
            }
            //Hit nothing, tap is invalid
            else
            {
                PlayerActionChannel.RaiseOnPlayerSelectedInvalidPosition(screenPosition);
            }
        }
    }
}
