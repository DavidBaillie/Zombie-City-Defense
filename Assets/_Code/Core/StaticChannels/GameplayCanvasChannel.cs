using Assets.Tags.Common;
using System;

namespace Assets.Core.StaticChannels
{
    public static class GameplayCanvasChannel
    {
        public static event Action<StaticEntityIdentifier> OnUserSelectedStaticEntityPlacement;


        public static void RaiseOnUserSelectedStaticEntityPlacement(StaticEntityIdentifier entityIdentifier)
            => OnUserSelectedStaticEntityPlacement?.Invoke(entityIdentifier);
    }
}
