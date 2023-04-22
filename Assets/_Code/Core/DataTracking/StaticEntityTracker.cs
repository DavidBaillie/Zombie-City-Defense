﻿using Assets.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Core.DataTracking
{
    public static class StaticEntityTracker
    {
        private static Dictionary<Guid, AStaticEntity> staticEntities = new Dictionary<Guid, AStaticEntity>();

        /// <summary>
        /// In case anything was left after being destroyed, clean up the dictionary
        /// </summary>
        private static void CleanUpEntities()
        {
            foreach (var kvp in staticEntities.Where(x => x.Value == null))
            {
                staticEntities.Remove(kvp.Key);
            }
        }


        /// <summary>
        /// Registers that an entity exists at a world position determined by Id
        /// </summary>
        /// <param name="placementId">World Position Id entity has been placed on</param>
        /// <param name="entity">Entity that was placed</param>
        public static void RegisterEntity(Guid placementId, AStaticEntity entity)
        {
            CleanUpEntities();

            if (staticEntities.ContainsKey(placementId))
            {
                Utilities.Worker.Logger.LogError(nameof(StaticEntityTracker),
                    $"Could not register an entity because another entity has been registered in slot [{placementId}]", entity);
                return;
            }

            staticEntities.Add(placementId, entity);
        }

        /// <summary>
        /// Removes the entity that was registered at the provided position Id
        /// </summary>
        /// <param name="placementId">World Position Id for entity</param>
        public static void DeregisterEntity(Guid placementId)
        {
            CleanUpEntities();

            if (!staticEntities.ContainsKey(placementId))
            {
                Utilities.Worker.Logger.LogWarning(nameof(StaticEntityTracker), 
                    $"Request to deregister entity [{placementId}] will take no action because no entity exists under that id");
                return;
            }

            staticEntities.Remove(placementId);
        }

        /// <summary>
        /// Attempts to get a static entity registered under the given Id
        /// </summary>
        /// <param name="placementId">World position Id</param>
        /// <param name="entity">Entity to return data through</param>
        /// <returns>If an entity could be found at the given Id</returns>
        public static bool TryGetEntityById(Guid placementId, out AStaticEntity entity)
        {
            CleanUpEntities();

            if (staticEntities.ContainsKey(placementId))
            {
                entity = staticEntities[placementId];
                return true;
            }
            else
            {
                entity = null;
                return false;
            }
        }

        /// <summary>
        /// Checks if an entity is registered at the provided position
        /// </summary>
        /// <param name="positionId">Position Id to check</param>
        /// <returns>If an entity exists at the position</returns>
        public static bool EntityExistsAtPosition(Guid positionId)
        {
            CleanUpEntities();
            return staticEntities.ContainsKey(positionId);
        }
    }
}
