﻿using Drawing;
using Game.Core.Abstract;
using Game.Core.Interfaces;
using Game.Tags.Models;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Core.Controllers
{
    [SelectionBase]
    public class BasicStaticEntity : AEntity, IDamageReceiver, ILogicUpdateProcessor
    {
        [SerializeField, Required, InlineEditor]
        private StaticUnitStatsTag unitStats = null;

        [SerializeField, ReadOnly]
        private float currentHealth;
        [SerializeField, ReadOnly]
        private float attackCooldown;

        /// <summary>
        /// Called when object created
        /// </summary>
        protected override void Start()
        {
            base.Start();
            
            currentHealth = unitStats.MaxHealth;
            ALogicProcessor.Instance.RegisterHighPriorityProcessor(this);
        }

        /// <summary>
        /// Called when this object is destroyed
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            ALogicProcessor.Instance.DeregisterHighPriorityProcessor(this);
        }

        /// <summary>
        /// Applies damage to the entity
        /// </summary>
        /// <param name="damage">Damage to take</param>
        public void ApplyDamage(float damage)
        {
            currentHealth -= damage;

            if (currentHealth < 0)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Handles taking the call from orchestrator to perform a logical update
        /// </summary>
        public void ProcessLogic()
        {
            if (attackCooldown > 0)
                return;

            var collidersInRange = Physics.OverlapSphere(transform.position, unitStats.maxRange, unitStats.validTargetLayers);
            foreach (var collider in collidersInRange)
            {
                //Check if this collider is a valid target
                var receiverComponent = collider.gameObject.GetComponent<IDamageReceiver>();
                if (receiverComponent == null || !receiverComponent.IsHostile(TeamId))
                {
                    LogInformation($"Component: {receiverComponent} / Is Hostile: {(receiverComponent is null ? "NULL" : receiverComponent.IsHostile(TeamId))}");
                    continue;
                }

                //Apply damage to target and reset cooldown
                receiverComponent.ApplyDamage(unitStats.AttackDamage);
                attackCooldown = unitStats.AttackCooldown;

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

            Draw.WireSphere(transform.position, unitStats.maxRange, Color.white);
        }
    }
}