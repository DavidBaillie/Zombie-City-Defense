using Drawing;
using Assets.Core.Abstract;
using Assets.Core.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using Assets.Tags.Abstract;
using Assets.Tags.Channels;
using Assets.Tags.Models;

namespace Assets.Core.Controllers
{
    /// <summary>
    /// Component class that represents a unit in the game world. Contains the minimum logic for the entity to fight and act in the world.
    /// </summary>
    [SelectionBase]
    public class HumanUnitController : AStaticEntityController, IDamageReceiver, ILogicUpdateProcessor
    {
        [SerializeField, ReadOnly, FoldoutGroup("Stats")]
        private StaticUnitTag unit = null;

        [SerializeField, ReadOnly, FoldoutGroup("Stats")]
        private float currentHealth;
        [SerializeField, ReadOnly, FoldoutGroup("Stats")]
        private float attackCooldown;

        /// <summary>
        /// Called when object created
        /// </summary>
        protected override void Start()
        {
            base.Start();
            
            currentHealth = unit.MaxHealth;
            ALogicProcessor.Instance.RegisterHighPriorityProcessor(this);
        }

        /// <summary>
        /// Not used by this class but allows for child types to receieve the tag associated with it
        /// </summary>
        /// <param name="unitTag"></param>
        public virtual void SetUnitTag(StaticUnitTag unitTag)
        {
            unit = unitTag;
        }


        public override void OnEntityRemoved()
        {
            base.OnEntityRemoved();
            ALogicProcessor.Instance.DeregisterHighPriorityProcessor(this);
            SurvivalGameplayChannel.RaiseOnStaticUnitDeath(this, unit);
        }

        /// <summary>
        /// Handles cleaning up the entity
        /// </summary>
        protected virtual void OnEntityDeath()
        {
            OnEntityRemoved();
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

            var collidersInRange = Physics.OverlapSphere(transform.position, unit.AttackRange, unit.ValidTargetLayers);
            
            foreach (var collider in collidersInRange)
            {
                //Check if this collider is a valid target
                if (!collider.gameObject.TryGetComponent(out IDamageReceiver receiver))
                    continue;
                
                //Apply damage to target and reset cooldown
                bool killedTarget = receiver.ApplyDamage(unit.AttackDamage);
                attackCooldown = unit.AttackCooldown;

                using (Draw.ingame.WithDuration(0.15f))
                {
                    Draw.ingame.Line(transform.position, collider.gameObject.transform.position, Color.red);
                }

                break;
            }
        }

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
        /// Debug gizmos drawer
        /// </summary>
        public override void DrawGizmos()
        {
            base.DrawGizmos();

            if (GizmoContext.InSelection(this))
                Draw.WireSphere(transform.position, unit.AttackRange, Color.red);
        }
    }
}
