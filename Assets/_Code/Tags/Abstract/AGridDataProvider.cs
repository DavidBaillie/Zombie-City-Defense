using Assets.Core.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tags.Abstract
{
    public abstract class AGridDataProvider : ATag
    {
        public abstract Vector3[] WorldPositionsArray { get; }
        public abstract List<Vector3> WorldPositionsList { get; }

        public abstract void SetWorldPositions(IEnumerable<Vector3> positions);
        public abstract bool TryGetClosestGridPosition(Vector3 source, out WorldPosition bestPosition, float maxDistance = 1f);
        public abstract bool TryGetGridPositionsWithinRange(Vector3 source, out List<WorldPosition> gridPointsInRange, float range);
    }
}
