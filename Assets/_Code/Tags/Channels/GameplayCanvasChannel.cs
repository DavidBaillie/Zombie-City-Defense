using Assets.Core.Models;
using Assets.Tags.Abstract;
using Assets.Tags.Common;
using System;
using UnityEngine;

namespace Assets.Tags.Channels
{
    [CreateAssetMenu(menuName = ChannelAssetBaseName + "Gameplay Canvas", fileName = "Gameplay Canvas Channel")]
    public class GameplayCanvasChannel : AChannel
    {
        public event Action<StaticEntityIdentifier> OnUserSelectedStaticEntityPlacement;


        public void RaiseOnUserSelectedStaticEntityPlacement(StaticEntityIdentifier entityIdentifier) 
            => OnUserSelectedStaticEntityPlacement?.Invoke(entityIdentifier); 
    }
}
