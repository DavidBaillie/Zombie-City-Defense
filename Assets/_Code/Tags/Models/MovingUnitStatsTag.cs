using Assets.Tags.Abstract;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Tags.Models
{
    [CreateAssetMenu(menuName = UnitAssetMenuBaseName + "Moving Unit", fileName = "Moving Unit Stats")]
    public class MovingUnitStatsTag : AUnitTag
    {
        [SerializeField, MinValue(1)]
        public float MaxHealth = 1f;

        [SerializeField, MinValue(0), SuffixLabel("m/s", Overlay = true)]
        public float MovementSpeed = 5f;

        [SerializeField, MinValue(0)]
        public float AttackDamage = 1f;

        [SerializeField, MinValue(0), SuffixLabel("seconds", Overlay = true)]
        public float AttackCooldown = 1f;

        [SerializeField, MinValue(0), SuffixLabel("m", Overlay = true)]
        public float attackRange = 0.5f;

        [SerializeField, MinValue(0), SuffixLabel("m", Overlay = true)]
        public float sightRange = 8f;

        [SerializeField]
        public LayerMask ValidTargetLayers;

        [SerializeField]
        public LayerMask SightBlockingLayers;
    }
}
