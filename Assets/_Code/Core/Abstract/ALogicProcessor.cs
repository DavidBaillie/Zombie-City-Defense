using Game.Core.Interfaces;
using Game.Utilities.BaseObjects;

namespace Game.Core.Abstract
{
    /// <summary>
    /// Abstract definition for logic processors
    /// </summary>
    public abstract class ALogicProcessor : AExtendedMonobehaviour
    {
        public static ALogicProcessor Instance;

        public abstract void RegisterHighPriorityProcessor(ILogicUpdateProcessor processor);
        public abstract void RegisterLowPriorityProcessor(ILogicUpdateProcessor processor);
        public abstract void DeregisterHighPriorityProcessor(ILogicUpdateProcessor processor);
        public abstract void DeregisterLowPriorityProcessor(ILogicUpdateProcessor processor);
    }
}
