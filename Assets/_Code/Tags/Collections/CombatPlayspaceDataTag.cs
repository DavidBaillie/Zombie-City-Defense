using Assets.Tags.Abstract;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.Collections
{
    [CreateAssetMenu(menuName = CollectionAssetMenuBaseName + "Combat Playspace")]
    public class CombatPlayspaceDataTag : ACollectionTag
    {
        [SerializeField, Required]
        public SceneReference combatScene = null;

        [SerializeField]
        public string DisplayName = "";
    }
}
