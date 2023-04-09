using System;

namespace Game.Core.Interfaces
{
    public interface IDamageReceiver
    {
        public void ApplyDamage(float damage);
        public bool IsHostile(int teamid);
    }
}
