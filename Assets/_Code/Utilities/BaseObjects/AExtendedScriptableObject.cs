namespace Game.Utilities.BaseObjects
{
    public abstract class AExtendedScriptableObject : Sirenix.OdinInspector.SerializedScriptableObject
    {
        protected void LogInformation(string message, UnityEngine.Object obj = null) => Assets.Utilities.Worker.Logger.LogInformation(GetType().Name, message, obj);
        protected void LogWarning(string message, UnityEngine.Object obj = null) => Assets.Utilities.Worker.Logger.LogWarning(GetType().Name, message, obj);
        protected void LogError(string message, UnityEngine.Object obj = null) => Assets.Utilities.Worker.Logger.LogError(GetType().Name, message, obj);

        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }

        public void MakeDirty()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
