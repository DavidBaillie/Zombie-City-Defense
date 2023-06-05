using Assets.Core.Abstract;
using Assets.Core.DataTracking;
using Assets.Core.Models;
using Assets.Tags.Abstract;
using Assets.Tags.Channels;
using Assets.Tags.Collections;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets.Tags.Processors
{
    [CreateAssetMenu(menuName = ProcessorAssetBaseName + "Static Unit Placement")]
    public class StaticUnitPlacementProcessorTag : AProcessorTag
    {
        [SerializeField, InlineEditor, Required]
        private EntityPrefabCollectionTag prefabCollection = null;


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
        /// <param name="entity">The entity to place</param>
        /// <returns>If a unit could be placed</returns>
        public bool TryPlaceUnitAtWorldPosition(WorldPosition position, AStaticUnitInstance entity, SurvivalGameplayChannelTag unitChannel, 
            out AStaticEntityController controller)
        {
            controller = null;

            if (entity == null)
            {
                LogError($"Cannot try to place a unit at the world positon when a null entity was passed!");
                return false;
            }

            if (entity.unitType == null)
            {
                LogError($"Cannot try to place entity [{entity.DisplayName}] because it has no identifier!");
            }

            if (unitChannel == null)
            {
                LogError($"Cannot place a unit for entity {entity.DisplayName} because no channel was provided for the unit.");
                return false;
            }

            //Check the spot is free
            if (StaticEntityTracker.EntityExistsAtPosition(position.Id))
            {
                LogError($"Cannot place entity at the requested position {position.Coordinate} because another entity is there!");
                return false;
            }

            //Grab the prefab
            if (!prefabCollection.TryGetEntityPrefab(entity.unitType, out var prefab))
            {
                LogError($"Could not spawn the desired static entity, none was found in the prefab collection!", entity.unitType);
                return false;
            }

            //Spawn the prefab at the position and run it's startup
            var instance = Instantiate(prefab, position.Coordinate, prefab.transform.rotation, null);
            controller = instance.GetComponent<AStaticEntityController>();

            if (controller == null)
            {
                LogError($"Static unit placement processor placed a unit that has no controller on it, please check prefab!", instance);
            }
            else
            {
                controller.AssignStateData(entity, position.Id, unitChannel);
            }

            return true;
        }
    }
}
