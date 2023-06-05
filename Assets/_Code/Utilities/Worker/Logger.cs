namespace Assets.Utilities.Worker
{
    public static class Logger
    {
        public static void LogInformation(string infoBoxContent, string message, UnityEngine.Object referenceObject = null)
            => UnityEngine.Debug.Log($"[Information] [{infoBoxContent}]\n{message}", referenceObject);

        public static void LogWarning(string infoBoxContent, string message, UnityEngine.Object referenceObject = null)
            => UnityEngine.Debug.LogWarning($"[Warn] [{infoBoxContent}]\n{message}", referenceObject);

        public static void LogError(string infoBoxContent, string message, UnityEngine.Object referenceObject = null)
            => UnityEngine.Debug.LogError($"[Error] [{infoBoxContent}]\n{message}", referenceObject);
    }
}
