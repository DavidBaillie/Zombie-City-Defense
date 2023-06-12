using System;
using UnityEngine;

namespace Assets.Core.StaticChannels
{
    public static class PlayerActionChannel
    {
        public static event Action<Vector3> OnPlayerSelectedWorldPosition;
        public static event Action<Vector2> OnPlayerSelectedInvalidPosition;


        public static void RaiseOnPlayerSelectedWorldPosition(Vector3 position) => OnPlayerSelectedWorldPosition?.Invoke(position);
        public static void RaiseOnPlayerSelectedInvalidPosition(Vector2 screenPosition) => OnPlayerSelectedInvalidPosition?.Invoke(screenPosition);
    }
}
