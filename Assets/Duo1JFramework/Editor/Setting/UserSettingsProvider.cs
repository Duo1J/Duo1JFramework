using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// Preferences…Ë÷√
    /// </summary>
    public class UserSettingsProvider : SettingsProvider
    {
        public UserSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingProvider()
        {
            return new UserSettingsProvider($"Preferences/{Def.FRAME_WORK_NAME}", SettingsScope.User, null);
        }

        public override void OnGUI(string searchContext)
        {
        }
    }
}
