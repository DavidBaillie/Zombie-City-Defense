namespace Assets.Tags.Abstract
{
    public abstract class ASceneLoadingManager : AManager
    {
        public abstract void LoadScene(string sceneName);
        public abstract void LoadScene(SceneReference scene);
    }
}
