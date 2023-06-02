using UnityEngine;

namespace Assets.Tags.Abstract
{
    /// <summary>
    /// Abstract class acts as the base for all game modes to allow them to be set up when a scene is loaded
    /// </summary>
    public abstract class AGameMode : ATag
    {
        public static AGameMode ActiveInstance;

        protected const string AssetMenuGameModeName = AssetMenuBaseName + "GameMode/";

        /// <summary>
        /// Called by the scene context that wants to start the gamemode.
        /// </summary>
        /// <param name="context">Scene initializing game mode</param>
        public abstract void InitializeGameMode(GameObject context);

        /// <summary>
        /// Called when the game mode actions should be undone and any assets deleted
        /// </summary>
        public abstract void EndGameMode();
    }
}
