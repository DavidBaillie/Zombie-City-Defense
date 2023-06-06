using Assets.Core.Abstract;
using Game.Utilities.BaseObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Core.Controllers
{
    public class UnitSelectionCanvasController : AExtendedMonobehaviour
    {
        [SerializeField]
        private TextMeshProUGUI fieldText = null;
        [SerializeField]
        private Button button = null;


        private GameplayCanvasController parentController = null;
        private AStaticUnitInstance unit = null;


        public void AssignUnit(AStaticUnitInstance unit, GameplayCanvasController parent)
        {
            //TODO - build unit visual here
            this.unit = unit;
            fieldText.text = unit.DisplayName;
            parentController = parent;
        }

        public void MarkVisualAsUsed()
        {
            button.interactable = false;
        }

        public void OnPressed()
        {
            parentController?.OnUserPressedUnitButton(unit);
        }
    }
}
