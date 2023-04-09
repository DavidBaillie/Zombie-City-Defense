using Game.Tags.Common.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Tags.Common
{
    [CreateAssetMenu(menuName = AssetMenuName + "Data/Grid Data", fileName = "Grid Data Tag")]
    [Serializable]
    public class GridDataTag : AGridDataProvider
    {
        [SerializeField]
        private Vector3[] _worldPositions = new Vector3[0];
        public override Vector3[] WorldPositions { get => _worldPositions; }

        /// <summary>
        /// Overrides the current world data for the new input
        /// </summary>
        /// <param name="positions">All valid world positions</param>
        public override void SetWorldPositions(IEnumerable<Vector3> positions)
        {
            _worldPositions = positions.ToArray();
            MakeDirty();
        }

        /// <summary>
        /// Attempts to get the best world grid position within a specified distance of the source
        /// </summary>
        /// <param name="source">Source position to check from</param>
        /// <param name="bestPosition">Best match to return through</param>
        /// <param name="maxDistance">Max distance for a match</param>
        /// <returns>If a match was found</returns>
        public override bool TryGetClosestGridPosition(Vector3 source, out Vector3 bestPosition, float maxDistance = 1f)
        {
            //Default data
            bestPosition = Vector3.zero;
            if (WorldPositions == null || WorldPositions.Length < 1)
                return false;

            //Tracking vars
            Vector3? bestVector = null;
            float bestDistance = float.MinValue;

            //Check all coords
            foreach (var position in _worldPositions)
            {
                float distance = Vector3.Distance(source, position);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestVector = position;
                }
            }

            //Return state
            if (bestVector == null)
            {
                bestPosition = Vector3.zero;
                return false;
            }
            else
            {
                bestPosition = bestVector.Value;
                return true;
            }
        }

        /// <summary>
        /// Attempts to get all saved grid points within range of a given source point
        /// </summary>
        /// <param name="source">Source point to poll from</param>
        /// <param name="gridPointsInRange">out param used to return data</param>
        /// <param name="range">Max range away from source for a grid point to be valid</param>
        /// <returns>If any grid points were found within range</returns>
        public override bool TryGetGridPositionsWithinRange(Vector3 source, out List<Vector3> gridPointsInRange, float range)
        {
            //Default data
            gridPointsInRange = new List<Vector3>((int)(range * range));
            if (WorldPositions == null || WorldPositions.Length < 1)
                return false;

            //Check all coords
            foreach (var position in _worldPositions)
            {
                if (Vector3.Distance(position, source) < range)
                    gridPointsInRange.Add(position);
            }

            return gridPointsInRange.Count > 0;
        }
    }
}
