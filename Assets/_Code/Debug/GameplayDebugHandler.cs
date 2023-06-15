using Assets.Utilities.Worker;
using Game.Tags.Settings;
using System;

namespace Assets.Debug
{
    public static class GameplayDebugHandler
    {
        /// <summary>
        /// Handles enabling the rendering of debug content in the editor.
        /// Function is disabled at runtime.
        /// </summary>
        /// <param name="debugEvent">Code to run</param>
        /// <param name="requireLines">If this call requires line debug events</param>
        /// <param name="requireText">If this call requires text debug events</param>
        public static void HandleRenderCall(Action debugEvent, bool requireLines = false, bool requireText = false)
        {
#if DEBUG
            //Only process valid requests
            if ((requireLines && !GlobalSettingsTag.DevInstance.DrawDebugLines) || (requireText && !GlobalSettingsTag.DevInstance.DrawDebugText))
                return;

            try { debugEvent.Invoke(); }
            catch (Exception e)
            { Logger.LogError(nameof(GameplayDebugHandler), $"Failed to correctly invoke a request: {e}"); }
#endif
        }

        /// <summary>
        /// Handles enabling the rendering of debug content in the editor.
        /// Function is disabled at runtime.
        /// </summary>
        /// <param name="debugEvent">Code to run</param>
        /// <param name="withDuration">Specifies an ALINE WithDuration to wrap the render call</param>
        /// <param name="requireLines">If this call requires line debug events</param>
        /// <param name="requireText">If this call requires text debug events</param>
        public static void HandleRenderCall(Action debugEvent, float withDuration, bool requireLines = false, bool requireText = false)
        {
#if DEBUG
            //Only process valid requests
            if ((requireLines && !GlobalSettingsTag.DevInstance.DrawDebugLines) || (requireText && !GlobalSettingsTag.DevInstance.DrawDebugText))
                return;

            try 
            {
                using (Drawing.Draw.WithDuration(withDuration))
                {
                    debugEvent.Invoke();
                } 
            }
            catch (Exception e)
            { 
                Logger.LogError(nameof(GameplayDebugHandler), $"Failed to correctly invoke a request: {e}"); 
            }
#endif
        }
    }
}
