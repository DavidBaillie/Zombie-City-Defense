using UnityEngine;

namespace Assets.Utilities.Extensions
{
    public static class GetComponentExtensions
    {
        public static bool TryGetComponent<T>(this Component source, out T component) where T : Component
        {
            component = source.GetComponent<T>();
            return component != null;
        }

        public static bool TryGetComponent<T>(this GameObject source, out T component) where T : Component
        {
            component = source.GetComponent<T>();
            return component != null;
        }
    }
}
