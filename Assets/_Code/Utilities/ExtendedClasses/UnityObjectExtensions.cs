using System;

namespace Assets.Utilities.ExtendedClasses
{
    public static class UnityObjectExtensions
    {
        public static void ThrowIfNull(this UnityEngine.Object obj, string message)
        {
            if (obj == null)
                throw new ArgumentNullException(message);
        }
    }
}
