using Drawing;
using Game.Core.Abstract;
using Game.Core.Interfaces;
using Sirenix.OdinInspector;
using System;
using UnityEditor;
using UnityEngine;

namespace Game.Core.Controllers
{
    [SelectionBase]
    public class BasicMovingEntity : AMovingEntity, IDamageReceiver, ILogicUpdateProcessor
    {
        [SerializeField, ReadOnly]
        private float currentHealth = 1f;

        [SerializeField, ReadOnly]
        private float attackCooldown = 0;


        private Tuple<GameObject, IDamageReceiver> currentTarget = null;

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

            if (currentTarget != null)
            {
                //Target died
                if (currentTarget.Item1 == null)
                {
                    currentTarget = null;
                }
                //Target out of range
                else if (Vector3.Distance(transform.position, currentTarget.Item1.transform.position) > UnitStats.attackRange)
                {
                    MoveTowards(currentTarget.Item1.transform.position);
                }
                //Target in range and can attack
                else if (attackCooldown <= 0)
                {
                    currentTarget.Item2.ApplyDamage(UnitStats.AttackDamage);
                    attackCooldown = UnitStats.AttackCooldown;
                }
            }
            else if (AssignedPath == null || PathIndex >= AssignedPath.Count)
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
            if (currentTarget != null)
                return;

            var collidersInRange = Physics.OverlapSphere(transform.position, UnitStats.sightRange, UnitStats.validTargetLayers);
            foreach (var collider in collidersInRange)
            {
                //Check if this collider is a valid target
                var receiverComponent = collider.gameObject.GetComponent<IDamageReceiver>();
                if (receiverComponent == null || !receiverComponent.IsHostile(TeamId))
                {
                    continue;
                }

                currentTarget = new(collider.gameObject, receiverComponent);
                break;
            }
        }


        public override void DrawGizmos()
        {
            base.DrawGizmos();

            if (GizmoContext.InSelection(this))
            {
                Draw.WireSphere(transform.position, UnitStats.sightRange, Color.white);
                Draw.WireSphere(transform.position, UnitStats.attackRange, Color.red);
            }
        }
    }
}
