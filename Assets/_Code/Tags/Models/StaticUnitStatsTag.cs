using Game.Tags.Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Tags.Models
{
    [CreateAssetMenu(menuName = AssetMenuBaseName + "Stats/Static Unit", fileName = "Static Entity Stats")]
    public class StaticUnitStatsTag : ATag
    {
        [SerializeField, MinValue(0)]
        public float MaxHealth = 1f;

        [SerializeField, MinValue(0)]
        public float AttackDamage = 1f;

        [SerializeField, MinValue(0), SuffixLabel("seconds", Overlay = true)]
        public float AttackCooldown = 1f;

        [SerializeField, MinValue(0), SuffixLabel("metres", Overlay = true)]
        public float maxRange = 5f;

        [SerializeField]
        public LayerMask validTargetLayers;
    }
}
