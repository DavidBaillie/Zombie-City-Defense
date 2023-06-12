using Game.Tags.Common;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Assets.Tags.Abstract;
using System.Linq;

namespace Game.Tags.Settings
{
    [CreateAssetMenu(menuName = SettingsAssetMenuBaseName + "Preload Settings", fileName = "Preload Settings Tag")]
    public class PreloadSettingsTag : ASettingsTag
    {
        [SerializeField, Required]
        public SceneReference MainGameScene = null;

        [SerializeField, AssetsOnly]
        public List<GameObject> PreloadPrefabs = new List<GameObject>();

        [SerializeField, AssetsOnly, InlineEditor]
        public List<ATag> InitializedTags = new List<ATag>();

        [SerializeField, AssetsOnly, InlineEditor, ValidateInput(nameof(SingletonIsValid), "Cannot have duplciates of the same tag!")]
        public List<ATag> SingletonTags = new List<ATag>();


        private bool SingletonIsValid => SingletonTags.Count < 1 || SingletonTags.Select(x => x.GetType()).Distinct().Count() == SingletonTags.Count;


        public override void InitializeTag()
        {
            base.InitializeTag();

            foreach (var prefab in PreloadPrefabs)
            {
                var instance = Instantiate(prefab);
                DontDestroyOnLoad(instance);
            }

            foreach (var tag in InitializedTags)
            {
                try { tag.InitializeTag(); } catch (System.Exception e) { LogError($"Failed to initialize [{tag?.name}]: {e.GetType().Name}"); }
            }

            foreach (var tag in SingletonTags)
            {
                try { tag.InitializeTag(); } catch (System.Exception e) { LogError($"Failed to initialize singleton [{tag?.name}]: {e.GetType().Name}"); }
            }
        }
    }
}
