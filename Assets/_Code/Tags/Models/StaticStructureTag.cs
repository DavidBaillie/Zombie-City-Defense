using Assets.Core.Abstract;
using Assets.Core.Controllers;
using Assets.Core.Models;
using Assets.Tags.Abstract;
using UnityEngine;

namespace Assets.Tags.Models
{
    [CreateAssetMenu(menuName = UnitAssetMenuBaseName + "Structure Unit", fileName = "Structure Unit")]
    public class StaticStructureTag : AUnitTag
    {
        public float MaxHealth => BaseHealth;

        public override AEntityController SetupController(WorldPosition worldPosition, GameObject spawnedEntity)
        {
            if (spawnedEntity.TryGetComponent(out StaticStructureEntityController controller))
            {
                controller.SetupController(worldPosition.Id, this);
                return controller;
            }

            LogError($"Failed to setup entity controller for {name} because it does not have the required {nameof(StaticStructureEntityController)}");
            return null;
        }
    }
}
