using Assets.Tags.Common;
using Game.Tags.Models;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets.Core.Abstract
{
    /// <summary>
    /// Abstract representation of the instance data associated with a player accessible static unit.
    /// Note: not marked as abstract so I can get it to render in the inspector.
    /// </summary>
    [System.Serializable]
    public class AStaticUnitInstance
    {
        [SerializeField, ReadOnly]
        public Guid Id = Guid.NewGuid();

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
