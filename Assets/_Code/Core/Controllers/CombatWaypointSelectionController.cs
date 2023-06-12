using Assets.Core.Interfaces;
using Assets.Tags.Channels;
using Assets.Tags.Collections;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Core.Controllers
{
    [SelectionBase]
    public class CombatWaypointSelectionController : AExtendedMonobehaviour, ISceneInteractable
    {
        [SerializeField, Required]
        private CombatPlayspaceDataTag playspaceData = null;


        /// <summary>
        /// Called when an external system wants to interact with this waypoint
        /// </summary>
        public void OnInteract()
        {
            HubGameplayChannel.RaiseOnUserSelectedPlayspaceWaypoint(playspaceData);
        }
    }
}
