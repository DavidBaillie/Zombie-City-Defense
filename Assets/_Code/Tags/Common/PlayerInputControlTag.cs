﻿using Assets.Core.Interfaces;
using Assets.Tags.Abstract;
using Assets.Tags.Channels;
using Game.Utilities.Worker;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Assets.Utilities.ExtendedClasses;

namespace Game.Tags.Common
{
    [CreateAssetMenu(menuName = AssetMenuBaseName + "Input/Player Input Control")]
    public class PlayerInputControlTag : ATag, IInputController
    {
        [SerializeField, Required]
        private PlayerInputChannel channel;

        [SerializeField]
        private bool enableLogging = false;


        private PlayerInput inputController = null;
        private Vector2 touchPosition => inputController.Gameplay.TouchPosition.ReadValue<Vector2>();
        private bool startedDragging = false;


        /// <summary>
        /// Called during preload to set up the tag
        /// </summary>
        public override void InitializeTag()
        {
            base.InitializeTag();

            //Subscribe to the central update loop for the game
            UnityEventPassthrough.Instance.OnUpdate += Update;

            //Generate events for local events to be raised when the user subscribes to us
            inputController = new PlayerInput();
            inputController.Gameplay.Tap.performed += PlayerTappedScreen;
            inputController.Gameplay.Drag.performed += PlayerStartedDragging;
            inputController.Gameplay.Drag.canceled += PlayerStoppedDragging;

            channel.RegisterInputEnableCallback(this);
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
                if (enableLogging)
                    LogInformation($"Dragging: {touchPosition}");

                channel.RaiseOnPlayerIsDragging(touchPosition);
            }
        }

        /// <summary>
        /// Called when the user performs a valid tap on the screen
        /// </summary>
        private void PlayerTappedScreen(InputAction.CallbackContext context)
        {
            if (EventSystem.current.IsPositionOverElement(touchPosition))
                return;

            if (enableLogging)
                LogInformation($"Tapped Screen: {touchPosition}");

            channel.RaiseOnPlayerTappedScreen(touchPosition);
        }

        /// <summary>
        /// Called when the user starts dragging their finger across the screen
        /// </summary>
        private void PlayerStartedDragging(InputAction.CallbackContext context)
        {
            if (EventSystem.current.IsPositionOverElement(touchPosition))
                return;

            if (enableLogging)
                LogInformation($"Started Dragging: {touchPosition}");
            
            channel.RaiseOnPlayerStartedDragging(touchPosition);
            startedDragging = true;
        }

        /// <summary>
        /// Called when the user stops dragging their finger across the screen
        /// </summary>
        private void PlayerStoppedDragging(InputAction.CallbackContext context)
        {
            if (!startedDragging)
                return;
            
            if (enableLogging)
                LogInformation($"Stopped Dragging: {touchPosition}");

            startedDragging = false;
            channel.RaiseOnPlayerStoppedDragging(touchPosition);
        }
    }
}
