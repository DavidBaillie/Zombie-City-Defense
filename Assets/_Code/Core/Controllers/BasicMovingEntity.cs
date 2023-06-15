﻿using Drawing;
using Assets.Core.Abstract;
using Assets.Core.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using Assets.Tags.Abstract;
using Assets.Core.Models;
using Assets.Debug;
using Assets.Utilities.Definitions;

namespace Assets.Core.Controllers
{
    [SelectionBase]
    public class BasicMovingEntity : AMovingEntity, IDamageReceiver, ILogicUpdateProcessor
    {
        [SerializeField, ReadOnly]
        private float currentHealth = 1f;

        [SerializeField, ReadOnly]
        private float attackCooldown = 0;


        private MovingEntityTarget currentTarget;

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

        /// <summary>
        /// Called each frame
        /// </summary>
        protected override void Update()
        {
            base.Update();
            GameplayDebugHandler.HandleRenderCall(() => DrawDevDebug(), true, false);


            if (attackCooldown > 0)
                attackCooldown -= Time.deltaTime;

            //We have a target, seek it
            if (currentTarget.HasTarget)
            {
                //Can attack 
                if (Vector3.Distance(transform.position, currentTarget.TargetPosition) < UnitStats.attackRange)
                {
                    if (attackCooldown <= 0)
                        currentTarget.DamageReceiver.ApplyDamage(UnitStats.AttackDamage);
                }
                //Need to seek
                else
                {
                    MoveTowards(currentTarget.TargetPosition);
                }
            }
            //No target, walk forwards
            else
            {
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
        }

        /// <summary>
        /// Code called by the logic processor to compute more complex actions
        /// </summary>
        public void ProcessLogic()
        {
            if (currentTarget.HasTarget)
                return;

            Collider bestTarget = null;
            float bestDistance = float.MaxValue;
            float newDistance = 0;
            RaycastHit[] raycastData = new RaycastHit[1];

            foreach (Collider collider in Physics.OverlapSphere(transform.position, UnitStats.sightRange, UnitStats.ValidTargetLayers))
            {
                //Check if this collider is a valid target
                if (!collider.gameObject.TryGetComponent(out IDamageReceiver receiver) || !receiver.IsHostile(TeamId))
                    continue;

                //Check if there's a line of sight blocker
                if (Physics.RaycastNonAlloc(transform.position, collider.transform.position - transform.position, raycastData,
                    Vector3.Distance(transform.position, collider.transform.position), (int)UnitStats.SightBlockingLayers) > 0)
                {
                    GameplayDebugHandler.HandleRenderCall(() => 
                        Draw.Line(transform.position + new Vector3(0, 0.5f, 0), collider.transform.position, Color.red), 1f, true, false);
                    continue;
                }
                else
                {
                    GameplayDebugHandler.HandleRenderCall(() =>
                        Draw.Line(transform.position + new Vector3(0, 0.5f, 0), collider.transform.position, Color.green), 1f, true, false);
                }

                //Save it if this collider is closer than the last one
                newDistance = Vector3.Distance(transform.position, collider.gameObject.transform.position);
                if (newDistance < bestDistance)
                {
                    bestDistance = newDistance;
                    bestTarget = collider;
                }
            }

            //If we found a target, save it
            if (bestTarget != null)
                currentTarget = new MovingEntityTarget(
                    bestTarget.gameObject, 
                    bestTarget.gameObject.GetComponent<IDamageReceiver>(), 
                    bestTarget.ClosestPoint(transform.position));
        }

        /// <summary>
        /// Applies damage to the entity
        /// </summary>
        /// <param name="damage">Damage applied</param>
        /// <returns>If the damage killed this entity</returns>
        public bool ApplyDamage(float damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }

            return currentHealth <= 0;
        }

        /// <summary>
        /// ALINE plugin for rendering debug in editor
        /// </summary>
        public override void DrawGizmos()
        {
            base.DrawGizmos();
            DrawDevDebug(true);
        }

        /// <summary>
        /// Draws the developer debug visuals
        /// </summary>
        /// <param name="requireEditorSelection">If the dev must have them selected in editor to render</param>
        private void DrawDevDebug(bool requireEditorSelection = false)
        {
            if (UnitStats != null && (!requireEditorSelection || GizmoContext.InSelection(this)))
            {
                Draw.ingame.WireSphere(transform.position, UnitStats.sightRange, CustomColours.SoftWhite);
                Draw.ingame.WireSphere(transform.position, UnitStats.attackRange, CustomColours.SoftRed);

                if (currentTarget.HasTarget)
                    Draw.ingame.Line(transform.position, currentTarget.TargetPosition, Color.red);
            }
        }
    }
}
