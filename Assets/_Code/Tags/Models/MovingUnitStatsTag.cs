using Game.Tags.Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Tags.Models
{
    [CreateAssetMenu(menuName = AssetMenuName + "Stats/Moving Unit", fileName = "Moving Unit Stats")]
    public class MovingUnitStatsTag : ATag
    {
        [SerializeField, MinValue(0), SuffixLabel("m/s", Overlay = true)]
        public float movementSpeed = 5f;

        [SerializeField, MinValue(0)]
        private float attackDamage = 1f;

        [SerializeField, MinValue(0), SuffixLabel("seconds", Overlay = true)]
        private float attackCooldown = 1f;
    }
}
