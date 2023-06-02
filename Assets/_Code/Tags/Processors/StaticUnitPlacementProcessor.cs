using Assets.Core.Abstract;
using Assets.Core.DataTracking;
using Assets.Core.Models;
using Assets.Core.StaticChannels;
using Assets.Tags.Abstract;
using Assets.Tags.Collections;
using Assets.Tags.Common;
using Game.Tags.Settings;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.Processors
{
    [CreateAssetMenu(menuName = ProcessorAssetBaseName + "Static Unit Placement")]
    public class StaticUnitPlacementProcessor : AProcessorTag
    {
        [SerializeField, Required]
        private EntityPrefabCollection prefabCollection = null;


        private GameObject highlightVisual = null;

        private StaticEntityIdentifier placementEntityId = null;
        private WorldPosition? lastValidTapPosition = null;


        /// <summary>
        /// Called when the game is loaded to initialize state data
        /// </summary>
        public override void InitializeTag()
        {
            base.InitializeTag();

            highlightVisual = Instantiate(GlobalSettingsTag.Instance.userSelectedPositionPrefab, Vector3.zero, Quaternion.identity, null);
            highlightVisual.SetActive(false);
            DontDestroyOnLoad(highlightVisual);

            PlayerActionChannel.OnPlayerSelectedWorldPosition += PlayerTappedOnWorldPosition;
        }

        /// <summary>
        /// Called when the action channel raises the event for when a user selects a world position
        /// </summary>
        /// <param name="position">WorldPosition data selected</param>
        private void PlayerTappedOnWorldPosition(WorldPosition position)
        {
            //Show the user where they selected
            highlightVisual.SetActive(true);
            highlightVisual.transform.position = position.Coordinate;

            //If something is already at this point, ignore the action
            if (StaticEntityTracker.TryGetEntityById(position.Id, out _))
                return;

            //Show the 
        }

        /// <summary>
        /// Shows the highlight visual at the given world position
        /// </summary>
        /// <param name="position">Where to place the visual</param>
        private void HighlightSelectedWorldPosition(WorldPosition position)
        {
            highlightVisual.SetActive(true);
            highlightVisual.transform.position = position.Coordinate;
        }

        /// <summary>
        /// Hides the highlight visual from view
        /// </summary>
        private void HideHighlightVisual()
        {
            highlightVisual.SetActive(false);
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
