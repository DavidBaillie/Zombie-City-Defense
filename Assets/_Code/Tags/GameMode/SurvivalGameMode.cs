using Assets.Core.Controllers;
using Assets.Tags.Abstract;
using Assets.Tags.Channels;
using Assets.Tags.Models;
using Assets.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.GameMode
{
    [CreateAssetMenu(menuName = AssetMenuGameModeName + "Survival", fileName = "Survival Game Mode Tag")]
    public class SurvivalGameMode : AGameMode
    {
        [SerializeField, Required, AssetsOnly]
        private GameObject gameplayCanvas = null;

        [SerializeField, Required]
        private PlayerUnitCollectionTag playerUnitCollection = null;

        [SerializeField, Required]
        private SurvivalGameplayChannelTag gameplayChannel = null;


        private GameplayCanvasController canvasControllerInstance = null;

        /// <summary>
        /// Sets up the game mode
        /// </summary>
        /// <param name="context">Scene that started the initialization</param>
        public override void InitializeGameMode(GameObject context)
        {
            //Try to setup the canvas
            if (!Instantiate(gameplayCanvas).TryGetComponent(out canvasControllerInstance))
            {
                LogError($"Failed to set up the gameplay canvas, could not find a component of type {nameof(GameplayCanvasController)}!");
            }

            //Setup data
            canvasControllerInstance.SetupUnitCollection(playerUnitCollection);
            playerUnitCollection.TryLoadUnitsFromStorage();

            LogInformation($"Started Game Mode [{name}]");

            gameplayChannel.RaiseOnGameModeSetupComplete(this);
        }

        /// <summary>
        /// Called to cleanup actions from mode
        /// </summary>
        public override void EndGameMode()
        {
            Destroy(canvasControllerInstance);
            playerUnitCollection.TrySaveUnitsToStorage();

            gameplayChannel.RaiseOnGameModeCleanupComplete(this);

            LogInformation($"Ended Game Mode [{name}]");
        }
    }
}
