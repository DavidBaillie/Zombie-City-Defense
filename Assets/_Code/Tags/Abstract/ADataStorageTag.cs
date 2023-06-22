using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.Abstract
{
    public abstract class ADataStorageTag : ATag
    {
        protected const string StorageAssetMenuBaseName = AssetMenuBaseName + "Storage/";

        protected bool HasLoadedFromStorage = false;

        [SerializeField, ValidateInput(nameof(fileNameIsInvalid), "Must provide a file name to save under")]
        protected string FileSaveName = "";
        private bool fileNameIsInvalid => !string.IsNullOrEmpty(FileSaveName);


        public abstract bool TryLoadFromStorage();
        public abstract bool TrySaveToStorage();
    }
}
