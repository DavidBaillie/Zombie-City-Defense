using Assets.Core.Abstract;
using Assets.Core.DataTracking;
using Assets.Core.Models;
using Assets.Tags.Abstract;
using Assets.Tags.Collections;
using Sirenix.OdinInspector;
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
        public bool TryPlaceUnitAtWorldPosition(WorldPosition position, AStaticUnitInstance entity)
        {
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
            var staticController = instance.GetComponent<AStaticEntity>();

            staticController?.AssignWorldPositionId(position.Id);
            return true;
        }
    }
}
