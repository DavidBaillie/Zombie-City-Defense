using System;

namespace Assets.Core.Interfaces
{
    public interface IDamageReceiver
    {
        /// <summary>
        /// Applies damage to the receiver
        /// </summary>
        /// <param name="damage">Damage to apply</param>
        /// <returns>If the damage killed the receiver</returns>
        bool ApplyDamage(float damage);

        /// <summary>
        /// Determines if the object is hostile to the given teamId
        /// </summary>
        /// <param name="teamid">TeamId to check against</param>
        /// <returns>If the team is hostile to this object</returns>
        bool IsHostile(int teamid);
    }
}
