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
    }
}
