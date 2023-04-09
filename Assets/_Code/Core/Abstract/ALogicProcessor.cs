using Game.Core.Interfaces;
using Game.Tags.Common;

namespace Game.Core.Abstract
{
    /// <summary>
    /// Abstract definition for logic processors
    /// </summary>
    public abstract class ALogicProcessor : ATag
    {
        public static ALogicProcessor Instance;

        public abstract void RegisterHighPriorityProcessor(ILogicUpdateProcessor processor);
        public abstract void RegisterLowPriorityProcessor(ILogicUpdateProcessor processor);
        public abstract void DeregisterHighPriorityProcessor(ILogicUpdateProcessor processor);
        public abstract void DeregisterLowPriorityProcessor(ILogicUpdateProcessor processor);
    }
}
