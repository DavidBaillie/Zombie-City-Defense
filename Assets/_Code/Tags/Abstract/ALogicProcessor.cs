using Assets.Core.Interfaces;

namespace Assets.Tags.Abstract
{
    /// <summary>
    /// Abstract definition for logic processors
    /// </summary>
    public abstract class ALogicProcessor : ATag
    {
        public abstract void RegisterHighPriorityProcessor(ILogicUpdateProcessor processor);
        public abstract void RegisterLowPriorityProcessor(ILogicUpdateProcessor processor);
        public abstract void DeregisterHighPriorityProcessor(ILogicUpdateProcessor processor);
        public abstract void DeregisterLowPriorityProcessor(ILogicUpdateProcessor processor);
        public abstract void BroadcastGameModeFailState();
    }
}
