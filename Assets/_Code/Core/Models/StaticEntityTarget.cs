using Assets.Core.Interfaces;
using UnityEngine;

namespace Assets.Core.Models
{
    public struct StaticEntityTarget
    {
        public GameObject TargetGameObject { get; set; }
        public IDamageReceiver DamageReceiver { get; set; }

        public bool HasTarget => TargetGameObject != null;

        /// <summary>
        /// Base constructor
        /// </summary>
        public StaticEntityTarget(int nu = 0)
        {
            TargetGameObject = null;
            DamageReceiver = null;
        }

        /// <summary>
        /// Assigned object constructor
        /// </summary>
        /// <param name="target">GameObject reference</param>
        /// <param name="receiver">Component damage receiver</param>
        /// <param name="position">Point to seek in world space</param>
        public StaticEntityTarget(GameObject target, IDamageReceiver receiver)
        {
            TargetGameObject = target;
            DamageReceiver = receiver;
        }
    }
}
