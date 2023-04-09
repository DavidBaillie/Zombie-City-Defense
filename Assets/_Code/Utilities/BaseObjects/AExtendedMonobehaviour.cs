using Drawing;
using System;
using UnityEngine;

namespace Game.Utilities.BaseObjects
{
    public abstract class AExtendedMonobehaviour : MonoBehaviourGizmos
    {
        protected void LogInformation(string message, UnityEngine.Object obj = null) => Debug.Log($"[Information] [{GetType().Name}]\n{message}", obj);
        protected void LogWarning(string message, UnityEngine.Object obj = null) => Debug.LogWarning($"[Warning] [{GetType().Name}]\n{message}", obj);
        protected void LogError(string message, UnityEngine.Object obj = null) => Debug.LogError($"[Error] [{GetType().Name}]\n{message}", obj);

        protected virtual void OnEnable() { }
        protected virtual void Awake() { }
        protected virtual void Start() { }

        protected virtual void Update() { }
        protected virtual void LateUpdate() { } 
        protected virtual void FixedUpdate() { }    

        protected virtual void OnDestroy() { }
        protected virtual void OnDisable() { }
    }
}
