using Assets.Core.Interfaces;
using Assets.Core.Models;
using Assets.Tags.Abstract;
using Assets.Tags.Channels;
using Assets.Tags.Models;
using Drawing;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets.Core.Abstract
{
    /// <summary>
    /// Controller class designed to manage state data for static entities in the game
    /// </summary>
    public class StaticEntityController : AEntityController, IDamageReceiver, ILogicUpdateProcessor
    {
        [ShowInInspector, ReadOnly, BoxGroup("General")]
        public Guid WorldPositionId = Guid.Empty;

        [SerializeField, ReadOnly, FoldoutGroup("Stats")]
        public StaticUnitTag StaticUnit = null;

        [SerializeField, ReadOnly, FoldoutGroup("Stats")]
        private float currentHealth = 0;
        [SerializeField, ReadOnly, FoldoutGroup("Stats")]
        private float attackCooldown = 0;

        /// <summary>
        /// Called each physics update to process timer
        /// </summary>
        protected override void FixedUpdate()
        {
            base.Update();

            if (attackCooldown > 0)
                attackCooldown -= Time.fixedDeltaTime;
        }

        /// <summary>
        /// Not used by this class but allows for child types to receieve the tag associated with it
        /// </summary>
        /// <param name="unitTag"></param>
        public virtual void SetupController(Guid positionId, StaticUnitTag unitTag)
        {
            WorldPositionId = positionId;
            StaticUnit = unitTag;

            currentHealth = StaticUnit.MaxHealth;
            ALogicProcessor.Instance.RegisterHighPriorityProcessor(this);
        }

        /// <summary>
        /// Handles cleaning up the entity
        /// </summary>
        protected virtual void OnEntityDeath()
        {
            ALogicProcessor.Instance.DeregisterHighPriorityProcessor(this);
            SurvivalGameplayChannel.RaiseOnStaticUnitDeath(this, StaticUnit);

            Destroy(gameObject);
        }

        /// <summary>
        /// Applies damage to the entity
        /// </summary>
        /// <param name="damage">Damage to take</param>
        /// <returns>If this entity died from the damage</returns>
        public bool ApplyDamage(float damage)
        {
            //Apply damage
            currentHealth -= damage;

            //Check if we're dead
            if (currentHealth > 0)
                return false;

            //We died...
            OnEntityDeath();
            return true;
        }

        /// <summary>
        /// Handles taking the call from orchestrator to perform a logical update
        /// </summary>
        public void ProcessLogic()
        {
            if (attackCooldown > 0)
                return;

            var collidersInRange = Physics.OverlapSphere(transform.position, StaticUnit.AttackRange, StaticUnit.ValidTargetLayers);

            foreach (var collider in collidersInRange)
            {
                //Check if this collider is a valid target
                if (!collider.gameObject.TryGetComponent(out IDamageReceiver receiver))
                    continue;

                //Apply damage to target and reset cooldown
                bool killedTarget = receiver.ApplyDamage(StaticUnit.AttackDamage);
                attackCooldown = StaticUnit.AttackCooldown;

                using (Draw.ingame.WithDuration(0.15f))
                {
                    Draw.ingame.Line(transform.position, collider.gameObject.transform.position, Color.red);
                }

                break;
            }
        }

        /// <summary>
        /// Debug gizmos drawer
        /// </summary>
        public override void DrawGizmos()
        {
            base.DrawGizmos();

            if (GizmoContext.InSelection(this))
                Draw.WireSphere(transform.position, StaticUnit.AttackRange, Color.red);
        }
    }
}
