using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// Project设置
    /// </summary>
    public class ProjectSettingsProvider : SettingsProvider
    {
        private Vector2 scollPos;

        public override void OnGUI(string searchContext)
        {
            ES.SetRichText(true);

            ED.Scroll(ref scollPos, () =>
            {
                DrawEditor();
            });
        }

        private void DrawEditor()
        {
            GUILayout.Label("<size=16>Editor Setting</size>");
            EditorProjectSettings.Draw();
        }

        #region Inner

        public ProjectSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingProvider()
        {
            return new ProjectSettingsProvider($"Project/{Def.FRAME_WORK_NAME}", SettingsScope.Project, null);
        }

        #endregion Inner
    }
}
