using Assets.Core.Abstract;
using Assets.Core.Controllers;
using Assets.Core.Models;
using Assets.Tags.Abstract;
using UnityEngine;

namespace Assets.Tags.Models
{
    [CreateAssetMenu(menuName = UnitAssetMenuBaseName + "Objective Unit", fileName = "Objective Unit")]
    public class ObjectiveUnitTag : AUnitTag
    {
        public float MaxHealth => BaseHealth;


        public override AEntityController SetupController(WorldPosition worldPosition, GameObject spawnedEntity)
        {
            if (spawnedEntity.TryGetComponent(out StaticObjectiveEntityController controller))
            {
                return controller;
            }

            LogError($"Could not setup the controller for [{name}] because it lacks a {nameof(StaticObjectiveEntityController)}");
            return null;
        }
    }
}
