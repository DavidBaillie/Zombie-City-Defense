using Assets.Core.Controllers;
using Game.Tags.Settings;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Game.Editor.Tools
{
    public static class EditorPreloader
    {
        private const string preloadEditorPrefName = "Game.Preload";
        public static bool ShouldPreload
        {
            get { return EditorPrefs.GetBool(preloadEditorPrefName, true); }
            set { EditorPrefs.SetBool(preloadEditorPrefName, value); }
        }

        private const string editorPreviousScene = "Game.PreviousScene";
        public static string PreviousScene
        {
            get { return EditorPrefs.GetString(editorPreviousScene, SceneManager.GetActiveScene().path); }
            set { EditorPrefs.SetString(editorPreviousScene, value); }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        static EditorPreloader()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        /// <summary>
        /// Called when the playmode state changes
        /// </summary>
        /// <param name="state">Playmode state</param>
        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                PreviousScene = SceneManager.GetActiveScene().path;

                //Grab the currently open scene and ask if the user wants to save
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    //Try to open the preload scene and then switch back to this one
                    try
                    {
                        if (!PreviousScene.EndsWith("Preload.unity"))
                            GlobalSettingsTag.DevInstance.DevSceneOverride = PreviousScene;

                        EditorSceneManager.OpenScene("Assets/_Scenes/Preload.unity"); 
                    }
                    catch (System.Exception e)
                    {
                        UnityEngine.Debug.Log($"Failed try/catch {e.GetType().Name}");
                        EditorApplication.isPlaying = false;
                    }
                }
                //User didn't want to save, don't play
                else
                {
                    UnityEngine.Debug.Log($"User declined save");
                    EditorApplication.isPlaying = false;
                }
            }

            // isPlaying check required because cannot OpenScene while playing
            if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                // User pressed stop -- reload previous scene.
                try
                {
                    EditorSceneManager.OpenScene(PreviousScene);
                }
                catch
                {
                    UnityEngine.Debug.LogError(string.Format("error: scene not found: {0}", PreviousScene));
                }
            }
        }
    }
}
