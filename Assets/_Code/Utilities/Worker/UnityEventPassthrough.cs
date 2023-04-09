using Game.Utilities.BaseObjects;
using System;
using UnityEngine;

namespace Game.Utilities.Worker
{
    /// <summary>
    /// Handles allowing other objects to access Unity's basic event cycle 
    /// </summary>
    public class UnityEventPassthrough : AExtendedMonobehaviour
    {
        private static UnityEventPassthrough _instance;
        public static UnityEventPassthrough Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("-- EVENT PASSTHROUGH --").AddComponent<UnityEventPassthrough>();
                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }

        public event Action OnUpdate;
        public event Action OnLateUpdate;
        public event Action OnFixedUpdate;

        public event Action OnDestroyed;
        public event Action OnDisabled;


        protected override void Update() { OnUpdate?.Invoke(); }
        protected override void LateUpdate() { OnLateUpdate?.Invoke(); }
        protected override void FixedUpdate() { OnFixedUpdate?.Invoke(); }
        
        protected override void OnDestroy() { OnDestroyed?.Invoke(); }
        protected override void OnDisable() { OnDisabled?.Invoke(); }

    }
}
