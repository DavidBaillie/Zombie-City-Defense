using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tags.Common
{
    public static class SceneObjectRegistry
    {
        private static Dictionary<ObjectTypeIdentifier, List<GameObject>> registry = new Dictionary<ObjectTypeIdentifier, List<GameObject>>();

        /// <summary>
        /// Registers an object under the provided Id object
        /// </summary>
        /// <param name="id">Id to save under</param>
        /// <param name="gameObject">Object to add</param>
        public static void RegisterObject(ObjectTypeIdentifier id, GameObject gameObject)
        {
            if (!registry.ContainsKey(id))
                registry.Add(id, new List<GameObject>());

            registry[id].Add(gameObject);
        }

        /// <summary>
        /// Deregisters an object under the provided Id object
        /// </summary>
        /// <param name="id">Id to remove under</param>
        /// <param name="gameObject">object to remove</param>
        public static void DeregisterObject(ObjectTypeIdentifier id, GameObject gameObject)
        {
            if (!registry.ContainsKey(id))
                return;

            registry[id].RemoveAll(x => x == null || x == gameObject);
        }

        /// <summary>
        /// Tries to get all the objects under the provided Id object
        /// </summary>
        /// <param name="id">Id to load by</param>
        /// <param name="objects">Objects available, returns as out param</param>
        /// <returns>If objects were returned</returns>
        public static bool TryGetObjectsById(ObjectTypeIdentifier id, out List<GameObject> objects)
        {
            if (registry.ContainsKey(id))
            {
                objects = new(registry[id].RemoveAll(x => x == null));
                return objects.Count > 0;
            }
            else
            {
                objects = new();
                return false;
            }
        }
    }
}
