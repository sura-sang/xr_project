using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

namespace SuraSang
{
    public enum DescType
    {
        WASD,
        Shift,
        Space,
        Hold,
        Absorb,
        Skill
    }

    public class UIDescPopup : UIPopupBase
    {
        [SerializeField] private GameObject[] _icons;
        public GameObject[] Icons => _icons;
    }

    public class UIDescPopupModel : UIModelBase
    {
        public override UIType UIType => UIType.Popup;
        public override string PrefabPath => "DescPopup";
        public override void SetState(UIState state)
        {
        }

        public override void OnCreate(UIViewBase view)
        {
            base.OnCreate(view);
        }

        public async void Init(DescType type, float releaseTime = -1)
        {
            var descPopup = UIView as UIDescPopup;

            for (int i = 0; i < descPopup.Icons.Length; i++)
            {
                descPopup.Icons[i].SetActive(i == (int)type);
            }

            if (releaseTime != -1)
            {
                await Task.Delay((int)(releaseTime * 1000));

                ReleaseUI();
            }
        }
    }
}