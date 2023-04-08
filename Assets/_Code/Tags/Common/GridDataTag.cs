using System.Collections.Generic;
using UnityEngine;

namespace Game.Tags.Common
{
    [CreateAssetMenu(menuName = AssetMenuName + "Data/Grid Data", fileName = "Grid Data Tag")]
    public class GridDataTag : ATag
    {
        [SerializeField]
        private Vector2[] _worldPositions = new Vector2[0];
        public Vector2[] WorldPositions { get => _worldPositions; }

        public void SetWorldPositions(List<Vector2> positions) => _worldPositions = positions.ToArray();
        public void SetWorldPositions(Vector2[] worldPositions) => _worldPositions = worldPositions;
    }
}
