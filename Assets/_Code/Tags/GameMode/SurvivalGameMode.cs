using Assets.Core.Abstract;
using Assets.Core.Controllers;
using Assets.Core.Models;
using Assets.Core.StaticChannels;
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

        private AStaticUnitInstance selectedUnit = null;
        private WorldPosition? selectedWorldPosition = null;



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

            //Register events
            gameplayChannel.OnUserSelectedEntityInGui += OnUserSelectedEntityInGui;
            PlayerActionChannel.OnPlayerSelectedWorldPosition += OnPlayerSelectedWorldPosition;
            PlayerActionChannel.OnPlayerSelectedInvalidPosition += OnPlayerSelectedInvalidWorldPosition;

            //Let others know we've set up
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

            //Deregister events
            gameplayChannel.OnUserSelectedEntityInGui -= OnUserSelectedEntityInGui;
            PlayerActionChannel.OnPlayerSelectedWorldPosition -= OnPlayerSelectedWorldPosition;
            PlayerActionChannel.OnPlayerSelectedInvalidPosition -= OnPlayerSelectedInvalidWorldPosition;

            //Let others know where done
            LogInformation($"Ended Game Mode [{name}]");
            gameplayChannel.RaiseOnGameModeCleanupComplete(this);
        }

        /// <summary>
        /// Called when the user taps on a unit in the GUI
        /// </summary>
        /// <param name="unit">Unit selected</param>
        private void OnUserSelectedEntityInGui(AStaticUnitInstance unit)
        {
            selectedUnit = unit;
        }

        /// <summary>
        /// Called when the player selects a valid world coordinate
        /// </summary>
        /// <param name="position">Coordinate about where they tapped</param>
        private void OnPlayerSelectedWorldPosition(WorldPosition position)
        {
            //Clicked the same spot twice
            if (selectedWorldPosition != null && selectedWorldPosition.Value == position)
            {

            }
            else
            {
                selectedWorldPosition = position;
            }
        }

        /// <summary>
        /// Called when the player selected an invalid world position
        /// </summary>
        /// <param name="screenPoint">Where on the screen they tapped</param>
        private void OnPlayerSelectedInvalidWorldPosition(Vector2 screenPoint)
        {
            selectedWorldPosition = null;
        }
    }
}
