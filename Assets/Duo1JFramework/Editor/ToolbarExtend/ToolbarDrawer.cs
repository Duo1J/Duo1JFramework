using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    [InitializeOnLoad]
    public class ToolbarDrawer
    {
        private const string BtnStyle = "AppCommand";

        private static void OnLeftToolbarGUI()
        {
            if (GUILayout.Button(EditorGUIUtility.TrTextContentWithIcon("", "", "Projector Icon"), BtnStyle))
            {
                //todo hlj 内敛样式和图标库
            }
        }

        private static void OnRightToolbarGUI()
        {

        }

        static ToolbarDrawer()
        {
            ToolbarExtender.LeftToolbarGUI = OnLeftToolbarGUI;
            ToolbarExtender.RightToolbarGUI = OnRightToolbarGUI;
        }
    }
}