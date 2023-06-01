using Assets.Core.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Core.StaticChannels
{
    public static class GameplayInputChannel
    {
        private static List<IInputController> inputControllers = new();

        public static event Action<Vector2> OnPlayerTappedScreen;
        public static event Action<Vector2> OnPlayerStartedDragging;
        public static event Action<Vector2> OnPlayerIsDragging;
        public static event Action<Vector2> OnPlayerStoppedDragging;


        public static void RaiseOnPlayerTappedScreen(Vector2 screenPosition) => OnPlayerTappedScreen?.Invoke(screenPosition);
        public static void RaiseOnPlayerStartedDragging(Vector2 screenPosition) => OnPlayerStartedDragging?.Invoke(screenPosition);
        public static void RaiseOnPlayerIsDragging(Vector2 screenPosition) => OnPlayerIsDragging?.Invoke(screenPosition);
        public static void RaiseOnPlayerStoppedDragging(Vector2 screenPosition) => OnPlayerStoppedDragging?.Invoke(screenPosition);

        /// <summary>
        /// Enables all controllers using this channel
        /// </summary>
        public static void EnableInput()
        {
            foreach (var controller in inputControllers)
            {
                try { controller.EnableInput(); } catch { }
            }
        }

        /// <summary>
        /// Disables all controllers using this channel
        /// </summary>
        public static void DisableInput()
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
        public static void RegisterInputEnableCallback(IInputController controller)
        {
            inputControllers.Add(controller);
        }

        /// <summary>
        /// Register self to receive callbacks for disabling input
        /// </summary>
        /// <param name="controller">Controller to call to</param>
        public static void DeregisterInputEnableCallback(IInputController controller)
        {
            inputControllers.Remove(controller);
        }
    }
}
