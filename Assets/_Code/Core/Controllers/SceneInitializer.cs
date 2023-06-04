using Assets.Core.StaticChannels;
using Assets.Tags.Abstract;
using Game.Tags.Common;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Core.Controllers
{
    /// <summary>
    /// Initializer class used to start the required gamemode in the associated scene
    /// </summary>
    [SelectionBase]
    public class SceneInitializer : AExtendedMonobehaviour
    {
        [SerializeField, Required]
        private AGameMode gameMode = null;

        [SerializeField, Required]
        private AGridDataProvider sceneGridData = null;

        /// <summary>
        /// Called when the GameObject is created
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            GameplayInputChannel.EnableInput();
            gameMode.InitializeGameMode(gameObject);
            AGridDataProvider.SetActiveDataProvider(sceneGridData);
        }

        /// <summary>
        /// Called when the gameobject is destroyed
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            GameplayInputChannel.DisableInput();
            gameMode.EndGameMode();
            AGridDataProvider.SetActiveDataProvider(null);
        }
    }
}
