using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

namespace SuraSang
{
    public partial class Player
    {
        public Emotion CurrentEmotion { get; private set; }
        
        //변신 모델
        private GameObject _currentCharacter;
        [SerializeField] private GameObject _characterDefault;
        [SerializeField] private GameObject _characterAnger;
        [SerializeField] private GameObject _characterHappy;
        [SerializeField] private GameObject _characterSad;

        private List<Collider> _hitTargetContainer = new List<Collider>();//인식한 물체 보관하는 리스트

        private bool _isFind;

        private void InitAbsorb()
        {
        }

        private void UpdateAbsorb()
        {
        }

        public void SetAbsorbAction()
        {
            SetAction(ButtonActions.Absorb, OnAbsorb);
        }
        
        private void FindViewTargets()
        {
            _hitTargetContainer.Clear();

            var originPos = transform.position;

            Collider[] hitedTargets = Physics.OverlapSphere(originPos, PlayerData.AbsorbRange, PlayerData.EnemyCheckLayer);

            foreach (Collider target in hitedTargets)
            {
                var dist = target.transform.position - transform.position;
                var dot = Vector3.Dot(dist.normalized, transform.forward);
                var angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

                if (angle <= PlayerData.AbsorbAngle / 2f)
                {
                    _isFind = true;
                    _hitTargetContainer.Add(target);
                }
                else
                {
                    _isFind = false;
                    continue;
                }
            }
        }

        private void OnAbsorb(bool isOn)
        {
            FindViewTargets();

            if (isOn && _hitTargetContainer.Count != 0)
            {
                for (int i = 0; i < _hitTargetContainer.Count; i++)
                {
                    if (!_hitTargetContainer[i].gameObject.GetComponent<Monster>().IsSleep)
                    {
                        CurrentEmotion = _hitTargetContainer[i].gameObject.GetComponent<Monster>().Emotion;

                        GameObject obj;
                        switch (CurrentEmotion)
                        {
                            case Emotion.Anger:
                                obj = Global.Instance.ResourceManager.GetObject(Constant.AngerAbsorbEffectPath, transform);
                                Global.Instance.ResourceManager.ReturnParticleSystem(Constant.AngerAbsorbEffectPath, obj);
                                //gameObject.GetComponent<Renderer>().material.color = Color.red;// 임시 색상 변경
                                break;

                            case Emotion.Happiness:
                                obj = Global.Instance.ResourceManager.GetObject(Constant.HappyAbsorbEffectPath, transform);
                                Global.Instance.ResourceManager.ReturnParticleSystem(Constant.HappyAbsorbEffectPath, obj);
                                //gameObject.GetComponent<Renderer>().material.color = Color.yellow;// 임시 색상 변경
                                break;

                            case Emotion.Sadness:
                                obj = Global.Instance.ResourceManager.GetObject(Constant.SadAbsorbEffectPath, transform);
                                Global.Instance.ResourceManager.ReturnParticleSystem(Constant.SadAbsorbEffectPath, obj);
                                //gameObject.GetComponent<Renderer>().material.color = Color.blue;// 임시 색상 변경
                                break;
                        }

                        //임시 흡수 애니메이션 재생
                        Animator.SetTrigger("IsChange");
                        _hitTargetContainer[i].gameObject.GetComponent<Monster>().Absorbed();
                    }
                }
            }
        }

        public void ReturnEmotion()
        {
            CurrentEmotion = Emotion.Default;
        }
    }
}
