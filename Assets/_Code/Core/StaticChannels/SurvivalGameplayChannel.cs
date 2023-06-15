using Assets.Core.Abstract;
using Assets.Core.Controllers;
using Assets.Tags.GameMode;
using Assets.Tags.Models;
using System;
using UnityEditor.ShaderGraph.Internal;

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
        public static event Action<StaticUnitTag> OnUserSelectedEntityInGui;

        /// <summary>
        /// Raised when a static entity has been spawned into the game world during gameplay
        /// </summary>
        public static event Action<StaticEntityController, StaticUnitTag> OnStaticEntitySpawned;

        /// <summary>
        /// Raised when an instanced unit has died during gameplay
        /// </summary>
        public static event Action<StaticEntityController, StaticUnitTag> OnStaticUnitDeath;

        /// <summary>
        /// Raised when the player takes an action that will reset any selected unit they previous interacted with
        /// </summary>
        public static event Action OnPlayerResetUnitSelection;

        /// <summary>
        /// Called when the health on a deployed entity changes
        /// Unit / Current / Max
        /// </summary>
        public static event Action<StaticUnitTag, float, float> OnUnitHealthChanged;



        public static void RaiseOnGameModeSetupComplete(SurvivalGameMode mode) => OnGameModeSetupComplete?.Invoke(mode);
        public static void RaiseOnGameModeCleanupComplete(SurvivalGameMode mode) => OnGameModeCleanupComplete?.Invoke(mode);
        public static void RaiseOnUserSelectedEntityInGui(StaticUnitTag unit) => OnUserSelectedEntityInGui?.Invoke(unit);
        public static void RaiseOnStaticEntitySpawned(StaticEntityController entity, StaticUnitTag unit) => OnStaticEntitySpawned?.Invoke(entity, unit);
        public static void RaiseOnStaticUnitDeath(StaticEntityController spawnedEntity, StaticUnitTag unit) => OnStaticUnitDeath?.Invoke(spawnedEntity, unit);
        public static void RaiseOnPlayerResetUnitSelection() => OnPlayerResetUnitSelection?.Invoke();
        public static void RaiseOnUnitHealthChanged(StaticUnitTag unit, float currentHealth, float maxHealth) => OnUnitHealthChanged?.Invoke(unit, currentHealth, maxHealth);
    }
}
