using Drawing;
using Game.Core.Abstract;
using Game.Core.Interfaces;
using UnityEngine;

namespace Game.Core.Controllers
{
    [SelectionBase]
    public class BasicMovingEntity : AMovingEntity, IDamageReceiver, ILogicUpdateProcessor
    {
        [SerializeField]
        private float currentHealth = 1f;


        /// <summary>
        /// Called when scene starts
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            ALogicProcessor.Instance.RegisterLowPriorityProcessor(this);
            currentHealth = UnitStats.MaxHealth;
        }

        /// <summary>
        /// Called when object destroyed
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            ALogicProcessor.Instance.DeregisterLowPriorityProcessor(this);
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
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }

        public void ProcessLogic()
        {
            //using (Draw.ingame.WithDuration(0.25f))
            //{
            //    Draw.ingame.Ray(transform.position, Vector3.up * 3, Color.red);
            //}
        }
    }
}
