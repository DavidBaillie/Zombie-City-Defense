using Game.Core.Abstract;
using Game.Core.Interfaces;
using UnityEngine;

namespace Game.Core.Controllers
{
    [SelectionBase]
    public class BasicMovingEntity : AMovingEntity, IDamageReceiver, ILogicUpdateProcessor
    {

        protected override void Start()
        {
            base.Start();

            ALogicProcessor.Instance.RegisterLowPriorityProcessor(this);
        }

        protected override void Update()
        {
            base.Update();


            if (AssignedPath == null || PathIndex >= AssignedPath.Count)
            {
                Destroy(gameObject);
                this.enabled = false;
            }
            else if (AssignedPath != null && PathIndex < AssignedPath.Count)
            {
                MoveTowardsNextWaypoint();

                if (Vector3.Distance(AssignedPath[PathIndex], transform.position) < 0.1f)
                {
                    PathIndex++;
                }
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
