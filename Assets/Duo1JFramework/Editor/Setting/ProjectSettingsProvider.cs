using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// Project…Ë÷√
    /// </summary>
    public class ProjectSettingsProvider : SettingsProvider
    {
        public ProjectSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingProvider()
        {
            return new ProjectSettingsProvider(Def.FRAME_WORK_NAME, SettingsScope.Project, null);
        }

        public override void OnGUI(string searchContext)
        {
        }
    }
}
