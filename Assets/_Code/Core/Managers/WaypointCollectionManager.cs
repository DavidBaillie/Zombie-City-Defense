﻿using Drawing;
using Game.Core.Controllers;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Core.Managers
{
    /// <summary>
    /// Handles managing a collection of waypoints that an entity might path along
    /// </summary>
    public class WaypointCollectionManager : AExtendedMonobehaviour
    {
        [SerializeField, Range(0.001f, 1f)]
        private float waypointVariantThreshold = 0.15f;

        [SerializeField, ValidateInput(nameof(waypointsIsValid), "Waypoints cannot have empty/null values!")]
        private List<WaypointController> waypoints;
        private bool waypointsIsValid => !waypoints.Exists(x => x == null);

        [SerializeField, FoldoutGroup("Debug")]
        private float renderDuration = 3f;

        [SerializeField, FoldoutGroup("Debug")]
        private bool renderDebug = true;
        [SerializeField, FoldoutGroup("Debug")]
        private Color renderColour = Color.blue;


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
            float upper = 1, lower = -1;
            float relativeValue = UnityEngine.Random.Range(lower, upper);

            for (int i = 0; i < waypoints.Count; i++)
            {
                path.Add(waypoints[i].GetRandomPosition(out relativeValue, lower, upper));
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

            if (renderDebug && waypoints != null && waypoints.Count > 1)
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


        [Button("Preview Path")]
        private void PreviewPath()
        {
            for (int x = 0; x < 50; x++)
            {
                var path = GeneratePath();
                using (Draw.WithDuration(2))
                {
                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        Draw.Line(path[i], path[i + 1], renderColour);
                    }
                }
            }
        }
    }
}