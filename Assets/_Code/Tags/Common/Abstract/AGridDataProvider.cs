using System.Collections.Generic;
using UnityEngine;

namespace Game.Tags.Common.Abstract
{
    public abstract class AGridDataProvider : ATag
    {
        public abstract Vector3[] WorldPositions { get; }
        public abstract void SetWorldPositions(IEnumerable<Vector3> positions);
        public abstract bool TryGetClosestGridPosition(Vector3 source, out Vector3 bestPosition, float maxDistance = 1f);
        public abstract bool TryGetGridPositionsWithinRange(Vector3 source, out List<Vector3> gridPointsInRange, float range);
    }
}
