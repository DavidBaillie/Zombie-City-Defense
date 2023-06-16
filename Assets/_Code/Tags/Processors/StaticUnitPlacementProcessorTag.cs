using Assets.Core.Abstract;
using Assets.Core.Controllers;
using Assets.Core.DataTracking;
using Assets.Core.Interfaces;
using Assets.Core.Models;
using Assets.Tags.Abstract;
using Assets.Tags.Models;
using UnityEngine;

namespace Assets.Tags.Processors
{
    [CreateAssetMenu(menuName = ProcessorAssetBaseName + "Static Unit Placement")]
    public class StaticUnitPlacementProcessorTag : AProcessorTag
    {
        /// <summary>
        /// Called when the game is loaded to initialize state data
        /// </summary>
        public override void InitializeTag()
        { 
            base.InitializeTag();
        }

        /// <summary>
        /// Attempts to place the provided unit at the given position in the game world.
        /// </summary>
        /// <param name="position">Where to place the entity</param>
        /// <param name="unit">The entity to place</param>
        /// <returns>If a unit could be placed</returns>
        public bool TryPlaceStaticUnitAtWorldPosition(WorldPosition position, AUnitTag unit, out AEntityController controller)
        {
            controller = null;

            if (unit == null)
            {
                LogError($"Cannot try to place a unit at the world positon when a null entity was passed!");
                return false;
            }

            //Check the spot is free
            if (StaticEntityTracker.EntityExistsAtPosition(position.Id))
            {
                LogError($"Cannot place entity at the requested position {position.Coordinate} because another entity is there!");
                return false;
            }

            //Spawn prefab and try to setup
            GameObject prefabInstance = Instantiate(unit.UnitPrefab, position.Coordinate, unit.UnitPrefab.transform.rotation, null);
            if (!prefabInstance.TryGetComponent(out IDeployableEntity entity))
            {
                LogError($"Unit placement processor failed to place a unit because it does not have the required IDeployableEntity interface!");
                Destroy(prefabInstance); 
                return false;  
            }

            controller = entity.SetupController(position, unit);
            if (controller == null)
            {
                LogError($"Unit placement processor failed to place a unit because the setupController returned null!");
                Destroy(prefabInstance);
                return false;
            }

            return true;
        }
    }
}
