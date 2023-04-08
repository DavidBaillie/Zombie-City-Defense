using Game.Tags.Settings;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core.Controllers
{
    public class GamePreloader : AExtendedMonobehaviour
    {
        public static string DevModeScene = string.Empty;

        [SerializeField, Required]
        private PreloadSettingsTag preloadSettings = null;

        /// <summary>
        /// Called when object is created, handles setting up required game logic
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            if (preloadSettings == null)
                LogError("Cannot initialize game, no settings have been provided!", gameObject);

            SetupDependencies();

            if (string.IsNullOrWhiteSpace(DevModeScene))
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                string sceneName = DevModeScene;
                DevModeScene = string.Empty;

                SceneManager.LoadScene(sceneName);
            }
        }

        /// <summary>
        /// Called to build the dependencies needed by the project
        /// </summary>
        private void SetupDependencies()
        {
            foreach (var prefab in preloadSettings.PreloadPrefabs)
            {
                var instance = Instantiate(prefab);
                DontDestroyOnLoad(instance);
            }
        }
    }
}
