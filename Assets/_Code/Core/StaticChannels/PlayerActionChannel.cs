using Assets.Core.Models;
using System;
using UnityEngine;

namespace Assets.Core.StaticChannels
{
    public static class PlayerActionChannel
    {
        public static event Action<WorldPosition> OnPlayerSelectedWorldPosition;
        public static event Action<Vector2> OnPlayerSelectedInvalidPosition;


        public static void RaiseOnPlayerSelectedWorldPosition(WorldPosition position) => OnPlayerSelectedWorldPosition?.Invoke(position);
        public static void RaiseOnPlayerSelectedInvalidPosition(Vector2 screenPosition) => OnPlayerSelectedInvalidPosition?.Invoke(screenPosition);
    }
}
