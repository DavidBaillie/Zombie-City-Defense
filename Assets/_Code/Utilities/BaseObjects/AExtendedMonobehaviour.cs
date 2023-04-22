namespace Game.Utilities.BaseObjects
{
    public abstract class AExtendedMonobehaviour : Drawing.MonoBehaviourGizmos
    {
        protected void LogInformation(string message, UnityEngine.Object obj = null) => Assets.Utilities.Worker.Logger.LogInformation(GetType().Name, message, obj);
        protected void LogWarning(string message, UnityEngine.Object obj = null) => Assets.Utilities.Worker.Logger.LogWarning(GetType().Name, message, obj);
        protected void LogError(string message, UnityEngine.Object obj = null) => Assets.Utilities.Worker.Logger.LogError(GetType().Name, message, obj);

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
