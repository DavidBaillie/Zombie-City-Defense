using Assets.Core.Managers.Static;
using Assets.Tags.Abstract;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Tags.Common
{
    [CreateAssetMenu(menuName = ManagerAssetMenuBaseName + "Scene Manager", fileName = "Scene Manager")]
    public class SceneLoadingManager : ASceneLoadingManager
    {
        /// <summary>
        /// Sets up the tag
        /// </summary>
        public override void InitializeTag()
        {
            base.InitializeTag();
            
            if (GameManagers.SceneManager != null)
            {
                LogError($"Cannot initialize tag because another reference has been assigned to the static scene manager reference");
                return;
            }

            GameManagers.SceneManager = this;
        }

        /// <summary>
        /// Cleans up the tag when no longer needed
        /// </summary>
        public override void CleanupTag()
        {
            base.CleanupTag();

            if (GameManagers.SceneManager == this)
                GameManagers.SceneManager = null;
        }

        /// <summary>
        /// Handles loading a scene correctly
        /// </summary>
        /// <param name="sceneName">Scene to load</param>
        public override void LoadScene(string sceneName)
        {
            //TODO - Process loading screen
            SceneManager.LoadScene(sceneName);
        }
    }
}
