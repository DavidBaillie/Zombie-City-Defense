using Assets.Tags.Abstract;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.Models
{
    [CreateAssetMenu(menuName = UnitAssetMenuBaseName + "Objective Unit", fileName = "Objective Unit")]
    public class ObjectiveUnitTag : AUnitTag
    {
        [SerializeField, ReadOnly]
        public float MaxHealth => BaseHealth;
        protected override bool prefabIsValid => UnitPrefab != null;

        [SerializeField]
        public bool isPrimaryObjective = false;
    }
}
