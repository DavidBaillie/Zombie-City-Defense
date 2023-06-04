using Game.Tags.Common;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Assets.Tags.Abstract;

namespace Game.Tags.Settings
{
    [CreateAssetMenu(menuName = SettingsAssetMenuBaseName + "Preload Settings", fileName = "Preload Settings Tag")]
    public class PreloadSettingsTag : ASettingsTag
    {
        [SerializeField, AssetsOnly]
        public List<GameObject> PreloadPrefabs = new List<GameObject>();

        [SerializeField, AssetsOnly, InlineEditor]
        public List<ATag> InitializedTags = new List<ATag>();
    }
}
