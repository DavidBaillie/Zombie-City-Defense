using Drawing;
using Game.Core.Controllers;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Managers
{
    /// <summary>
    /// Handles managing a collection of waypoints that an entity might path along
    /// </summary>
    public class WaypointCollectionManager : AExtendedMonobehaviour
    {
        [SerializeField, ValidateInput(nameof(waypointsIsValid), "Waypoints cannot have empty/null values!")]
        private List<WaypointController> waypoints;
        private bool waypointsIsValid => !waypoints.Exists(x => x == null);

        /// <summary>
        /// Called when object created
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            //Make sure the collection is valid
            waypoints.RemoveAll(x => x == null);
        }

        /// <summary>
        /// Generates a path for the caller given the current waypoint collection
        /// </summary>
        /// <returns>A path through the waypoints</returns>
        public List<Vector3> GeneratePath()
        {
            var path = new List<Vector3>(waypoints.Count);

            for (int i = 0; i < waypoints.Count; i++)
            {
                path.Add(waypoints[i].GetRandomPosition());
            }

            return path;
        }

        /// <summary>
        /// Draws debug gizmos in scene
        /// </summary>
        public override void DrawGizmos()
        {
            base.DrawGizmos();

            if (waypoints != null && waypoints.Count > 1 )
            {
                try
                {
                    for (int i = 0; i < waypoints.Count - 1; i++)
                    {
                        Draw.Line(waypoints[i].GetMaxPosition(), waypoints[i + 1].GetMaxPosition(), Color.black);
                        Draw.Line(waypoints[i].GetMinPosition(), waypoints[i + 1].GetMinPosition(), Color.black);
                    }
                }
                catch (System.Exception e)
                {
                    //LogError($"Could not generate bounds of waypoint path because an exception arose [{e.GetType().Name}]");
                }
            }
        }
    }
}
