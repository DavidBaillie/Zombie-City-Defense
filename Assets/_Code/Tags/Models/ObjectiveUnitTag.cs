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
    }
}
