using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Tags.Common
{
    [CreateAssetMenu(menuName = AssetMenuName + "Data/Grid Data", fileName = "Grid Data Tag")]
    [Serializable]
    public class GridDataTag : ATag
    {
        [SerializeField]
        private Vector3[] _worldPositions = new Vector3[0];
        public Vector3[] WorldPositions { get => _worldPositions; }

        public void SetWorldPositions(IEnumerable<Vector3> positions)
        {
            _worldPositions = positions.ToArray();
            MakeDirty();
        }
    }
}
