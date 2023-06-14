using Assets.Core.Abstract;
using Assets.Core.Controllers;
using Assets.Core.DataTracking;
using Assets.Core.Models;
using Assets.Tags.Abstract;
using Assets.Tags.Channels;
using Assets.Tags.Collections;
using Assets.Tags.Models;
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
        /// <param name="unit">The entity to place</param>
        /// <returns>If a unit could be placed</returns>
        public bool TryPlaceHumanAtWorldPosition(WorldPosition position, StaticUnitTag unit, out HumanUnitController controller)
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

            //Spawn the prefab at the position and run it's startup
            var instance = Instantiate(unit.UnitPrefab, position.Coordinate, unit.UnitPrefab.transform.rotation, null);
            if (!instance.TryGetComponent(out controller))
            {
                LogError($"Static unit placement processor placed a unit that has no controller on it, please check prefab!", instance);
                return false;
            }

            controller.SetWorldPosition(position.Id);
            controller.SetUnitTag(unit);
            return true;
        }
    }
}
