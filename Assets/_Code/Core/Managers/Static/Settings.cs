using Assets.Tags.Settings;

namespace Assets.Core.Managers.Static
{
    /// <summary>
    /// Static class used to store singleton references in the game project when loaded
    /// </summary>
    public static class GameSettings
    {
        public static InputSettingsTag InputSettings = null;
        public static EconomySettingsTag EconomySettings = null;
    }
}
