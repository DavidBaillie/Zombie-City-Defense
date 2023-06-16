using Assets.Core.Abstract;
using Assets.Core.StaticChannels;
using Assets.Tags.Abstract;
using Assets.Tags.Channels;
using Assets.Tags.Models;
using Assets.Utilities.ExtendedClasses;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Core.Controllers
{
    /// <summary>
    /// Controller class used to handle the state data on individual unit buttons
    /// </summary>
    [SelectionBase]
    public class UnitSelectionCanvasController : AExtendedMonobehaviour
    {
        [SerializeField, Required]
        private TextMeshProUGUI fieldText = null;
        [SerializeField, Required]
        private Button button = null;
        [SerializeField, Required]
        private Image healthBarImage = null;

        private GameplayCanvasController parentController = null;
        private AUnitTag representedUnit = null;

        /// <summary>
        /// Called when the component is enabled
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            SurvivalGameplayChannel.OnUnitHealthChanged += OnUnitHealthChanged;
        }

        
        /// <summary>
        /// Called when the component is disabled
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();
            SurvivalGameplayChannel.OnUnitHealthChanged -= OnUnitHealthChanged;
        }

        /// <summary>
        /// Called by the parent canvas to allow this component to dynamically represent a single unit
        /// </summary>
        /// <param name="unit">Unit to represent</param>
        /// <param name="parent">Parent controller</param>
        public void AssignUnit(AUnitTag unit, GameplayCanvasController parent)
        {
            unit.ThrowIfNull("Cannot assign unit to the UnitSelectionController because the provided unit is null!");
            parent.ThrowIfNull("Cannot assign unit to the UnitSelectionController because the provided parent controller is null!");

            //TODO - build unit visual here
            representedUnit = unit;
            fieldText.text = unit.DisplayName;
            parentController = parent;
            healthBarImage.fillAmount = 1f;
        }

        /// <summary>
        /// marks this visual as deployed into the game world when a unit is spawned
        /// </summary>
        public void MarkVisualAsUsed()
        {
            if (button == null)
            {
                LogError($"Unit selection controller failed to process marked visual, button has not been assigned.");
            }
            else
            {
                button.interactable = false;
            }
        }

        /// <summary>
        /// Called by a UnityEvent when the user taps the local button
        /// </summary>
        public void OnPressed()
        {
            parentController?.OnUserPressedUnitButton(representedUnit);
        }

        /// <summary>
        /// Event raised externally when the health value to represent needs to be updated
        /// </summary>
        /// <param name="unit">Unit with change</param>
        /// <param name="currentHealth">Current health value</param>
        /// <param name="maxHealth">Max unit health</param>
        private void OnUnitHealthChanged(AUnitTag unit, float currentHealth, float maxHealth)
        {
            if (unit == null || unit != representedUnit)
                return;

            healthBarImage.fillAmount = currentHealth / maxHealth;
        }
    }
}
