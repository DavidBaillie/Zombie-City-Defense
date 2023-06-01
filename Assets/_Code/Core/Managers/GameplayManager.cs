using Assets.Core.StaticChannels;
using Game.Utilities.BaseObjects;
using UnityEngine;

namespace Assets.Core.Managers
{
    /// <summary>
    /// Class manages the current state of the game as it's being played out.
    /// </summary>
    [SelectionBase]
    public class GameplayManager : AExtendedMonobehaviour
    {
        /// <summary>
        /// Called when the scene starts
        /// </summary>
        protected override void Start()
        {
            base.Start();

            GameplayInputChannel.EnableInput();
        }
    }
}
