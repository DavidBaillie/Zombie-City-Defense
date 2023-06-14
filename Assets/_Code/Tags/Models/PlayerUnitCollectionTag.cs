using Assets.Core.Abstract;
using Assets.Tags.Abstract;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tags.Models
{
    /// <summary>
    /// Class represents the collection of user owned units they have access to during gameplay
    /// </summary>
    [CreateAssetMenu(menuName = CollectionAssetMenuBaseName + "Player Units", fileName = "Player Units")]
    public class PlayerUnitCollectionTag : ACollectionTag
    {
        [SerializeField, ValidateInput(nameof(fileNameIsInvalid), "Must provide a file name to save under")]
        private string fileSaveName = "";
        private bool fileNameIsInvalid => !string.IsNullOrEmpty(fileSaveName);

        [SerializeField]
        public List<StaticUnitTag> availableUnits = new();


        public virtual bool TryLoadUnitsFromStorage()
        {
            //TODO - Load data from file
            return false;
        }

        public virtual bool TrySaveUnitsToStorage()
        {
            //TODO - Save units to storage
            return false;
        }
    }
}
