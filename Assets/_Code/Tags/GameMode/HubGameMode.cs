using Assets.Core.Controllers;
using Assets.Core.StaticChannels;
using Assets.Tags.Abstract;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.GameMode
{
    [CreateAssetMenu(menuName = AssetMenuGameModeName + "Hub", fileName = "Hub Game Mode")]
    public class HubGameMode : AGameMode
    {
        [SerializeField, Required, AssetsOnly]
        private GameObject hubCanvas = null;

        private HubCanvasController canvasController = null;

        /// <summary>
        /// Called to setup the game mode
        /// </summary>
        /// <param name="context">GameObject in scene starting the game mode</param>
        public override void InitializeGameMode(GameObject context)
        {
            GameplayInputChannel.EnableInput();

            if (!Instantiate(hubCanvas).TryGetComponent(out canvasController))
            {
                LogError($"Failed to Instanciate the hub canvas, please check that controller component exists on the prefab");
            }
        }

        /// <summary>
        /// Called to end the game mode and cleanup data
        /// </summary>
        public override void EndGameMode()
        {
            GameplayInputChannel.DisableInput();
        }
    }
}
