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

        public void Init(SkillDescType type)
        {
            var descPopup = UIView as UISkillDescPopup;

            for (int i = 0; i < descPopup.Descs.Length; i++)
            {
                descPopup.Descs[i].SetActive(i == (int)type);
            }


            var inputActions = new global::CharacterActions();
            inputActions.Player.Jump.performed += (x) =>
            {
                UIView.GetComponent<Animator>().SetTrigger("Release");
                ReleaseWithDelay(0.3f);
            };
        }
    }
}