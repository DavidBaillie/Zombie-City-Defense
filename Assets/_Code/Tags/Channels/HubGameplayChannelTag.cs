using Assets.Tags.Abstract;
using Assets.Tags.Collections;
using System;
using UnityEngine;

namespace Assets.Tags.Channels
{
    /// <summary>
    /// Communication channel tag used for communication between assets in the context of the hub gameplay scene/space
    /// </summary>
    [CreateAssetMenu(menuName = ChannelAssetBaseName + "Hub Channel")]
    public class HubGameplayChannelTag : AChannel
    {
        public event Action<CombatPlayspaceDataTag> OnUserSelectedPlayspaceWaypoint;


        public void RaiseOnUserSelectedPlayspaceWaypoint(CombatPlayspaceDataTag data) => OnUserSelectedPlayspaceWaypoint?.Invoke(data);
    }
}
