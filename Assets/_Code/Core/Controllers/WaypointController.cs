using Drawing;
using Game.Utilities.BaseObjects;
using UnityEngine;

namespace Game.Core.Controllers
{
    public class WaypointController : AExtendedMonobehaviour
    {
        [SerializeField]
        private float trackingWidth;

        /// <summary>
        /// Draws debug data in the game
        /// </summary>
        public override void DrawGizmos()
        {
            base.DrawGizmos();

            Draw.Line(GetMaxPosition(), GetMinPosition(), Color.red);
        }

        /// <summary>
        /// Returns a valid position aling the waypoint line randomly between the min and max
        /// </summary>
        public Vector3 GetRandomPosition() => transform.position + (transform.right.normalized * trackingWidth * Random.Range(-1f, 1f));
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
