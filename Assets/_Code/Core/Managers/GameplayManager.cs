using Assets.Tags.Channels;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Core.Managers
{
    /// <summary>
    /// Class manages the current state of the game as it's being played out.
    /// </summary>
    [SelectionBase]
    public class GameplayManager : AExtendedMonobehaviour
    {
        [SerializeField, Required]
        private PlayerInputChannel inputChannel = null;

        /// <summary>
        /// Called when the scene starts
        /// </summary>
        protected override void Start()
        {
            base.Start();

            inputChannel.EnableInput();
        }
    }
}
