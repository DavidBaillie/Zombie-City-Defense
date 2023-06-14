using Assets.Core.DataTracking;
using Assets.Tags.Models;
using Sirenix.OdinInspector;
using System;

namespace Assets.Core.Abstract
{
    /// <summary>
    /// Component class instanced into the gameplay scene responsible for representing a unit in the game
    /// </summary>
    public abstract class AStaticEntityController : AEntityController
    {
        [ShowInInspector, ReadOnly, BoxGroup("General")]
        public Guid WorldPositionId = Guid.Empty;

        /// <summary>
        /// Called when the unit is being set up
        /// </summary>
        /// <param name="unitInstance">Unit to represent</param>
        /// <param name="positionId">World position to use</param>
        /// <param name="gameplayChannel">Communication channel to use</param>
        public virtual void SetWorldPosition(Guid positionId)
        {
            WorldPositionId = positionId;
            StaticEntityTracker.RegisterEntity(positionId, this);
        }

        /// <summary>
        /// Called when the entity needs to be removed from the game world
        /// </summary>
        public virtual void OnEntityRemoved()
        {
            if (WorldPositionId != Guid.Empty)
                StaticEntityTracker.RegisterEntity(WorldPositionId, this);
        }
    }
}
