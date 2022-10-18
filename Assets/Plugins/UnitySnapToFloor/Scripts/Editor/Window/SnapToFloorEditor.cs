using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SnapToFloor
{
    public class SnapToFloorEditor : EditorWindow
    {
        //uxml절대 경로
        [SerializeField] private VisualTreeAsset STFUXML;

        #region ObjectField

        private DropdownField _modeDropDown;
        private DropdownField _startAtShowDropDown;

        #endregion

        #region Button

        private Button _applyButton;

        #endregion

        #region Label

        private Label _howToUseLabel;
        private Label _descriptionLabel;

        #endregion

        [MenuItem("Window/SnapToFloor/SnapToFloor Installer")]
        public static void Title()
        {
            SnapToFloorEditor wnd = GetWindow<SnapToFloorEditor>();
            wnd.titleContent = new GUIContent("SnapToFloor Installer");
            wnd.minSize = new Vector2(280, 300);
            wnd.maxSize = new Vector2(400, 360);
        }

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.delayCall += Popup;
        }

        private static void Popup()
        {
            EditorApplication.delayCall -= Popup;
            
            if (Application.isPlaying)
                return;
                
            STFSettings stfSettings = STFUitllity.FindStfSettingsByEditor();
            
            //Always이면 true, 아니면 false
            int showOnStartup = stfSettings.StartAtShow;

            //항상으로 표시되어 있다면, 켜기
            if (showOnStartup == 0)
                Title();
        }
        
        public void CreateGUI()
        {
            #region 기본 설정

            // 각 편집기 창에는 루트 VisualElement 개체가 포함되어 있습니다.
            VisualElement root = rootVisualElement;

            //UXML 가져오기
            VisualTreeAsset visualTree = STFUXML;
            VisualElement container = visualTree.Instantiate();
            root.Add(container);

            #endregion

            #region DropdownField

            _modeDropDown = root.Q<DropdownField>("mode-DropDown");
            _startAtShowDropDown = root.Q<DropdownField>("startAtShow-DropDown");

            #endregion

            #region Button

            _applyButton = root.Q<Button>("apply-Button");

            #endregion

            #region Label

            _howToUseLabel = root.Q<Label>("howToUse-Text");
            _descriptionLabel = root.Q<Label>("description-Text");

            #endregion

            //초기화
            InitSetUp();

            //언어 설정
            InitLanguage();

            void InitSetUp()
            {
                #region Default

                //언어 설정하기
                SnapToFloorSettings.KLanguage language = STFUitllity.IsSystemLanguageKorean()
                    ? SnapToFloorSettings.KLanguage.한국어
                    : SnapToFloorSettings.KLanguage.English;

                //모드-드롭다운에 내용을 추가합니다.
                _modeDropDown.choices = Enum.GetNames(typeof(SnapToFloorSettings.KSnapMode)).ToList();

                //항상 켜기/끄기 설정
                _startAtShowDropDown.choices = SnapToFloorSettings.GetLanguageScript(language);

                STFSettings stfSettings = STFUitllity.FindStfSettingsByEditor();

                int startAtShowData = stfSettings.StartAtShow;
                int modeData = stfSettings.SnapMode == -1
                    ? STFUitllity.GetEditorMode()
                    : stfSettings.SnapMode;

                #endregion

                //모드 설정
                _modeDropDown.index = modeData;

                //쇼 드롭다운 인덱스 처리
                _startAtShowDropDown.index = startAtShowData;

                //이미 적용되어 있으면 비활성화합니다.
                _applyButton.SetEnabled(!STFUitllity.HasDefine());

                if (STFUitllity.HasDefine())
                {
                    //적용 메세지 표시
                    Debug.Log(STFUitllity.IsSystemLanguageKorean()
                        ? "SnapToFloor기능이 활성화 되었습니다."
                        : "The SnapToFloor function has been activated.");    
                }
            }

            void InitLanguage()
            {
                //언어 설정하기
                SnapToFloorSettings.KLanguage language = STFUitllity.IsSystemLanguageKorean()
                    ? SnapToFloorSettings.KLanguage.한국어
                    : SnapToFloorSettings.KLanguage.English;

                if (language == SnapToFloorSettings.KLanguage.한국어)
                    SetKorean();
                else
                    SetEnglish();

                void SetKorean()
                {
                    _applyButton.text = "적용";
                    _modeDropDown.label = "사용하는 모드 ?";
                    _howToUseLabel.text = "사용법 ?";
                    _descriptionLabel.text = "자신이 사용하는 유니티 모드가 2D/3D에 맞춰 선택하고 적용을 누르세요.";
                    _startAtShowDropDown.label = "시작 시 표시";
                    _modeDropDown.tooltip = "프로젝트에 맞춰서 모드를 선택합니다." +
                                            "2.5D를 만드는 경우, 2D프로젝트에서 3D를 선택하면 캐릭터는 스프라이트 렌더러를 사용하고 지형은 3D오브젝트를 사용할 수 있습니다.";
                    _startAtShowDropDown.tooltip = "설정을 완료하면 이 옵션을 '끄기'로 변경하여 컴파일 이후 설정창이 생성되는 것을 끌 수 있습니다.";
                }

                void SetEnglish()
                {
                    _applyButton.text = "Apply";
                    _modeDropDown.label = "Use Mode ?";
                    _howToUseLabel.text = "How to use ?";
                    _descriptionLabel.text = "Check whether the Unity mode is 2D or 3D.";
                    _startAtShowDropDown.label = "Show at StartUp";

                    _modeDropDown.tooltip = "Choose a mode according to your project." +
                                            "If you're making 2.5D, if you choose 3D in a 2D project, the character will use the sprite renderer and the terrain will use the 3D object.";
                    _startAtShowDropDown.tooltip =
                        "After setting, change this option to 'Never' so that it will not be created after compilation.";
                }
            }

            #region CallBack

            _modeDropDown.RegisterValueChangedCallback(_ => ChangeMode());
            _startAtShowDropDown.RegisterValueChangedCallback(_ => ChangeStartAtShow());
            _applyButton.clicked += Apply;

            #endregion
        }

        private static void Apply()
        {
            if (STFUitllity.HasDefine())
            {
                Debug.LogWarning(STFUitllity.IsSystemLanguageKorean()
                    ? "SnapToFloor기능이 이미 활성화 되어 있습니다."
                    : "SnapToFloor is already enabled.");
            }
            else
                STFUitllity.AddDefine();
        }

        /// <summary>
        /// 값을 적용
        /// </summary>
        private void ChangeStartAtShow()
        {
            STFSettings stfSettings = STFUitllity.FindStfSettingsByEditor();
            stfSettings.StartAtShow = _startAtShowDropDown.index;
            EditorUtility.SetDirty(stfSettings);
        }

        /// <summary>
        /// 모드를 변경합니다.
        /// </summary>
        private void ChangeMode()
        {
            STFSettings stfSettings = STFUitllity.FindStfSettingsByEditor();
            stfSettings.SnapMode = _modeDropDown.index;
            EditorUtility.SetDirty(stfSettings);
        }

#if SNAP2FLOOR_3D || SNAP2FLOOR_2D
        [MenuItem("Window/SnapToFloor/Disable")]
        private static void CleanDefine()
        {
            if (!STFUitllity.HasDefine())
            {
                Debug.LogWarning(STFUitllity.IsSystemLanguageKorean()
                    ? "SnapToFloor기능이 활성화 되어 있지 않습니다."
                    : "SnapToFloor function is not activated.");
            }
            else
            {
                string title;
                string message;
                string ok;
                string cancel;

                if (STFUitllity.IsSystemLanguageKorean())
                {
                    title = "삭제 마법사";
                    message = "Snap To Floor 기능을 해제하겠습니까?";
                    ok = "네";
                    cancel = "아니오";
                }
                else
                {
                    title = "UnInstall";
                    message = "Are you sure you want to turn off the Snap To Floor feature?";
                    ok = "Yes";
                    cancel = "No";
                }

                bool isInstall = EditorUtility.DisplayDialog(title, message, ok, cancel);

                if (isInstall)
                    STFUitllity.RemoveDefine();
            }
        }
#endif
        
    }
}