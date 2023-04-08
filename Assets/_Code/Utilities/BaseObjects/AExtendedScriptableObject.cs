using UnityEditor;
using UnityEngine;

namespace Game.Utilities.BaseObjects
{
    public abstract class AExtendedScriptableObject : ScriptableObject
    {
        protected void LogInformation(string message, Object obj = null) => Debug.Log($"[Information] [{GetType().Name}]\n{message}", obj);
        protected void LogWarning(string message, Object obj = null) => Debug.LogWarning($"[Warning] [{GetType().Name}]\n{message}", obj);
        protected void LogError(string message, Object obj = null) => Debug.LogError($"[Error] [{GetType().Name}]\n{message}", obj);

        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }

        public void MakeDirty()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
