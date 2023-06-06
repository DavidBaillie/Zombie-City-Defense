using Assets.Tags.Abstract;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Core.Controllers
{
    /// <summary>
    /// Scene initializer to start a gamemode when the scene begins
    /// </summary>
    public class GameModeSceneInitializer : ASceneInitializer
    {
        [SerializeField, Required]
        private AGameMode gameMode = null;

        /// <summary>
        /// Called when the GameObject is created
        /// </summary>
        protected override void Awake()
        { 
            base.Awake();
            gameMode.InitializeGameMode(gameObject);
        }

        /// <summary>
        /// Called when the gameobject is destroyed
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            gameMode.EndGameMode();
        }
    }
}
