using Assets.Tags.Abstract;
using Assets.Tags.Common;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tags.Collections
{
    [CreateAssetMenu(menuName = CollectionTagAssetMenu + "Entity Prefabs", fileName = "Entity Prefab Collection")]
    public class EntityPrefabCollection : ACollectionTag
    {
        [SerializeField]
        private Dictionary<StaticEntityIdentifier, GameObject> staticEntities = new Dictionary<StaticEntityIdentifier, GameObject>();

        [Space(10)]

        [SerializeField]
        private Dictionary<DynamicEntityIdentifier, GameObject> dynamicEntities = new Dictionary<DynamicEntityIdentifier, GameObject>();

        /// <summary>
        /// Tries to return the prefab associated with the provided tag
        /// </summary>
        /// <param name="identifier">Identifier to select by</param>
        /// <param name="prefab">Prefab assigned to id</param>
        /// <returns>If a prefab was found</returns>
        public bool TryGetEntityPrefab(StaticEntityIdentifier identifier, out GameObject prefab)
        {
            if (staticEntities.ContainsKey(identifier))
            {
                prefab = staticEntities[identifier];
                return true;
            }
            else
            {
                prefab = null;
                return false;
            }
        }

        /// <summary>
        /// Tries to return the prefab associated with the provided tag
        /// </summary>
        /// <param name="identifier">Identifier to select by</param>
        /// <param name="prefab">Prefab assigned to id</param>
        /// <returns>If a prefab was found</returns>
        public bool TryGetEntityPrefab(DynamicEntityIdentifier identifier, out GameObject prefab)
        {
            if (dynamicEntities.ContainsKey(identifier))
            {
                prefab = dynamicEntities[identifier];
                return true;
            }
            else
            {
                prefab = null;
                return false;
            }
        }
    }
}
