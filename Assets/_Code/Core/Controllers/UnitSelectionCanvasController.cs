﻿using Assets.Core.Abstract;
using Game.Utilities.BaseObjects;
using TMPro;
using UnityEngine;

namespace Assets.Core.Controllers
{
    public class UnitSelectionCanvasController : AExtendedMonobehaviour
    {
        [SerializeField]
        private TextMeshProUGUI fieldText;


        public void AssignUnit(AStaticUnitInstance unit)
        {
            //TODO - build unit visual here
            fieldText.text = unit.DisplayName;
        }

        public void OnPressed()
        {

        }
    }
}
