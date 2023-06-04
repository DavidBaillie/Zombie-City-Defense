using Assets.Core.DataTracking;
using System;

namespace Assets.Core.Abstract
{
    public abstract class AStaticEntity : AEntity
    {
        protected Guid WorldPositionId = Guid.Empty;

        public virtual void AssignWorldPositionId(Guid positionId)
        {
            if (positionId == Guid.Empty)
            {
                LogError($"A static entity was asked to initialize with an empty world position Id and will be disabled!");
                gameObject.SetActive(false);
                return;
            }

            WorldPositionId = positionId;
            StaticEntityTracker.RegisterEntity(positionId, this);
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();

            //If the unit was set up, try to register it
            if (WorldPositionId != Guid.Empty)
                StaticEntityTracker.DeregisterEntity(WorldPositionId);
        }
    }
}
