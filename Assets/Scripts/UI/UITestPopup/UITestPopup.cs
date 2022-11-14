using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace SuraSang
{
    public class UITestPopup : UIPopupBase
    {
        [SerializeField] private TextMeshProUGUI _testText;
        public TextMeshProUGUI TestText => _testText;


        [HideInInspector] public UnityAction OnClickExit;

        public void OnClick_ExitButton()
        {
            OnClickExit?.Invoke();
        }
    }

    public class UITestPopupModel : UIModelBase
    {
        public override UIType UIType => UIType.Popup;
        public override string PrefabPath => "TestPopup";
        public override void SetState(UIState state)
        {
        }

        public override void OnCreate(UIViewBase view)
        {
            base.OnCreate(view);

            var testPopup = view as UITestPopup;
            testPopup.OnClickExit += ReleaseUI;
            testPopup.TestText.text = Time.time.ToString();
        }
    }
}