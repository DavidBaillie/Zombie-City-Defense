using Assets.Core.DataTracking;
using System;

namespace Assets.Core.Abstract
{
    public abstract class AStaticEntity : AEntity
    {
        protected Guid WorldPositionId;

        public virtual void AssignWorldPositionId(Guid positionId)
        {
            WorldPositionId = positionId;
            StaticEntityTracker.RegisterEntity(positionId, this);
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();

            StaticEntityTracker.DeregisterEntity(WorldPositionId);
        }
    }
}
