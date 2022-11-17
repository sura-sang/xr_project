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

            if (isOn && _hitTargetContainer.Count != 0 && Controller.isGrounded)
            {
                CanMove = false;

                if (_hitTargetContainer.Count != 1)
                {
                    var temp = 0f;
                    var temp2 = 0f;
                    var index = 0;
                    var index2 = 0;

                    for (int i = 0; i < _hitTargetContainer.Count; i++)
                    {
                        if(i == 0)
                        {
                            temp = Vector3.Distance(transform.position, _hitTargetContainer[i].transform.position);
                            index = i;
                            continue;
                        }
                        else
                        {
                            temp2 = Vector3.Distance(transform.position, _hitTargetContainer[i].transform.position);
                            index2 = i;
                        }

                        if(temp>temp2)
                        {
                            temp = temp2;
                            index = index2;
                        }
                    }

                    if (!_hitTargetContainer[index].gameObject.GetComponent<Monster>().IsSleep && Controller.isGrounded)
                    {
                        CurrentEmotion = _hitTargetContainer[index].gameObject.GetComponent<Monster>().Emotion;

                        GameObject obj;
                        switch (CurrentEmotion)
                        {
                            case Emotion.Anger:
                                obj = Global.Instance.ResourceManager.GetObject(Constant.AngerAbsorbEffectPath, transform);
                                Global.Instance.ResourceManager.ReturnParticleSystem(Constant.AngerAbsorbEffectPath, obj);
                                obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
                                break;

                            case Emotion.Happiness:
                                obj = Global.Instance.ResourceManager.GetObject(Constant.HappyAbsorbEffectPath, transform);
                                Global.Instance.ResourceManager.ReturnParticleSystem(Constant.HappyAbsorbEffectPath, obj);
                                obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
                                break;

                            case Emotion.Sadness:
                                obj = Global.Instance.ResourceManager.GetObject(Constant.SadAbsorbEffectPath, transform);
                                Global.Instance.ResourceManager.ReturnParticleSystem(Constant.SadAbsorbEffectPath, obj);
                                obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
                                break;
                        }

                        //Animator.SetTrigger("Change");
                        // TO DO : 트리거 대신 Bool로 파라미터 지정해주니까 문제 해결됨. 혹시 몰라서 애니메이터는 안만질게요.
                        Animator.SetBool("Change", true);
                        _hitTargetContainer[index].gameObject.GetComponent<Monster>().Absorbed();
                        _hitTargetContainer[index].gameObject.GetComponent<Animator>().SetTrigger("Absorbed");
                    }
                }
                else
                {
                     if (!_hitTargetContainer[0].gameObject.GetComponent<Monster>().IsSleep && Controller.isGrounded)
                    {
                        CurrentEmotion = _hitTargetContainer[0].gameObject.GetComponent<Monster>().Emotion;

                        GameObject obj;
                        switch (CurrentEmotion)
                        {
                            case Emotion.Anger:
                                obj = Global.Instance.ResourceManager.GetObject(Constant.AngerAbsorbEffectPath, transform);
                                Global.Instance.ResourceManager.ReturnParticleSystem(Constant.AngerAbsorbEffectPath, obj);
                                obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
                                break;

                            case Emotion.Happiness:
                                obj = Global.Instance.ResourceManager.GetObject(Constant.HappyAbsorbEffectPath, transform);
                                Global.Instance.ResourceManager.ReturnParticleSystem(Constant.HappyAbsorbEffectPath, obj);
                                obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
                                break;

                            case Emotion.Sadness:
                                obj = Global.Instance.ResourceManager.GetObject(Constant.SadAbsorbEffectPath, transform);
                                Global.Instance.ResourceManager.ReturnParticleSystem(Constant.SadAbsorbEffectPath, obj);
                                obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
                                break;
                        }
                        //Animator.SetTrigger("Change");
                        Animator.SetBool("Change", true);
                        _hitTargetContainer[0].gameObject.GetComponent<Monster>().Absorbed();
                        _hitTargetContainer[0].gameObject.GetComponent<Animator>().SetTrigger("Absorbed");
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
