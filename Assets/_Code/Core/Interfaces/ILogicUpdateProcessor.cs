namespace Assets.Core.Interfaces
{
    public interface ILogicUpdateProcessor
    {
        void ProcessLogic();
        void OnGameModeFailure();
    }
}
