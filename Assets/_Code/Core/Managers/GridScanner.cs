using Game.Tags.Common;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Core.Managers
{
    public class GridScanner : AExtendedMonobehaviour
    {
        [SerializeField, Required]
        private GridDataTag gridData = null;

        [SerializeField, BoxGroup("Grid Size")]
        private Vector2 minGridPosition = new(-100, -100);

        [SerializeField, BoxGroup("Grid Size")]
        private Vector2 maxGridPosition = new(100, 100);

        [SerializeField, MinValue(0.001), BoxGroup("Grid Size")]
        private float gridSize = 1;

        
    }
}
