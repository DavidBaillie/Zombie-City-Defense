using Assets.Core.Models;
using Assets.Tags.Abstract;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Tags.Common
{
    [CreateAssetMenu(menuName = AssetMenuBaseName + "Data/Grid Data", fileName = "Grid Data Tag")]
    [Serializable]
    public class GridDataTag : AGridDataProvider
    {
        [SerializeField, ReadOnly]
        private Dictionary<Guid, Vector3> _worldPositions = new();
        public override Vector3[] WorldPositionsArray { get => _worldPositions.Select(x => x.Value).ToArray(); }
        public override List<Vector3> WorldPositionsList { get => _worldPositions.Select(x => x.Value).ToList(); }


        /// <summary>
        /// Overrides the current world data for the new input
        /// </summary>
        /// <param name="positions">All valid world positions</param>
        public override void SetWorldPositions(IEnumerable<Vector3> positions)
        {
            _worldPositions.Clear();

            foreach (var pos in positions)
            {
                _worldPositions.Add(Guid.NewGuid(), pos);
            }

            MakeDirty();
        }

        /// <summary>
        /// Attempts to get the best world grid position within a specified distance of the source
        /// </summary>
        /// <param name="source">Source position to check from</param>
        /// <param name="bestPosition">Best match to return through</param>
        /// <param name="maxDistance">Max distance for a match</param>
        /// <returns>If a match was found</returns>
        public override bool TryGetClosestGridPosition(Vector3 source, out WorldPosition bestPosition, float maxDistance = 1f)
        {
            //Default data
            bestPosition = new(Guid.Empty, Vector3.zero);
            if (_worldPositions == null || _worldPositions.Count < 1)
                return false;

            //Tracking vars
            float bestDistance = float.MinValue;

            //Check all coords
            foreach (var position in _worldPositions)
            {
                float distance = Vector3.Distance(source, position.Value);

                if (distance < bestDistance && distance < maxDistance)
                {
                    bestDistance = distance;
                    
                    bestPosition.Id = position.Key;
                    bestPosition.Coordinate = position.Value;
                }
            }

            return bestPosition.Id != Guid.Empty;
        }

        /// <summary>
        /// Attempts to get all saved grid points within range of a given source point
        /// </summary>
        /// <param name="source">Source point to poll from</param>
        /// <param name="gridPointsInRange">out param used to return data</param>
        /// <param name="range">Max range away from source for a grid point to be valid</param>
        /// <returns>If any grid points were found within range</returns>
        public override bool TryGetGridPositionsWithinRange(Vector3 source, out List<WorldPosition> gridPointsInRange, float range)
        {
            //Default data
            gridPointsInRange = new List<WorldPosition>((int)(range * range));
            if (_worldPositions == null || _worldPositions.Count < 1)
                return false;

            //Check all coords
            foreach (var position in _worldPositions)
            {
                if (Vector3.Distance(position.Value, source) < range)
                    gridPointsInRange.Add(new(position.Key, position.Value));
            }

            return gridPointsInRange.Count > 0;
        }

        /// <summary>
        /// Tries to find and return a world position associate with the provided Id value
        /// </summary>
        /// <param name="id">Coordinate ID</param>
        /// <param name="coordinate">World position</param>
        /// <returns>If a position was found</returns>
        public override bool TryGetGridPositionById(Guid id, out WorldPosition coordinate)
        {
            if (_worldPositions.ContainsKey(id))
            {
                coordinate = new(id, _worldPositions[id]);
                return true;
            }
            else
            {
                coordinate = new(Guid.Empty, Vector3.zero);
                return false;
            }
        }
    }
}
