﻿using Assets.Core.Abstract;
using Assets.Core.Interfaces;
using Assets.Core.Managers.Static;
using Assets.Core.Models;
using Assets.Core.StaticChannels;
using Assets.Debug;
using Assets.Tags.Abstract;
using Assets.Tags.Channels;
using Assets.Tags.Models;
using Assets.Utilities.ExtendedClasses;
using Drawing;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Core.Controllers
{
    /// <summary>
    /// Controller class designed to manage state data for static entities in the game
    /// </summary>
    public class StaticEntityController : AEntityController, IDamageReceiver, ILogicUpdateProcessor, IDeployableEntity
    {
        [ShowInInspector, ReadOnly, BoxGroup("General")]
        public Guid WorldPositionId = Guid.Empty;

        [SerializeField, ReadOnly, FoldoutGroup("Stats")]
        public StaticUnitTag StaticUnit = null;

        [SerializeField, ReadOnly, FoldoutGroup("Stats")]
        private float currentHealth = 0;
        [SerializeField, ReadOnly, FoldoutGroup("Stats")]
        private float attackCooldown = 0;

        private StaticEntityTarget targetData = new();

        public AEntityController SetupController(WorldPosition position, AUnitTag tag)
        {
            StaticUnitTag localTag = tag as StaticUnitTag;
            localTag.ThrowIfNull("Cannot setup the static entity controller because the provided unit tag is not a static unit tag!");

            StaticUnit = localTag;
            WorldPositionId = position.Id;

            currentHealth = StaticUnit.MaxHealth;
            GameManagers.LogicProcessor.RegisterHighPriorityProcessor(this);

            return this;
        }

        /// <summary>
        /// Called from the logic processor when the associated game mode enters a fail state
        /// </summary>
        public void OnGameModeFailure()
        {
            GameManagers.LogicProcessor.DeregisterHighPriorityProcessor(this);
            this.enabled = false;
        }

        /// <summary>
        /// Called each frame
        /// </summary>
        protected override void Update()
        {
            base.Update();

            //Update attack state
            if (attackCooldown > 0)
                attackCooldown -= Time.deltaTime;

            //Check for a valid target
            if (!targetData.HasTarget || attackCooldown > 0)
                return;

            //Attack target
            using (Draw.ingame.WithDuration(0.15f)) { Draw.ingame.Line(transform.position + Vector3.up, targetData.TargetGameObject.transform.position, Color.red); }
            
            var killedTarget = targetData.DamageReceiver.ApplyDamage(StaticUnit.AttackDamage);
            attackCooldown = StaticUnit.AttackCooldown;

            //Wipe data if we killed it
            if (killedTarget)
                targetData = new();
        }

        /// <summary>
        /// Handles cleaning up the entity
        /// </summary>
        protected virtual void OnEntityDeath()
        {
            GameManagers.LogicProcessor.DeregisterHighPriorityProcessor(this);
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
            currentHealth = Mathf.Max(0, currentHealth - damage);
            SurvivalGameplayChannel.RaiseOnUnitHealthChanged(StaticUnit, currentHealth, StaticUnit.MaxHealth);

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
        public virtual void ProcessLogic()
        {
            if (targetData.HasTarget)
                return;

            //Check for all targets in range
            var collidersInRange = Physics
                .OverlapSphere(transform.position, StaticUnit.AttackRange, StaticUnit.ValidTargetLayers)
                .OrderBy(x => Vector3.Distance(x.transform.position, transform.position));

            //Find the closest valid target
            RaycastHit[] hits = new RaycastHit[0];
            foreach (var collider in collidersInRange)
            {
                //Check if this collider is a valid target
                if (!collider.gameObject.TryGetComponent(out IDamageReceiver receiver))
                    continue;

                //Check for a line of sight blocker
                if (Physics.RaycastNonAlloc(transform.position, collider.transform.position - transform.position, hits, 
                    Vector3.Distance(transform.position, collider.transform.position), (int)StaticUnit.SightBlockingLayers) > 0)
                    continue;

                //Save valid target for attack
                targetData = new(collider.gameObject, receiver);
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
