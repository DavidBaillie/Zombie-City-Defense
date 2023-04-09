using Game.Utilities.BaseObjects;

namespace Game.Core.Abstract
{
    public abstract class AEntity : AExtendedMonobehaviour
    {
        public int TeamId = 0;

        /// <summary>
        /// Determines if this entity believe the provided team id is hostile to it
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public virtual bool IsHostile(int teamId)
        {
            //Team 0 is neutral
            if (TeamId == 0 || teamId == 0) return false;

            //Otherwise match team values
            return TeamId != teamId;
        }
    }
}
