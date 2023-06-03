using Assets.Core.Abstract;
using Game.Utilities.BaseObjects;
using TMPro;
using UnityEngine;

namespace Assets.Core.Controllers
{
    public class UnitSelectionCanvasController : AExtendedMonobehaviour
    {
        [SerializeField]
        private TextMeshProUGUI fieldText;


        private GameplayCanvasController parentController = null;


        public void AssignUnit(AStaticUnitInstance unit, GameplayCanvasController parent)
        {
            //TODO - build unit visual here
            fieldText.text = unit.DisplayName;
            parentController = parent;
        }

        public void OnPressed()
        {

        }
    }
}
