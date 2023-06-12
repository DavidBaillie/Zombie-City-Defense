using Game.Tags.Settings;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Core.Controllers
{
    public class GamePreloader : AExtendedMonobehaviour
    {
        public static string DevModeScene = string.Empty;

        [SerializeField, Required, InlineEditor]
        private PreloadSettingsTag preloadSettings = null;

        /// <summary>
        /// Called when object is created, handles setting up required game logic
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            if (preloadSettings == null)
            {
                LogError("Cannot initialize game, no settings have been provided!", gameObject);
                return;
            }

            preloadSettings.InitializeTag();

#if UNITY_EDITOR

            //Check for dev preload scene
            string devSceneName = GlobalSettingsTag.DevInstance.DevSceneOverride;
            GlobalSettingsTag.DevInstance.DevSceneOverride = string.Empty;

            //If dev mode, load it
            if (!string.IsNullOrWhiteSpace(devSceneName))
            {
                LogInformation($"Preload detected Dev Mode, loading scene [{devSceneName}].");

                SceneManager.LoadScene(devSceneName);
                return;
            }

#endif

            //Load primary scene
            LogInformation($"Preloading game with standards settings.");
            SceneManager.LoadScene(preloadSettings.MainGameScene);
        }
    }
}
