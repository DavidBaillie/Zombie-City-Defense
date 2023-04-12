using Assets.Tags.Abstract;
using Assets.Tags.Channels;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.Processors
{
    [CreateAssetMenu(menuName = ProcessorAssetBaseName + "Input Processor")]
    public class PlayerInputProcessor : AProcessorTag
    {
        [SerializeField, Required]
        private PlayerInputChannel inputChannel = null;

        [SerializeField, MinValue(0)]
        private float cameraMovementSpeed = 10f;


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

        private void OnPlayerStoppedDragging(Vector2 obj)
        {
            playerStartedDragging = false;
        }

        private void OnPlayerIsDragging(Vector2 obj)
        {
            
        }

        private void OnPlayerStartedDragging(Vector2 obj)
        {
            playerStartedDragging = true;


        }

        private void OnPlayerTappedScreen(Vector2 obj)
        {

        }
    }
}
