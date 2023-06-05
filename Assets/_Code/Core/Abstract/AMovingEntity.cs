using Game.Tags.Models;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Core.Abstract
{
    public abstract class AMovingEntity : AEntityController
    {
        [SerializeField, Required, InlineEditor]
        protected MovingUnitStatsTag UnitStats = null;

        [SerializeField, ReadOnly, FoldoutGroup("Debug")]
        protected int PathIndex = 0;
        [SerializeField, ReadOnly, FoldoutGroup("Debug")]
        protected List<Vector3> AssignedPath = null;

        /// <summary>
        /// Assigns the provided path to this entity
        /// </summary>
        /// <param name="path">Path to follow</param>
        public virtual void AssignPath(List<Vector3> path)
        {
            AssignedPath = path;
            PathIndex = 0;
        }

        /// <summary>
        /// Moves the entity towards the next waypoint
        /// </summary>
        protected virtual void MoveTowardsNextWaypoint(float timeMultiplier = -1)
        {
            if (AssignedPath == null || PathIndex >= AssignedPath.Count)
            {
                AssignedPath = null;
                PathIndex = 0;
                return;
            }

            MoveTowards(AssignedPath[PathIndex], Time.deltaTime);
        }

        /// <summary>
        /// Moves this entity towards a given position
        /// </summary>
        /// <param name="position">Target to seek</param>
        /// <param name="timeMultiplier">Time delta for movement speed</param>
        protected virtual void MoveTowards(Vector3 position, float timeMultiplier = -1)
        {
            if (timeMultiplier < 0)
                timeMultiplier = Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, position, UnitStats.MovementSpeed * timeMultiplier);
        }
    }
}
