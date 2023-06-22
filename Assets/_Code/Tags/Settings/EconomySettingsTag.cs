using Assets.Core.Managers.Static;
using Assets.Tags.Abstract;
using UnityEngine;

namespace Assets.Tags.Settings
{
    [CreateAssetMenu(menuName = SettingsAssetMenuBaseName + "Economy", fileName = "Economy Settings")]
    public class EconomySettingsTag : ASettingsTag
    {
        [SerializeField]
        public Vector2Int GatherResourceRange = new Vector2Int(1, 2);
    }
}
