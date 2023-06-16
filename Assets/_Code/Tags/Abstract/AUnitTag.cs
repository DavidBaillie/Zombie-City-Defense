using Assets.Core.Abstract;
using Assets.Core.Models;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.Abstract
{
    public abstract class AUnitTag : ATag
    {
        protected const string UnitAssetMenuBaseName = AssetMenuBaseName + "Units/";

        [SerializeField, ValidateInput(nameof(displayNameIsValid), "Unit must have a name")]
        public string DisplayName;
        private bool displayNameIsValid => !string.IsNullOrWhiteSpace(DisplayName);

        [SerializeField, AssetsOnly, ValidateInput(nameof(prefabIsValid), "Prefab is required and must have a valid static entity controller."), BoxGroup("Stats")]
        public GameObject UnitPrefab = null;
        private bool prefabIsValid => UnitPrefab != null && UnitPrefab.TryGetComponent<AEntityController>(out _);

        [SerializeField, MinValue(1), BoxGroup("Base")]
        protected float BaseHealth = 100f;


        public abstract AEntityController SetupController(WorldPosition worldPosition, GameObject spawnedEntity);

        public override int GetHashCode() => Id.GetHashCode();
        public override string ToString() => $"[{name} - {DisplayName}]";
    }
}
