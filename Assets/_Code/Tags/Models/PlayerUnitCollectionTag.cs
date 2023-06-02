using Assets.Core.Abstract;
using Assets.Tags.Abstract;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tags.Models
{
    /// <summary>
    /// Class represents the collection of user owned units they have access to during gameplay
    /// </summary>
    [CreateAssetMenu(menuName = CollectionTagAssetMenu + "Player Units", fileName = "Player Units")]
    public class PlayerUnitCollectionTag : ACollectionTag
    {
        [SerializeField]
        public List<AStaticUnitInstance> availableUnits = new List<AStaticUnitInstance>();


    }
}
