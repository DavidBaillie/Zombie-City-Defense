using Assets.Core.Models;
using Assets.Tags.Abstract;
using System;
using UnityEngine;

namespace Assets.Tags.Channels
{
    [CreateAssetMenu(menuName = ChannelAssetBaseName + "Player Actions", fileName = "Player Actions Channel")]
    public class PlayerActionChannel : AChannel
    {
        public event Action<WorldPosition> OnPlayerSelectedWorldPosition;
        public event Action<Vector2> OnPlayerSelectedInvalidPosition;


        public void RaiseOnPlayerSelectedWorldPosition(WorldPosition position) => OnPlayerSelectedWorldPosition?.Invoke(position);
        public void RaiseOnPlayerSelectedInvalidPosition(Vector2 screenPosition) => OnPlayerSelectedInvalidPosition?.Invoke(screenPosition);    
    }
}
