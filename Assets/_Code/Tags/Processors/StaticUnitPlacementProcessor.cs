using Assets.Core.Abstract;
using Assets.Core.DataTracking;
using Assets.Core.Models;
using Assets.Tags.Abstract;
using Assets.Tags.Channels;
using Assets.Tags.Collections;
using Assets.Tags.Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.Processors
{
    [CreateAssetMenu(menuName = ProcessorAssetBaseName + "Static Unit Placement")]
    public class StaticUnitPlacementProcessor : AProcessorTag
    {
        [SerializeField, Required]
        private PlayerActionChannel actionChannel = null;

        [SerializeField, Required]
        private GameplayCanvasChannel gameplayCanvasChannel = null;

        [SerializeField, Required]
        private EntityPrefabCollection prefabCollection = null;

        /// <summary>
        /// Called when the game is loaded to initialize state data
        /// </summary>
        public override void InitializeTag()
        {
            base.InitializeTag();

            actionChannel.OnPlayerSelectedWorldPosition += PlayerTappedOnWorldPosition;

            gameplayCanvasChannel.OnUserSelectedStaticEntityPlacement += OnUserSelectedStaticEntityPlacement;
        }

        /// <summary>
        /// Called when the action channel raises the event for when a user selects a world position
        /// </summary>
        /// <param name="position">WorldPosition data selected</param>
        private void PlayerTappedOnWorldPosition(WorldPosition position)
        {
            //If something is already at this point, ignore the action
            if (StaticEntityTracker.TryGetEntityById(position.Id, out var entity))
            {
                return;
            }

            //Slot is empty, allow for placement
            gameplayCanvasChannel.RaiseOnDisplayPlacementSelectionView(position);
        }

        /// <summary>
        /// Called when the gameplay canvas channel confirms an entity placement
        /// </summary>
        /// <param name="position">Where to place the entity</param>
        /// <param name="entityIdentifier">The entity to place</param>
        private void OnUserSelectedStaticEntityPlacement(WorldPosition position, StaticEntityIdentifier entityIdentifier)
        {
            //Check the spot is free
            if (StaticEntityTracker.EntityExistsAtPosition(position.Id))
            {
                LogError($"Cannot place entity at the requested position {position.Coordinate} because another entity is there!");
                return;
            }

            //Grab the prefab
            if (!prefabCollection.TryGetEntityPrefab(entityIdentifier, out var prefab))
            {
                LogError($"Could not spawn the desired static entity, none was found in the prefab collection!", entityIdentifier);
                return;
            }

            //Spawn the prefab at the position and run it's startup
            var instance = Instantiate(prefab, position.Coordinate, prefab.transform.rotation, null);
            var staticController = instance.GetComponent<AStaticEntity>();
            staticController?.AssignWorldPositionId(position.Id);
        }
    }
}
