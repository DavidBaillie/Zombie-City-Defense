using Assets.Core.Interfaces;
using Assets.Tags.Abstract;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tags.Channels
{
    [CreateAssetMenu(menuName = ChannelAssetBaseName + "Player Input", fileName = "Player Input Channel")]
    public class PlayerInputChannel : AChannel
    {
        private List<IInputController> inputControllers;

        public event Action<Vector2> OnPlayerTappedScreen;
        public event Action<Vector2> OnPlayerStartedDragging;
        public event Action<Vector2> OnPlayerIsDragging;
        public event Action<Vector2> OnPlayerStoppedDragging;


        public void RaiseOnPlayerTappedScreen(Vector2 screenPosition) => OnPlayerTappedScreen?.Invoke(screenPosition);
        public void RaiseOnPlayerStartedDragging(Vector2 screenPosition) => OnPlayerStartedDragging?.Invoke(screenPosition);
        public void RaiseOnPlayerIsDragging(Vector2 screenPosition) => OnPlayerIsDragging?.Invoke(screenPosition);
        public void RaiseOnPlayerStoppedDragging(Vector2 screenPosition) => OnPlayerStoppedDragging?.Invoke(screenPosition);

        /// <summary>
        /// Enables all controllers using this channel
        /// </summary>
        public void EnableInput()
        {
            foreach (var controller in inputControllers)
            {
                try { controller.EnableInput(); } catch { }
            }
        }

        /// <summary>
        /// Disables all controllers using this channel
        /// </summary>
        public void DisableInput()
        {
            foreach (var controller in inputControllers)
            {
                try { controller.DisableInput(); } catch { }
            }
        }
        
        /// <summary>
        /// Register self to receive callbacks for enabling input
        /// </summary>
        /// <param name="controller">Controller to call to</param>
        public void RegisterInputEnableCallback(IInputController controller)
        {
            inputControllers.Add(controller);
        }

        /// <summary>
        /// Register self to receive callbacks for disabling input
        /// </summary>
        /// <param name="controller">Controller to call to</param>
        public void DeregisterInputEnableCallback(IInputController controller)
        {
            inputControllers.Remove(controller);
        }
    }
}
