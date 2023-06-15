using Assets.Core.StaticChannels;
using Assets.Tags.Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.Abstract
{
    /// <summary>
    /// Abstract class used to contain the portion of logic for moving the world camera
    /// </summary>
    public abstract class AMovementInputProcessor : AInputProcessor
    {
        [SerializeField, Required, BoxGroup("Data")]
        protected CameraTypeIdentifier CameraType = null;

        [SerializeField, MinValue(0), BoxGroup("Options")]
        protected float CameraMovementSpeed = 2f;

        protected bool PlayerStartedDragging = false;
        protected Vector2 PlayerStartDragScreenPosition = Vector2.zero;
        protected Vector3 PlayerStartCameraPosition = Vector3.zero;
        protected GameObject SceneVirtualCamera = null;


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
        /// Called when the tag is done being used and should dispose of resources
        /// </summary>
        public override void CleanupTag()
        {
            base.CleanupTag();

            GameplayInputChannel.OnPlayerTappedScreen -= OnPlayerTappedScreen;
            GameplayInputChannel.OnPlayerStartedDragging -= OnPlayerStartedDragging;
            GameplayInputChannel.OnPlayerIsDragging -= OnPlayerIsDragging;
            GameplayInputChannel.OnPlayerStoppedDragging -= OnPlayerStoppedDragging;
        }

        /// <summary>
        /// Called when the player starts a drag event
        /// </summary>
        /// <param name="screenPosition">Screen position of the user touch touch</param>
        protected virtual void OnPlayerStartedDragging(Vector2 screenPosition)
        {
            //Try to find the current camera
            if (!SceneObjectRegistry.TryGetObjectById(CameraType, out var cameraObject))
                return;

            //Grab initial data
            SceneVirtualCamera = cameraObject;
            PlayerStartedDragging = true;
            PlayerStartDragScreenPosition = screenPosition;
            PlayerStartCameraPosition = SceneVirtualCamera.transform.position;
        }

        /// <summary>
        /// Called each frame the user is dragging the screen
        /// </summary>
        /// <param name="screenPosition">Current touch position this frame</param>
        protected virtual void OnPlayerIsDragging(Vector2 screenPosition)
        {
            //Do nothing if a start event wasn't processed
            if (!PlayerStartedDragging)
                return;

            if (SceneVirtualCamera == null)
            {
                LogError($"No scene VCam has been registered, cannot move camera!");
                return;
            }

            var screenOffset = PlayerStartDragScreenPosition - screenPosition;
            screenOffset.x /= Screen.width;
            screenOffset.y /= Screen.height;
            screenOffset *= CameraMovementSpeed;

            SceneVirtualCamera.transform.position = PlayerStartCameraPosition + new Vector3(screenOffset.x, 0, screenOffset.y);
        }

        /// <summary>
        /// Called when the user stops dragging the screen
        /// </summary>
        /// <param name="screenPosition">Touch position on screen</param>
        protected virtual void OnPlayerStoppedDragging(Vector2 screenPosition)
        {
            PlayerStartedDragging = false;
            SceneVirtualCamera = null;
        }

        /// <summary>
        /// Called when the user taps the screen
        /// </summary>
        /// <param name="screenPosition">Position the user tapped</param>
        protected virtual void OnPlayerTappedScreen(Vector2 screenPosition) { }
    }
}
