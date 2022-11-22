using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Threading.Tasks;

namespace SuraSang
{
    public class UISavingPopup : UIPopupBase
    {
    }

    public class UISavingPopupModel : UIModelBase
    {
        private const float ReleaseTime = 1.5f;

        public override UIType UIType => UIType.Popup;
        public override string PrefabPath => "SavingPopup";
        public override void SetState(UIState state)
        {
        }

        public override void OnCreate(UIViewBase view)
        {
            base.OnCreate(view);
        }

        public async void Init()
        {
            await Task.Delay((int)(ReleaseTime * 1000));

            ReleaseUI();
        }
    }
}