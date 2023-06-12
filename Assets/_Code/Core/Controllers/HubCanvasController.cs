using Assets.Core.Managers.Static;
using Assets.Core.StaticChannels;
using Assets.Tags.Channels;
using Assets.Tags.Collections;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        private TextMeshProUGUI waypointSelectionText = null;


        [SerializeField, ReadOnly]
        private HubGameplayChannelTag gameplayChannel = null;

        private CombatPlayspaceDataTag selectedPlayspace = null;


        /// <summary>
        /// Sets up needed references for this component
        /// </summary>
        /// <param name="gameplayChannel">Gameplay channel to use</param>
        public void Setup(HubGameplayChannelTag gameplayChannel)
        {
            waypointSelectionGroup.alpha = 0;
            waypointSelectionText.text = string.Empty;

            this.gameplayChannel = gameplayChannel;
            this.gameplayChannel.OnUserSelectedPlayspaceWaypoint += OnUserSelectedWorldWaypoint;

            PlayerActionChannel.OnPlayerSelectedInvalidPosition += OnPlayerSelectedInvalidWorldPosition;
        }


        /// <summary>
        /// Called when object destroyed
        /// </summary>
        protected override void OnDestroy()
        {
            if (gameplayChannel != null ) 
                gameplayChannel.OnUserSelectedPlayspaceWaypoint -= OnUserSelectedWorldWaypoint;

            PlayerActionChannel.OnPlayerSelectedInvalidPosition -= OnPlayerSelectedInvalidWorldPosition;
        }

        /// <summary>
        /// Called when the user selected a waypoint in the game world
        /// </summary>
        /// <param name="playspaceWaypoint"></param>
        private void OnUserSelectedWorldWaypoint(CombatPlayspaceDataTag playspaceWaypoint)
        {
            selectedPlayspace = playspaceWaypoint;
            waypointSelectionGroup.alpha = 1;
            waypointSelectionText.text = playspaceWaypoint.DisplayName;
        }

        /// <summary>
        /// Called when the user selects an invalid world position
        /// </summary>
        /// <param name="screenPosition"></param>
        private void OnPlayerSelectedInvalidWorldPosition(Vector2 screenPosition)
        {
            selectedPlayspace = null;
            waypointSelectionGroup.alpha = 0;
            waypointSelectionText.text = string.Empty;
        }

        /// <summary>
        /// Called from a UnityEvent on the canvas to load the desired level
        /// </summary>
        public void LoadSelectedLevel()
        {
            if (selectedPlayspace == null)
            {
                LogError($"Cannot load level from button press when no level has been selected!");
                return;
            }

            LogInformation($"Calling scene manager to change scenes!");
            GameManagers.SceneManager.LoadScene(selectedPlayspace.combatScene);
        }
    }
}
