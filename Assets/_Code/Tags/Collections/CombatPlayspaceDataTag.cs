using Assets.Tags.Abstract;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.Collections
{
    [CreateAssetMenu(menuName = CollectionAssetMenuBaseName + "Combat Playspace")]
    public class CombatPlayspaceDataTag : ACollectionTag
    {
        [SerializeField, Required]
        private SceneReference combatScene = null;

        [SerializeField]
        private string DisplayName = "";
    }
}
