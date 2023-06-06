using Assets.Tags.Abstract;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Core.Controllers
{
    /// <summary>
    /// Combat scene initializer that provides the local grid data for instancing
    /// </summary>
    public class CombatGameModeInitializer : GameModeSceneInitializer
    {
        [SerializeField, Required]
        private AGridDataProvider sceneGridData = null;

        protected override void Awake()
        {
            base.Awake();
            AGridDataProvider.SetActiveDataProvider(sceneGridData);
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            AGridDataProvider.ClearActiveDataProvider(sceneGridData);
        }
    }
}
