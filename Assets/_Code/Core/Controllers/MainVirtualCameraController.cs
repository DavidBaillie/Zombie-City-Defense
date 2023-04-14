using Assets.Tags.Common;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Core.Controllers
{
    public class MainVirtualCameraController : AExtendedMonobehaviour
    {
        [SerializeField, Required]
        private ObjectTypeIdentifier id = null;

        ///Called when object created
        protected override void Start()
        {
            base.Start();

            if (id != null)
            {
                SceneObjectRegistry.RegisterObject(id, gameObject);
            }
        }
    }
}
