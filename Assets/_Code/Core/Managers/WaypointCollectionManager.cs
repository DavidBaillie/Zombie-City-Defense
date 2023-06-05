using Drawing;
using Assets.Core.Controllers;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;
using Assets.Debug;

namespace Assets.Core.Managers
{
    /// <summary>
    /// Handles managing a collection of waypoints that an entity might path along
    /// </summary>
    [SelectionBase]
    public class WaypointCollectionManager : AExtendedMonobehaviour
    {
        [SerializeField, Range(0.001f, 1f)]
        private float waypointVariantThreshold = 0.15f;

        [SerializeField, ValidateInput(nameof(waypointsIsValid), "Waypoints cannot have empty/null values!")]
        private List<WaypointController> waypoints;
        private bool waypointsIsValid => !waypoints.Exists(x => x == null);

        [SerializeField, FoldoutGroup("Debug")]
        private float renderDuration = 3f;


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
        /// Called each frame
        /// </summary>
        protected override void Update()
        {
            base.Update();
            DrawDebug();
        }

        /// <summary>
        /// Generates a path for the caller given the current waypoint collection
        /// </summary>
        /// <returns>A path through the waypoints</returns>
        public List<Vector3> GeneratePath()
        {
            var path = new List<Vector3>(waypoints.Count);
            float upper = 1, lower = -1;

            for (int i = 0; i < waypoints.Count; i++)
            {
                path.Add(waypoints[i].GetRandomPosition(out float relativeValue, lower, upper));
                lower = math.max(-1, relativeValue - math.abs(relativeValue * waypointVariantThreshold));
                upper = math.min(1, relativeValue + math.abs(relativeValue * waypointVariantThreshold));
            }

            return path;
        }

        /// <summary>
        /// Draws debug gizmos in scene
        /// </summary>
        public override void DrawGizmos()
        {
            base.DrawGizmos();
            DrawDebug();   
        }

        /// <summary>
        /// Handles drawing debug data for dev builds
        /// </summary>
        private void DrawDebug()
        {
            GameplayDebugHandler.HandleRenderCall(() =>
            {
                if (waypoints != null && waypoints.Any())
                {
                    try
                    {
                        for (int i = 0; i < waypoints.Count - 1; i++)
                        {
                            Draw.Line(waypoints[i].GetMaxPosition(), waypoints[i + 1].GetMaxPosition(), Color.black);
                            Draw.Line(waypoints[i].GetMinPosition(), waypoints[i + 1].GetMinPosition(), Color.black);
                        }
                    }
                    catch
                    {
                        //LogError($"Could not generate bounds of waypoint path because an exception arose [{e.GetType().Name}]");
                    }
                }
            }, true, false);
        }


        [Button("Preview Path")]
        private void PreviewPath()
        {
            for (int x = 0; x < 50; x++)
            {
                var path = GeneratePath();
                using (Draw.WithDuration(renderDuration))
                {
                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        Draw.Line(path[i], path[i + 1], Color.blue);
                    }
                }
            }
        }
    }
}
