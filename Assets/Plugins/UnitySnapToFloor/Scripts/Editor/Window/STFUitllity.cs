using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SnapToFloor
{
    public static class STFUitllity
    {
        /// <summary>
        /// 시스템 언어가 한국어이면 true, 아니면 false을 반환한다.
        /// </summary>
        /// <returns></returns>
        public static bool IsSystemLanguageKorean() => Application.systemLanguage == SystemLanguage.Korean;

        /// <summary>
        /// 3D모드이면 0, 2D이면 1을 반환합니다.
        /// </summary>
        /// <returns></returns>
        public static int GetEditorMode() => (int)EditorSettings.defaultBehaviorMode;

        public static STFSettings FindStfSettingsByEditor()
        {
            string[] stfSettingsGuid = AssetDatabase.FindAssets("t:STFSettings");

            if (stfSettingsGuid.Length == 0)
                return null;

            string path = AssetDatabase.GUIDToAssetPath(stfSettingsGuid[0]);

            STFSettings stfSettings = AssetDatabase.LoadAssetAtPath<STFSettings>(path);
            return stfSettings;
        }

        /// <summary>
        /// 디파인을 추가합니다.
        /// </summary>
        public static void AddDefine()
        {
            //현재 선택된 빌트 타겟에 처리합니다.
            List<string> defines = PlayerSettings
                .GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';')
                .ToList();

            STFSettings stfSettings = FindStfSettingsByEditor();

            //적용할 모드 가져오기
            int modeData = stfSettings.SnapMode == -1 ? GetEditorMode() : stfSettings.SnapMode;

            SnapToFloorSettings.KSnapMode modeIndex = (SnapToFloorSettings.KSnapMode)modeData;

            switch (modeIndex)
            {
                case SnapToFloorSettings.KSnapMode.Mode3D:
                    defines.Add("SNAP2FLOOR_3D");
                    defines.Remove("SNAP2FLOOR_2D");
                    break;
                case SnapToFloorSettings.KSnapMode.Mode2D:
                    defines.Add("SNAP2FLOOR_2D");
                    defines.Remove("SNAP2FLOOR_3D");
                    break;
            }

            //디파인 중복 제거
            defines = defines.Distinct().ToList();

            //문자열 다시 합친 후 심볼(디파인) 적용 
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join(";", defines.ToArray()));

            //결과를 표시합니다.
            EditorUtility.SetDirty(stfSettings);
        }

        /// <summary>
        /// 디파인을 제거합니다.
        /// </summary>
        public static void RemoveDefine()
        {
            STFSettings stfSettings = FindStfSettingsByEditor();

            //현재 선택된 빌트 타겟에 처리합니다.
            List<string> defines = PlayerSettings
                .GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';')
                .ToList();

            //디파인에 없었다면,
            if (!(defines.Contains("SNAP2FLOOR_2D") || defines.Contains("SNAP2FLOOR_3D")))
                return;

            //제거
            defines.Remove("SNAP2FLOOR_2D");
            defines.Remove("SNAP2FLOOR_3D");

            //디파인 중복 제거
            defines = defines.Distinct().ToList();

            //문자열 다시 합친후 심볼(디파인) 적용 
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join(";", defines.ToArray()));

            stfSettings.SnapMode = -1;
            stfSettings.StartAtShow = 0;

            EditorUtility.SetDirty(stfSettings);
        }

        public static bool HasDefine()
        {
#if SNAP2FLOOR_2D || SNAP2FLOOR_3D
            return true;
#else
            return false;
#endif
        }
    }
}