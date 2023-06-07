﻿using Assets.Core.StaticChannels;
using Assets.Tags.Abstract;
using Assets.Tags.Common;
using Assets.Utilities.Worker;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.Processors
{
    [CreateAssetMenu(menuName = InputProcessorAssetMenuBaseName + "Hub Input", fileName = "Hub Input Processor")]
    public class HubInputProcessor : AInputProcessor
    {
        [SerializeField, Required, BoxGroup("Data")]
        private ObjectTypeIdentifier cameraId = null;

        [SerializeField, MinValue(0), BoxGroup("Options")]
        private float cameraMovementSpeed = 2f;

        [SerializeField, BoxGroup("Options")]
        private LayerMask interactableMask;


        private bool playerStartedDragging = false;
        private Vector2 playerStartDragScreenPosition = Vector2.zero;
        private Vector3 playerStartCameraPosition = Vector3.zero;
        private GameObject sceneVirtualCamera = null;


        /// <summary>
        /// Called when the tag is loaded into the project
        /// </summary>
        public override void InitializeTag()
        {
            base.InitializeTag();

            GameplayInputChannel.OnPlayerTappedScreen += OnPlayerTappedScreen;
            GameplayInputChannel.OnPlayerStartedDragging += OnPlayerStartedDragging;
            GameplayInputChannel.OnPlayerIsDragging += OnPlayerIsDragging;
            GameplayInputChannel.OnPlayerStoppedDragging += OnPlayerStoppedDragging;
        }

        /// <summary>
        /// Called when the player starts a drag event
        /// </summary>
        /// <param name="screenPosition">Screen position of the user touch touch</param>
        private void OnPlayerStartedDragging(Vector2 screenPosition)
        {
            //Try to find the current camera
            if (!SceneObjectRegistry.TryGetObjectById(cameraId, out var cameraObject))
                return;

            //Grab initial data
            sceneVirtualCamera = cameraObject;
            playerStartedDragging = true;
            playerStartDragScreenPosition = screenPosition;
            playerStartCameraPosition = sceneVirtualCamera.transform.position;
        }

        /// <summary>
        /// Called each frame the user is dragging the screen
        /// </summary>
        /// <param name="screenPosition">Current touch position this frame</param>
        private void OnPlayerIsDragging(Vector2 screenPosition)
        {
            //Do nothing if a start event wasn't processed
            if (!playerStartedDragging)
                return;

            if (sceneVirtualCamera == null)
            {
                LogError($"No scene VCam has been registered, cannot move camera!");
                return;
            }

            var screenOffset = playerStartDragScreenPosition - screenPosition;
            screenOffset.x /= Screen.width;
            screenOffset.y /= Screen.height;
            screenOffset *= cameraMovementSpeed;

            sceneVirtualCamera.transform.position = playerStartCameraPosition + new Vector3(screenOffset.x, 0, screenOffset.y);
        }

        /// <summary>
        /// Called when the user stops dragging the screen
        /// </summary>
        /// <param name="screenPosition">Touch position on screen</param>
        private void OnPlayerStoppedDragging(Vector2 screenPosition)
        {
            playerStartedDragging = false;
            sceneVirtualCamera = null;
        }

        /// <summary>
        /// Called when the user taps the screen
        /// </summary>
        /// <param name="screenPosition">Position the user tapped</param>
        private void OnPlayerTappedScreen(Vector2 screenPosition)
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
