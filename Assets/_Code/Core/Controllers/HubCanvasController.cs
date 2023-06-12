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

        private CombatPlayspaceDataTag selectedPlayspace = null;


        protected override void OnEnable()
        {
            base.OnEnable();
            waypointSelectionGroup.alpha = 0;
            waypointSelectionText.text = string.Empty;

            HubGameplayChannel.OnUserSelectedPlayspaceWaypoint += OnUserSelectedWorldWaypoint;
            PlayerActionChannel.OnPlayerSelectedInvalidPosition += OnPlayerSelectedInvalidWorldPosition;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            HubGameplayChannel.OnUserSelectedPlayspaceWaypoint -= OnUserSelectedWorldWaypoint;
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
