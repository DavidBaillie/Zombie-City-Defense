using Assets.Core.DataTracking;
using Assets.Tags.Channels;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets.Core.Abstract
{
    public abstract class AStaticEntityController : AEntityController
    {
        [ShowInInspector, ReadOnly, FoldoutGroup("General")]
        public Guid WorldPositionId = Guid.Empty;

        [SerializeField, ReadOnly, FoldoutGroup("General")]
        public AStaticUnitInstance LocalInstance = null;

        [SerializeField, ReadOnly, FoldoutGroup("General")]
        protected SurvivalGameplayChannelTag GameplayChannel = null;

        /// <summary>
        /// Called when the entity is dying to handle cleanup and saving
        /// </summary>
        protected virtual void OnEntityDeath()
        {
            WorldPositionId = Guid.Empty;
        }

        /// <summary>
        /// Called when the unit is being set up
        /// </summary>
        /// <param name="unitInstance">Unit to represent</param>
        /// <param name="positionId">World position to use</param>
        /// <param name="gameplayChannel">Communication channel to use</param>
        public virtual void AssignStateData(AStaticUnitInstance unitInstance, Guid positionId, SurvivalGameplayChannelTag gameplayChannel)
        {
            WorldPositionId = positionId;
            LocalInstance = unitInstance;
            GameplayChannel = gameplayChannel;

            StaticEntityTracker.RegisterEntity(positionId, this);
        }

        /// <summary>
        /// Called when the unit is being removed from the game
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            //If the unit was set up, try to register it
            if (WorldPositionId != Guid.Empty)
                StaticEntityTracker.DeregisterEntity(WorldPositionId);
        }
    }
}
