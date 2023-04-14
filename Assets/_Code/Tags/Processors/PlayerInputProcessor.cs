using Assets.Tags.Abstract;
using Assets.Tags.Channels;
using Assets.Tags.Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.Processors
{
    [CreateAssetMenu(menuName = ProcessorAssetBaseName + "Input Processor")]
    public class PlayerInputProcessor : AProcessorTag
    {
        [SerializeField, Required]
        private PlayerInputChannel inputChannel = null;

        [SerializeField, Required]
        private ObjectTypeIdentifier cameraId = null;

        [SerializeField, MinValue(0)]
        private float cameraMovementSpeed = 2f;


        private bool playerStartedDragging = false;
        private Vector2 playerStartDragScreenPosition = Vector2.zero;
        private Vector3 playerStartCameraPosition = Vector3.zero;
        private GameObject sceneCamera = null;


        /// <summary>
        /// Called when the tag is loaded into the project
        /// </summary>
        public override void InitializeTag()
        {
            base.InitializeTag();

            inputChannel.OnPlayerTappedScreen += OnPlayerTappedScreen;
            inputChannel.OnPlayerStartedDragging += OnPlayerStartedDragging;
            inputChannel.OnPlayerIsDragging += OnPlayerIsDragging;
            inputChannel.OnPlayerStoppedDragging += OnPlayerStoppedDragging;
        }

        private void OnPlayerStoppedDragging(Vector2 screenPosition)
        {
            playerStartedDragging = false;
            sceneCamera = null;
        }

        private void OnPlayerIsDragging(Vector2 screenPosition)
        {
            //Do nothing if a start event wasn't processed
            if (!playerStartedDragging)
                return;

            if (sceneCamera == null)
            {
                LogError($"No scene VCam has been registered, cannot move camera!");
                return;
            }

            var screenOffset = playerStartDragScreenPosition - screenPosition;
            screenOffset.x /= Screen.width;
            screenOffset.y /= Screen.height;
            screenOffset *= cameraMovementSpeed;

            sceneCamera.transform.position = playerStartCameraPosition + new Vector3(screenOffset.x, 0, screenOffset.y);
        }

        private void OnPlayerStartedDragging(Vector2 screenPosition)
        {
            //Try to find the current camera
            if (!SceneObjectRegistry.TryGetObjectById(cameraId, out var cameraObject))
                return;

            //Grab initial data
            sceneCamera = cameraObject;
            playerStartedDragging = true;
            playerStartDragScreenPosition = screenPosition;
            playerStartCameraPosition = sceneCamera.transform.position;
        }

        private void OnPlayerTappedScreen(Vector2 screenPosition)
        {
            //LogInformation("Tap");
        }
    }
}
