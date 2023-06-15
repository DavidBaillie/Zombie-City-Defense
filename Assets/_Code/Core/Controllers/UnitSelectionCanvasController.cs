using Assets.Core.Abstract;
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


        private GameplayCanvasController parentController = null;
        private StaticUnitTag unit = null;


        public void AssignUnit(StaticUnitTag unit, GameplayCanvasController parent)
        {
            unit.ThrowIfNull("Cannot assign unit to the UnitSelectionController because the provided unit is null!");
            parent.ThrowIfNull("Cannot assign unit to the UnitSelectionController because the provided parent controller is null!");

            //TODO - build unit visual here
            this.unit = unit;
            fieldText.text = unit.DisplayName;
            parentController = parent;
        }

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

        public void OnPressed()
        {
            parentController?.OnUserPressedUnitButton(unit);
        }
    }
}
