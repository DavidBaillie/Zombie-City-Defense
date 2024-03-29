﻿using Game.Editor.Tools;
using Game.Tags.Settings;
using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

namespace Assets.Editor.Tools
{
    public static class ToolbarStyles
    {
        public static GUIStyle BranchLabelStyle;
        public static GUIStyle CommandButtonStyle;
        public static GUILayoutOption CommandButtonSpacing => GUILayout.Width(70);


        static ToolbarStyles()
        {
            BranchLabelStyle = new()
            {
                fontSize = 18,
                fontStyle = FontStyle.Bold,
            };

            CommandButtonStyle = new GUIStyle("Command")
            {
                fontSize = 10,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold,
                fixedWidth = 65,
                stretchWidth = true
            };
        }
    }


    [InitializeOnLoad]
    public class EditorToolbar
    {
        /// <summary>
        /// Constructor
        /// </summary>
        static EditorToolbar()
        {
            ToolbarExtender.RightToolbarGUI.Add(BuildRightToolbar);
        }

        /// <summary>
        /// Builds the toolbar on the right side of the play button
        /// </summary>
        private static void BuildRightToolbar() 
        {
            bool appSetting = EditorPreloader.ShouldPreload;

            GUILayout.Space(25);

            if (appSetting != GUILayout.Toggle(appSetting, new GUIContent("Preload", "Toggle Preloader"),
                ToolbarStyles.CommandButtonStyle, ToolbarStyles.CommandButtonSpacing))
            {
                EditorPreloader.ShouldPreload = !appSetting;
            }

            bool enableDebugLines = GlobalSettingsTag.DevInstance.DrawDebugLines;
            if (enableDebugLines != GUILayout.Toggle(enableDebugLines, new GUIContent("Debug Lines", "Toggle Debug View"),
                ToolbarStyles.CommandButtonStyle, ToolbarStyles.CommandButtonSpacing))
            {
                GlobalSettingsTag.DevInstance.DrawDebugLines = !enableDebugLines;
                EditorUtility.SetDirty(GlobalSettingsTag.DevInstance);
            }

            bool enableDebugText = GlobalSettingsTag.DevInstance.DrawDebugText;
            if (enableDebugText != GUILayout.Toggle(enableDebugText, new GUIContent("Debug Text", "Toggle Debug View"),
                ToolbarStyles.CommandButtonStyle, ToolbarStyles.CommandButtonSpacing))
            {
                GlobalSettingsTag.DevInstance.DrawDebugText = !enableDebugText;
                EditorUtility.SetDirty(GlobalSettingsTag.DevInstance);
            }
        }
    }
}
