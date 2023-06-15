using Assets.Tags.Common;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Core.Controllers
{
    public class MainVirtualCameraController : AExtendedMonobehaviour
    {
        [SerializeField, Required]
        private CameraTypeIdentifier cameraType = null;

        ///Called when object created
        protected override void Start()
        {
            base.Start();

            if (cameraType != null)
            {
                SceneObjectRegistry.RegisterObject(cameraType, gameObject);
            }
        }
    }
}
