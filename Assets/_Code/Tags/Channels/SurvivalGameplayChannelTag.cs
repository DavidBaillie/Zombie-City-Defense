using Assets.Core.Abstract;
using Assets.Tags.Abstract;
using Assets.Tags.GameMode;
using Assets.Tags.Models;
using System;
using UnityEngine;

namespace Assets.Tags.Channels
{
    /// <summary>
    /// Instance of a channel used for the survival game mode to communicate states
    /// </summary>
    [CreateAssetMenu(menuName = ChannelAssetBaseName + "Survival Mode", fileName = "Survival Mode Gameplay Channel")]
    public class SurvivalGameplayChannelTag : AChannel
    {
        /// <summary>
        /// Raised when the game mode has taken all steps required for the mode to function
        /// </summary>
        public event Action<SurvivalGameMode> OnGameModeSetupComplete;

        /// <summary>
        /// Raised when the game mode has finished cleaning up data and is ready to be deleted
        /// </summary>
        public event Action<SurvivalGameMode> OnGameModeCleanupComplete;

        /// <summary>
        /// Raised when the player has selected a unit from their UI to interact with
        /// </summary>
        public event Action<AStaticUnitInstance> OnUserSelectedEntityInGui;



        public void RaiseOnGameModeSetupComplete(SurvivalGameMode mode) => OnGameModeSetupComplete?.Invoke(mode);
        public void RaiseOnGameModeCleanupComplete(SurvivalGameMode mode) => OnGameModeCleanupComplete?.Invoke(mode);
        public void RaiseOnUserSelectedEntityInGui(AStaticUnitInstance instance) => OnUserSelectedEntityInGui?.Invoke(instance);
    }
}
