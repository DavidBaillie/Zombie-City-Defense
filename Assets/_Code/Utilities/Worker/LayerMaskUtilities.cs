using UnityEngine;

namespace Assets.Utilities.Worker
{
    public static class LayerMaskUtilities
    {
        /// <summary>
        /// Determines if the given layer is part of the provided mask
        /// </summary>
        /// <param name="layer">Layer to check</param>
        /// <param name="layermask">Mask to compare against</param>
        /// <returns>If the layer is in the mask</returns>
        public static bool IsInLayerMask(int layer, LayerMask layermask)
        {
            return layermask == (layermask | (1 << layer));
        }
    }
}
