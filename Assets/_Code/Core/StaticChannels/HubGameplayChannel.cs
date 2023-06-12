using Assets.Tags.Collections;
using System;

namespace Assets.Tags.Channels
{
    public static class HubGameplayChannel
    {
        public static event Action<CombatPlayspaceDataTag> OnUserSelectedPlayspaceWaypoint;

        public static void RaiseOnUserSelectedPlayspaceWaypoint(CombatPlayspaceDataTag data) => OnUserSelectedPlayspaceWaypoint?.Invoke(data);
    }
}
