using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

namespace SuraSang
{
    public enum SkillDescType
    {
        Happy,
        Sad,
        Anger
    }

    public class UISkillDescPopup : UIPopupBase
    {
        [SerializeField] private GameObject[] _descs;
        public GameObject[] Descs => _descs;
    }

    public class UISkillDescPopupModel : UIModelBase
    {
        public override UIType UIType => UIType.Popup;
        public override string PrefabPath => "SkillDescPopup";

        private bool _showSkillDesc;

        public override void SetState(UIState state)
        {
        }

        public override void OnCreate(UIViewBase view)
        {
            base.OnCreate(view);
        }

        public override void ReleaseUI()
        {
            base.ReleaseUI();

            Global.Instance.SceneMaster.Player.CanMove = true;

            if (_showSkillDesc)
            {
                Global.Instance.UIManager.ReleaseAllPopups();
                Global.Instance.UIManager.Get<UIDescPopupModel>().Init(DescType.Skill, 5);
            }
        }

        public async void Init(SkillDescType type, float waitTime = 5, bool ShowDescPopup = false)
        {
            _showSkillDesc = ShowDescPopup;

            UIView.gameObject.SetActive(false);

            await Task.Delay((int)(1000 * waitTime));

            UIView.gameObject.SetActive(true);

            var descPopup = UIView as UISkillDescPopup;

            for (int i = 0; i < descPopup.Descs.Length; i++)
            {
                descPopup.Descs[i].SetActive(i == (int)type);
            }

            var inputActions = new global::CharacterActions();
            inputActions.Enable();
            inputActions.Player.Jump.performed += (x) =>
            {
                UIView.GetComponent<Animator>().SetTrigger("Release");
                ReleaseWithDelay(1);
                inputActions.Dispose();
                Global.Instance.SceneMaster.Player.CanMove = false;
            };

            AudioManager.Instance.SoundOneShot2D(AudioManager.Instance.SFX_UI_Helper);

        }
    }
}