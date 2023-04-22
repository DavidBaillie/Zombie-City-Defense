using Assets.Tags.Abstract;
using Assets.Tags.Channels;
using Assets.Tags.Common;
using Game.Tags.Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.Processors
{
    [CreateAssetMenu(menuName = ProcessorAssetBaseName + "Input Processor")]
    public class PlayerInputProcessor : AProcessorTag
    {
        [SerializeField, Required, BoxGroup("Channels")]
        private PlayerInputChannel inputChannel = null;

        [SerializeField, Required, BoxGroup("Channels")]
        private PlayerActionChannel actionChannel = null;


        [SerializeField, Required, BoxGroup("Data")]
        private ObjectTypeIdentifier cameraId = null;

        [SerializeField, Required, BoxGroup("Data")]
        private GridDataTag gridData = null;


        [SerializeField, MinValue(0), BoxGroup("Options")]
        private float cameraMovementSpeed = 2f;

        [SerializeField, MinValue(0), BoxGroup("Options")]
        private float tapSelectionRange = 1f;

        [SerializeField]
        private LayerMask playspaceMask;


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
            sceneCamera = cameraObject;
            playerStartedDragging = true;
            playerStartDragScreenPosition = screenPosition;
            playerStartCameraPosition = sceneCamera.transform.position;
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

        /// <summary>
        /// Called when the user stops dragging the screen
        /// </summary>
        /// <param name="screenPosition">Touch position on screen</param>
        private void OnPlayerStoppedDragging(Vector2 screenPosition)
        {
            playerStartedDragging = false;
            sceneCamera = null;
        }

        /// <summary>
        /// Called when the user taps the screen
        /// </summary>
        /// <param name="screenPosition">Position the user tapped</param>
        private void OnPlayerTappedScreen(Vector2 screenPosition)
        {
            //Check player tap, do nothing if missed floor
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(screenPosition), out var hit, float.MaxValue, playspaceMask, QueryTriggerInteraction.Ignore))
            {
                actionChannel.RaiseOnPlayerSelectedInvalidPosition(screenPosition);
                return;
            }

            //Player tapped floor, determine an action
            if (gridData.TryGetClosestGridPosition(hit.point, out var worldPosition, tapSelectionRange))
            {
                actionChannel.RaiseOnPlayerSelectedWorldPosition(worldPosition);
            }
            else
            {
                actionChannel.RaiseOnPlayerSelectedInvalidPosition(screenPosition);
            }
        }
    }
}
