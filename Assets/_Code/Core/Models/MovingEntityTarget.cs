using Assets.Core.Interfaces;
using UnityEngine;

namespace Assets.Core.Models
{
    public struct MovingEntityTarget
    {
        public GameObject TargetGameObject { get; set; }
        public IDamageReceiver DamageReceiver { get; set; }
        public Vector3 TargetPosition { get; set; }

        public bool HasTarget => TargetGameObject != null;

        /// <summary>
        /// Base constructor
        /// </summary>
        public MovingEntityTarget(int nu = 0)
        {
            TargetGameObject = null;
            DamageReceiver = null;
            TargetPosition = Vector3.zero;
        }

        /// <summary>
        /// Assigned object constructor
        /// </summary>
        /// <param name="target">GameObject reference</param>
        /// <param name="receiver">Component damage receiver</param>
        /// <param name="position">Point to seek in world space</param>
        public MovingEntityTarget(GameObject target, IDamageReceiver receiver, Vector3 position)
        {
            TargetGameObject = target;
            DamageReceiver = receiver;
            TargetPosition = position;
        }
    }
}
