using Assets.Core.Models;
using Assets.Utilities.Worker;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tags.Abstract
{
    public abstract class AGridDataProvider : ATag
    {
        /// <summary>
        /// Actively loaded instance for the grid data to be used during gameplay
        /// </summary>
        public static AGridDataProvider ActiveInstance { get; protected set; }

        /// <summary>
        /// Assigns the provided value to the active instance and raised an event to inform others
        /// </summary>
        /// <param name="provider">New value for instance</param>
        public static void SetActiveDataProvider(AGridDataProvider provider)
        {
            try { ProviderChanged?.Invoke(ActiveInstance, provider); } catch (Exception e) 
            { Utilities.Worker.Logger.LogError(nameof(AGridDataProvider), $"Failed to raise event during instance assignment: {e}"); }

            ActiveInstance = provider;
        }

        /// <summary>
        /// Raised when the current data provider has been changed by an external body
        /// < Old / New >
        /// </summary>
        public static event Action<AGridDataProvider, AGridDataProvider> ProviderChanged;

        public abstract Vector3[] WorldPositionsArray { get; }
        public abstract List<Vector3> WorldPositionsList { get; }

        public abstract void SetWorldPositions(IEnumerable<Vector3> positions);
        public abstract bool TryGetClosestGridPosition(Vector3 source, out WorldPosition bestPosition, float maxDistance = 1f);
        public abstract bool TryGetGridPositionsWithinRange(Vector3 source, out List<WorldPosition> gridPointsInRange, float range);
        public abstract bool TryGetGridPositionById(Guid id, out WorldPosition coordinate);
    }
}
