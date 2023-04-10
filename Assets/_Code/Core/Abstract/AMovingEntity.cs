using Game.Tags.Models;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Abstract
{
    public abstract class AMovingEntity : AEntity
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

            if (timeMultiplier < 0)
                timeMultiplier = Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, AssignedPath[PathIndex], UnitStats.MovementSpeed * timeMultiplier);
        }
    }
}
