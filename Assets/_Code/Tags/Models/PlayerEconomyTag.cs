using Assets.Tags.Abstract;
using UnityEngine;

namespace Assets.Tags.Models
{
    [CreateAssetMenu(menuName = StorageAssetMenuBaseName + "Economy", fileName = "Economy Storage")]
    public class PlayerEconomyTag : ADataStorageTag
    {
        private bool hasLoadedFromMemory = false;

        [SerializeField]
        public int AvailableScrap = 0;


        /// <summary>
        /// Handles setting up the tag for use
        /// </summary>
        public override void InitializeTag()
        {
            base.InitializeTag();

            if (!hasLoadedFromMemory)
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
        /// Tries to load the data from local storage
        /// </summary>
        /// <returns>If the load worked</returns>
        public override bool TryLoadFromStorage()
        {
            hasLoadedFromMemory = true;

            //TODO
            return true;
        }

        /// <summary>
        /// Tries to save the data to local storage
        /// </summary>
        /// <returns>If the save worked</returns>
        public override bool TrySaveToStorage()
        {
            //TODO
            return true;
        }
    }
}
