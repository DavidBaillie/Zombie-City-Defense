using Assets.Core.Managers.Static;
using Assets.Tags.Abstract;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.Settings
{
    /// <summary>
    /// Settings tag to be used statically by the game to provide decisions on how to process inputs from the player
    /// </summary>
    [CreateAssetMenu(menuName = SettingsAssetMenuBaseName + "Input Settings", fileName = "Input Settings Tag")]
    public class InputSettingsTag : ASettingsTag
    {
        [PropertyTooltip("World units from the origin of a tap that will count towards selecting a world coordinate")]
        [SerializeField, MinValue(0), SuffixLabel("m", true)]
        public float TapSelectionRange = 1f;

        public override void InitializeTag()
        {
            base.InitializeTag();

            if (GameSettings.InputSettings != null)
            {
                LogError($"Could not assign settings to static class, a reference already exists!");
                return;
            }

            GameSettings.InputSettings = this;
        }
    }
}
