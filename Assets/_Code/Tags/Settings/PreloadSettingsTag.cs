using Game.Tags.Common;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Game.Tags.Settings
{
    [CreateAssetMenu(menuName = AssetMenuBaseName + "Settings/Preload Settings", fileName = "Preload Settings Tag")]
    public class PreloadSettingsTag : ATag
    {
        [SerializeField, AssetsOnly]
        public List<GameObject> PreloadPrefabs = new List<GameObject>();

        [SerializeField, AssetsOnly]
        public List<ATag> InitializedTags = new List<ATag>();
    }
}
