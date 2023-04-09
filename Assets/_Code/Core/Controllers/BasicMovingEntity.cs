using Game.Core.Abstract;
using Game.Core.Interfaces;
using UnityEngine;

namespace Game.Core.Controllers
{
    [SelectionBase]
    public class BasicMovingEntity : AMovingEntity, IDamageReceiver, ILogicUpdateProcessor
    {

        protected override void Update()
        {
            base.Update();

            if (AssignedPath != null)
            {
                MoveTowardsNextWaypoint();
            }
        }

        public void ApplyDamage(float damage)
        {
            
        }

        public void ProcessLogic()
        {
            
        }
    }
}
