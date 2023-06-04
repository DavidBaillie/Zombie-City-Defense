using Assets.Tags.Common;
using Game.Tags.Models;
using UnityEngine;

namespace Assets.Core.Abstract
{
    /// <summary>
    /// Abstract representation of the instance data associated with a player accessible static unit
    /// </summary>
    [System.Serializable]
    public abstract class AStaticUnitInstance
    {
        [SerializeField]
        public string DisplayName = "";

        [SerializeField]
        public int Level = 1;

        [SerializeField]
        public StaticUnitStatsTag unitStats = null;

        [SerializeField]
        public StaticEntityIdentifier unitType = null;
    }
}
