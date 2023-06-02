using Assets.Core.Abstract;
using Game.Tags.Models;

namespace Assets.Core.Models
{
    /// <summary>
    /// Instance data associated with a rifleman
    /// </summary>
    public class RiflemanUnitInstance : AStaticUnitInstance
    {
        public StaticUnitStatsTag RiflemanBaseData = null;
        public int Level = 1;

        public RiflemanUnitInstance(StaticUnitStatsTag riflemanBaseData, int level)
        {
            RiflemanBaseData = riflemanBaseData;
            Level = level;
        }
    }
}
