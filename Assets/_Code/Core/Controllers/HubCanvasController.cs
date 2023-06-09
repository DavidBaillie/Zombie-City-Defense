using Assets.Tags.Channels;
using Assets.Tags.Collections;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Assets.Core.Controllers
{
    /// <summary>
    /// Canvas controller for the hub game mode
    /// </summary>
    [SelectionBase]
    public class HubCanvasController : AExtendedMonobehaviour
    {
        [SerializeField, Required, FoldoutGroup("Waypoint Selection")]
        private CanvasGroup waypointSelectionGroup = null;

        [SerializeField, Required, FoldoutGroup("Waypoint Selection")]
        private TextMeshPro waypointSelectionText = null;


        [SerializeField, ReadOnly]
        private HubGameplayChannelTag gameplayChannel = null;


        /// <summary>
        /// Called when object destroyed
        /// </summary>
        protected override void OnDestroy()
        {
            if (gameplayChannel != null ) 
                gameplayChannel.OnUserSelectedPlayspaceWaypoint -= OnUserSelectedWorldWaypoint;
        }

        /// <summary>
        /// Sets up needed references for this component
        /// </summary>
        /// <param name="gameplayChannel">Gameplay channel to use</param>
        public void Setup(HubGameplayChannelTag gameplayChannel)
        {
            this.gameplayChannel = gameplayChannel;
            this.gameplayChannel.OnUserSelectedPlayspaceWaypoint += OnUserSelectedWorldWaypoint;
        }

        /// <summary>
        /// Called when the user selected a waypoint in the game world
        /// </summary>
        /// <param name="playspaceWaypoint"></param>
        private void OnUserSelectedWorldWaypoint(CombatPlayspaceDataTag playspaceWaypoint)
        {

        }

        
    }
}
