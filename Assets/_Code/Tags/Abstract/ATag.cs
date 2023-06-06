using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using System;

namespace Assets.Tags.Abstract
{
    public abstract class ATag : AExtendedScriptableObject
    {
        [ShowInInspector, ReadOnly]
        public Guid Id = Guid.NewGuid();

        protected const string AssetMenuBaseName = "Game/Tags/";

        /// <summary>
        /// Called once when the game starts to initialize this tag
        /// </summary>
        public virtual void InitializeTag() { }

        /// <summary>
        /// Called when the tag should stop functioning and cleanup resources
        /// </summary>
        public virtual void CleanupTag() { }
    }
}
