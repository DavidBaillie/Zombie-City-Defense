using Assets.Core.Interfaces;
using Assets.Tags.Abstract;
using Game.Utilities.Worker;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Assets.Utilities.ExtendedClasses;
using Assets.Core.StaticChannels;
using System;

namespace Game.Tags.Common
{
    [CreateAssetMenu(menuName = AssetMenuBaseName + "Input/Player Input Control")]
    public class PlayerInputControlTag : ATag, IInputController
    {
        [SerializeField]
        private bool enableLogging = false;


        private PlayerInput inputController = null;
        private Vector2 touchPosition => inputController.Gameplay.TouchPosition.ReadValue<Vector2>();
        private bool startedDragging = false;


        private void LogIfEnabled(Action act) { if (enableLogging) act.Invoke(); }


        /// <summary>
        /// Called during preload to set up the tag
        /// </summary>
        public override void InitializeTag()
        {
            base.InitializeTag();

            LogIfEnabled(() => LogInformation($"Initialized tag [{name}]"));

            //Subscribe to the central update loop for the game
            UnityEventPassthrough.Instance.OnUpdate += Update;

            //Generate events for local events to be raised when the user subscribes to us
            inputController = new PlayerInput();
            inputController.Gameplay.Tap.performed += PlayerTappedScreen;
            inputController.Gameplay.Drag.performed += PlayerStartedDragging;
            inputController.Gameplay.Drag.canceled += PlayerStoppedDragging;

            GameplayInputChannel.RegisterInputEnableCallback(this);
        }

        /// <summary>
        /// Enables gameplay input for the game
        /// </summary>
        public void EnableInput() => inputController.Enable();
        /// <summary>
        /// Disables gameplay input for the game
        /// </summary>
        public void DisableInput() => inputController.Disable(); 

        /// <summary>
        /// Called each frame while the game is active
        /// </summary>
        private void Update()
        {
            //If the user is dragging, raise an event each frame for listeners
            if (startedDragging)
            {
                LogIfEnabled(() => LogInformation($"Dragging: {touchPosition}"));
                GameplayInputChannel.RaiseOnPlayerIsDragging(touchPosition);
            }
        }

        /// <summary>
        /// Called when the user performs a valid tap on the screen
        /// </summary>
        private void PlayerTappedScreen(InputAction.CallbackContext context)
        {
            if (EventSystem.current.IsPositionOverElement(touchPosition))
            {
                LogIfEnabled(() => LogInformation($"Ignoring screen tap, over element: {touchPosition}"));
                return;
            }

            LogIfEnabled(() => LogInformation($"Tapped screen: {touchPosition}"));
            GameplayInputChannel.RaiseOnPlayerTappedScreen(touchPosition);
        }

        /// <summary>
        /// Called when the user starts dragging their finger across the screen
        /// </summary>
        private void PlayerStartedDragging(InputAction.CallbackContext context)
        {
            if (EventSystem.current.IsPositionOverElement(touchPosition))
                return;

            LogIfEnabled(() => LogInformation($"Started Dragging: {touchPosition}"));

            GameplayInputChannel.RaiseOnPlayerStartedDragging(touchPosition);
            startedDragging = true;
        }

        /// <summary>
        /// Called when the user stops dragging their finger across the screen
        /// </summary>
        private void PlayerStoppedDragging(InputAction.CallbackContext context)
        {
            if (!startedDragging)
                return;
            
            LogIfEnabled(() => LogInformation($"Stopped Dragging: {touchPosition}"));

            startedDragging = false;
            GameplayInputChannel.RaiseOnPlayerStoppedDragging(touchPosition);
        }
    }
}
