using Assets.Tags.Abstract;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace GameEditor.Editor.GameManagers
{
    public class TagManager : OdinMenuEditorWindow
    {
        [MenuItem("Window/Game Managers/Tag Manager")]
        private static void OpenWindow()
        {
            GetWindow<TagManager>().Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = false;

            tree.AddAllAssetsAtPath("Tags", "Assets/Resources/Tags", typeof(ATag), true, false);
            return tree;
        }
    }
}
