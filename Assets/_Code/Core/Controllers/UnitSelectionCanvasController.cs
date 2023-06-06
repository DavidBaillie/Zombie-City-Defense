﻿using Assets.Core.Abstract;
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
        private AStaticUnitInstance unit = null;


        public void AssignUnit(AStaticUnitInstance unit, GameplayCanvasController parent)
        {
            //TODO - build unit visual here
            this.unit = unit;
            fieldText.text = unit.DisplayName;
            parentController = parent;

            LogInformation($"Button state -> {button} on {gameObject}");
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
                LogInformation($"Set button as disabled -> {gameObject}");
            }
        }

        public void OnPressed()
        {
            parentController?.OnUserPressedUnitButton(unit);
        }
    }
}