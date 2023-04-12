using Game.Utilities.BaseObjects;

namespace Assets.Tags.Abstract
{
    public abstract class ATag : AExtendedScriptableObject
    {
        protected const string AssetMenuBaseName = "Game/Tags/";

        /// <summary>
        /// Called once when the game starts to initialize this tag
        /// </summary>
        public virtual void InitializeTag() { }
    }
}
