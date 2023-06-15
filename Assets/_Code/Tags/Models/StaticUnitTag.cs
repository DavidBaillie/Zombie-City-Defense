using Assets.Core.Abstract;
using Assets.Core.Controllers;
using Assets.Tags.Abstract;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.Models
{
    [CreateAssetMenu(menuName = UnitAssetMenuBaseName + "Static Unit", fileName = "NEW STATIC UNIT")]
    public class StaticUnitTag : AUnitTag
    {
        [SerializeField, ValidateInput(nameof(displayNameIsValid), "Unit must have a name")]
        public string DisplayName;
        private bool displayNameIsValid => !string.IsNullOrWhiteSpace(DisplayName);

        [SerializeField, MinValue(1), BoxGroup("Stats")]
        public int Level = 1;

        [SerializeField, AssetsOnly, ValidateInput(nameof(prefabIsValid), "Prefab is required and must have a valid static entity controller."), BoxGroup("Stats")]
        public GameObject UnitPrefab = null;
        private bool prefabIsValid => UnitPrefab != null && UnitPrefab.TryGetComponent<StaticEntityController>(out _);

        [SerializeField, BoxGroup("Stats")]
        public LayerMask ValidTargetLayers;

        [SerializeField, MinValue(1), BoxGroup("Base Stats")]
        private float BaseHealth = 100f;

        [SerializeField, MinValue(0), BoxGroup("Base Stats")]
        private float BaseAttackDamage = 1f;

        [SerializeField, MinValue(0), SuffixLabel("seconds", Overlay = true), BoxGroup("Base Stats")]
        private float BaseAttackCooldown = 1f;

        [SerializeField, MinValue(0), SuffixLabel("metres", Overlay = true), BoxGroup("Base Stats")]
        private float BaseAttackRange = 5f;

        [SerializeField, MinValue(0), SuffixLabel("per level", Overlay = true), BoxGroup("Level Improvements")]
        private float HealthGrowthFactor = 0.1f;

        [SerializeField, MinValue(0), SuffixLabel("per level", Overlay = true), BoxGroup("Level Improvements")]
        private float DamageGrowthFactor = 0.1f;

        [SerializeField, MinValue(0), SuffixLabel("per level", Overlay = true), BoxGroup("Level Improvements")]
        private float CooldownGrowthFactor = 0.1f;

        [SerializeField, MinValue(0), SuffixLabel("per level", Overlay = true), BoxGroup("Level Improvements")]
        private float RangeGrowthFactor = 0.1f;



        public float MaxHealth => BaseHealth + (Level * HealthGrowthFactor);
        public float AttackCooldown => BaseAttackCooldown + (Level * CooldownGrowthFactor);
        public float AttackDamage => BaseAttackDamage + (Level * DamageGrowthFactor);
        public float AttackRange => BaseAttackRange + (Level * RangeGrowthFactor);


        public override int GetHashCode() => Id.GetHashCode();
        public override string ToString() => $"[{name} - {DisplayName}]";
    }
}
