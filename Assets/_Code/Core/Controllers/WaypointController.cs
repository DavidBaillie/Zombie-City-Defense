using Assets.Debug;
using Drawing;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Core.Controllers
{
    public class WaypointController : AExtendedMonobehaviour
    {
        [SerializeField, MinValue(0)]
        private float trackingWidth;

        protected override void Update()
        {
            base.Update();
            DrawDebug();
        }

        /// <summary>
        /// Draws debug data in the game
        /// </summary>
        public override void DrawGizmos()
        {
            base.DrawGizmos();
            DrawDebug();
        }

        /// <summary>
        /// Handles drawing debug data in development builds
        /// </summary>
        private void DrawDebug()
        {
            GameplayDebugHandler.HandleRenderCall(() => Draw.Line(GetMaxPosition(), GetMinPosition(), Color.red), true, false);
        }

        /// <summary>
        /// Generates a a position for the waypoint within the bounds provided
        /// </summary>
        /// <param name="lowerBound">Lower bound of total width to generate point from</param>
        /// <param name="upperBound">Upper bound of total width to generate point from</param>
        /// <returns>A position on the waypoint</returns>
        public Vector3 GetRandomPosition(float lowerBound = -1, float upperBound = 1)
        {
            return GetRandomPosition(out float value, lowerBound, upperBound);
        }

        /// <summary>
        /// Generates a a position for the waypoint within the bounds provided
        /// </summary>
        /// <param name="lowerBound">Lower bound of total width to generate point from</param>
        /// <param name="upperBound">Upper bound of total width to generate point from</param>
        /// <returns>A position on the waypoint</returns>
        public Vector3 GetRandomPosition(out float relativePosition, float lowerBound = -1, float upperBound = 1)
        {
            lowerBound = math.max(-1, lowerBound);
            upperBound = math.min(1, upperBound);
            relativePosition = UnityEngine.Random.Range(lowerBound, upperBound);

            return transform.position + (transform.right.normalized * trackingWidth * relativePosition);
        }

        /// <summary>
        /// Returns the maximum vector a valid position for this waypoint can return
        /// </summary>
        public Vector3 GetMaxPosition() => transform.position + (transform.right.normalized * trackingWidth);
        /// <summary>
        /// Returns the minimum vector a valid position for this waypoint can return
        /// </summary>
        public Vector3 GetMinPosition() => transform.position - (transform.right.normalized * trackingWidth);
    }
}
