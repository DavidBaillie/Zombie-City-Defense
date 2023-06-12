using Assets.Core.Abstract;
using Assets.Tags.GameMode;
using System;

namespace Assets.Tags.Channels
{
    public static class SurvivalGameplayChannel
    {
        /// <summary>
        /// Raised when the game mode has taken all steps required for the mode to function
        /// </summary>
        public static event Action<SurvivalGameMode> OnGameModeSetupComplete;

        /// <summary>
        /// Raised when the game mode has finished cleaning up data and is ready to be deleted
        /// </summary>
        public static event Action<SurvivalGameMode> OnGameModeCleanupComplete;

        /// <summary>
        /// Raised when the player has selected a unit from their UI to interact with
        /// </summary>
        public static event Action<AStaticUnitInstance> OnUserSelectedEntityInGui;

        /// <summary>
        /// Raised when a static entity has been spawned into the game world during gameplay
        /// </summary>
        public static event Action<AStaticEntityController, AStaticUnitInstance> OnStaticEntitySpawned;

        /// <summary>
        /// Raised when an instanced unit has died during gameplay
        /// </summary>
        public static event Action<AStaticEntityController, AStaticUnitInstance> OnStaticUnitDeath;

        /// <summary>
        /// Raised when the player takes an action that will reset any selected unit they previous interacted with
        /// </summary>
        public static event Action OnPlayerResetUnitSelection;



        public static void RaiseOnGameModeSetupComplete(SurvivalGameMode mode) => OnGameModeSetupComplete?.Invoke(mode);
        public static void RaiseOnGameModeCleanupComplete(SurvivalGameMode mode) => OnGameModeCleanupComplete?.Invoke(mode);
        public static void RaiseOnUserSelectedEntityInGui(AStaticUnitInstance instance) => OnUserSelectedEntityInGui?.Invoke(instance);
        public static void RaiseOnStaticEntitySpawned(AStaticEntityController entity, AStaticUnitInstance instance) => OnStaticEntitySpawned?.Invoke(entity, instance);
        public static void RaiseOnStaticUnitDeath(AStaticEntityController spawnedEntity, AStaticUnitInstance unit) => OnStaticUnitDeath?.Invoke(spawnedEntity, unit);
        public static void RaiseOnPlayerResetUnitSelection() => OnPlayerResetUnitSelection?.Invoke();  
    }
}
