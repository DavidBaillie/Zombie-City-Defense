using Assets.Core.Abstract;
using Assets.Core.Controllers;
using Assets.Core.Models;
using Assets.Tags.Abstract;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.Models
{
    [CreateAssetMenu(menuName = UnitAssetMenuBaseName + "Static Unit", fileName = "NEW STATIC UNIT")]
    public class StaticUnitTag : AUnitTag
    {
        [SerializeField, MinValue(1), BoxGroup("Stats")]
        public int Level = 1;

        [SerializeField, BoxGroup("Stats")]
        public LayerMask ValidTargetLayers;

        [SerializeField, BoxGroup("Stats")]
        public LayerMask SightBlockingLayers;

        

        [SerializeField, MinValue(0), BoxGroup("Base")]
        protected float BaseAttackDamage = 1f;

        [SerializeField, MinValue(0), SuffixLabel("seconds", Overlay = true), BoxGroup("Base")]
        protected float BaseAttackCooldown = 1f;

        [SerializeField, MinValue(0), SuffixLabel("metres", Overlay = true), BoxGroup("Base")]
        protected float BaseAttackRange = 5f;

        [SerializeField, MinValue(0), SuffixLabel("per level", Overlay = true), BoxGroup("Level Improvements")]
        protected float HealthGrowthFactor = 0.1f;

        [SerializeField, MinValue(0), SuffixLabel("per level", Overlay = true), BoxGroup("Level Improvements")]
        protected float DamageGrowthFactor = 0.1f;

        [SerializeField, MinValue(0), SuffixLabel("per level", Overlay = true), BoxGroup("Level Improvements")]
        protected float CooldownGrowthFactor = 0.1f;

        [SerializeField, MinValue(0), SuffixLabel("per level", Overlay = true), BoxGroup("Level Improvements")]
        protected float RangeGrowthFactor = 0.1f;



        public float MaxHealth => BaseHealth + (Level * HealthGrowthFactor);
        public float AttackCooldown => BaseAttackCooldown + (Level * CooldownGrowthFactor);
        public float AttackDamage => BaseAttackDamage + (Level * DamageGrowthFactor);
        public float AttackRange => BaseAttackRange + (Level * RangeGrowthFactor);
    }
}
