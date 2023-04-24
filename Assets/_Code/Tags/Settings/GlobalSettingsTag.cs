using Assets.Tags.Abstract;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Tags.Settings
{
    public class GlobalSettingsTag : ATag
    {
        [HideInInspector]
        public static GlobalSettingsTag Instance;


        [ShowInInspector, Required, AssetsOnly]
        public GameObject userSelectedPositionPrefab = null;



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
    }
}
