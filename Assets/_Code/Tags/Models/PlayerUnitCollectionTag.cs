﻿using Assets.Tags.Abstract;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tags.Models
{
    /// <summary>
    /// Class represents the collection of user owned units they have access to during gameplay
    /// </summary>
    [CreateAssetMenu(menuName = StorageAssetMenuBaseName + "Player Units", fileName = "Player Units")]
    public class PlayerUnitCollectionTag : ADataStorageTag
    {
        [SerializeField]
        public List<StaticUnitTag> AvailableUnits = new();

        [SerializeField]
        public List<StaticUnitTag> LivingUnits = new();

        [SerializeField]
        public List<StaticUnitTag> DeadUnits = new();

        /// <summary>
        /// Handles setting the tag up for use
        /// </summary>
        public override void InitializeTag()
        {
            base.InitializeTag();

            if (!HasLoadedFromStorage)
                TryLoadFromStorage();
        }

        /// <summary>
        /// Handles cleaning up the tag when no longer needed
        /// </summary>
        public override void CleanupTag()
        {
            base.CleanupTag();
            TrySaveToStorage();
        }


        /// <summary>
        /// Tries to load data from the local storage device
        /// </summary>
        /// <returns>If the load worked</returns>
        public override bool TryLoadFromStorage()
        {
            HasLoadedFromStorage = true;

            //TODO - Load data from file
            return true;
        }

        /// <summary>
        /// Tries to save data to the local storage device
        /// </summary>
        /// <returns>If the save worked</returns>
        public override bool TrySaveToStorage()
        {
            //TODO - Save units to storage
            return true;
        }
    }
}
