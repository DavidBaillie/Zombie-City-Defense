using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tags.Common
{
    public static class SceneObjectRegistry
    {
        private static Dictionary<ObjectTypeIdentifier, List<GameObject>> collectionRegistry = new Dictionary<ObjectTypeIdentifier, List<GameObject>>();
        private static Dictionary<ObjectTypeIdentifier, GameObject> singleObjectRegistry = new Dictionary<ObjectTypeIdentifier, GameObject>();

        /// <summary>
        /// Registers an object under the provided Id object
        /// </summary>
        /// <param name="id">Id to save under</param>
        /// <param name="gameObject">Object to add</param>
        public static void RegisterObjectInCollection(ObjectTypeIdentifier id, GameObject gameObject)
        {
            if (!collectionRegistry.ContainsKey(id))
                collectionRegistry.Add(id, new List<GameObject>());

            collectionRegistry[id].Add(gameObject);
        }

        /// <summary>
        /// Deregisters an object under the provided Id object
        /// </summary>
        /// <param name="id">Id to remove under</param>
        /// <param name="gameObject">object to remove</param>
        public static void DeregisterObjectFromCollection(ObjectTypeIdentifier id, GameObject gameObject)
        {
            if (!collectionRegistry.ContainsKey(id))
                return;

            collectionRegistry[id].RemoveAll(x => x == null || x == gameObject);
        }

        /// <summary>
        /// Registers an object to be saved by Id
        /// </summary>
        /// <param name="id">Id to save under</param>
        /// <param name="gameObject">Object to save</param>
        public static void RegisterObject(ObjectTypeIdentifier id, GameObject gameObject)
        {
            if (singleObjectRegistry.ContainsKey(id))
                singleObjectRegistry[id] = gameObject;
            else 
                singleObjectRegistry.Add(id, gameObject);
        }

        /// <summary>
        /// Deregisters an object from the registry based on the given id
        /// </summary>
        /// <param name="id">Object Id</param>
        /// <param name="gameObject">GameObject to save</param>
        public static void DeregisterObject(ObjectTypeIdentifier id, GameObject gameObject)
        {
            singleObjectRegistry.Remove(id);
        }

        /// <summary>
        /// Tries to get all the objects under the provided Id object
        /// </summary>
        /// <param name="id">Id to load by</param>
        /// <param name="objects">Objects available, returns as out param</param>
        /// <returns>If objects were returned</returns>
        public static bool TryGetObjectsById(ObjectTypeIdentifier id, out List<GameObject> objects)
        {
            if (collectionRegistry.ContainsKey(id))
            {
                objects = new(collectionRegistry[id].RemoveAll(x => x == null));
                return objects.Count > 0;
            }
            else
            {
                objects = new();
                return false;
            }
        }

        /// <summary>
        /// Tries to get a GameObject by the given Id, returns value through out param
        /// </summary>
        /// <param name="id">Id to lookup</param>
        /// <param name="gameObject">returned object</param>
        /// <returns>If the object was found</returns>
        public static bool TryGetObjectById(ObjectTypeIdentifier id, out GameObject gameObject)
        {
            if (singleObjectRegistry.ContainsKey(id))
            {
                gameObject = singleObjectRegistry[id];
                return true;
            }
            else
            {
                gameObject = null;
                return false;
            }
        }
    }
}
