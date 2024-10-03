using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// Preferences设置
    /// </summary>
    public class UserSettingsProvider : SettingsProvider
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
            EditorUserSettings.Draw();
        }

        #region Inner

        public UserSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingProvider()
        {
            return new UserSettingsProvider($"Preferences/{Def.FRAME_WORK_NAME}", SettingsScope.User, null);
        }

        #endregion Inner
    }
}
