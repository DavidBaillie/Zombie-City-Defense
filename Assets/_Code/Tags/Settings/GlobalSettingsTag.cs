using Assets.Tags.Abstract;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Tags.Settings
{
    [CreateAssetMenu(menuName = SettingsAssetMenuBaseName + "Global Settings", fileName = "Global Settings")]
    public class GlobalSettingsTag : ASettingsTag
    {
        [HideInInspector]
        public static GlobalSettingsTag Instance;

        private static GlobalSettingsTag _devInstance = null;
        public static GlobalSettingsTag DevInstance
        {
            get
            {
                if (Instance != null)
                    return Instance;

                if (_devInstance == null)
                    _devInstance = Resources.Load<GlobalSettingsTag>("Tags/Settings/Global Settings");

                return _devInstance;
            }
        }

        public override void InitializeTag()
        {
            base.InitializeTag();

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                LogError($"More than one global settings tag has been generated and assigned, ignoring this tag instance value.");
            }
        }


        public bool DrawDebugLines = false;
        public bool DrawDebugText = false;
    }
}
