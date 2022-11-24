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
        public override void SetState(UIState state)
        {
        }

        public override void OnCreate(UIViewBase view)
        {
            base.OnCreate(view);
        }

        public async void Init(SkillDescType type, float waitTime = 5)
        {
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
            };

            AudioManager.Instance.SoundOneShot2D(AudioManager.Instance.SFX_UI_Helper);
        }
    }
}