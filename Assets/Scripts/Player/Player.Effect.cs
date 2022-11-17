using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public partial class Player
    {
        private GameObject _angerIdleEffect;
        private GameObject _happyIdleEffect;

        private GameObject _angerWalkEffect;
        private GameObject _happyWalkEffect;

        private GameObject _angerRunEffect;
        private GameObject _happyRunEffect;

        private bool _isMakeEffect = false;

        private GameObject _Skillobj;
        private GameObject _Animobj;

        private void ChangeEffect()
        {
            if (IsSkill)
            {
                if (CurrentEmotion == Emotion.Happiness && _Skillobj == null)
                {
                    _Skillobj = Global.Instance.ResourceManager.GetObject(Constant.HappySkillEffectPath, transform);
                    _Skillobj.transform.localPosition = new Vector3(0, -1, 0);
                }
                else if (CurrentEmotion == Emotion.Anger && _Skillobj == null)
                {
                    _Skillobj = Global.Instance.ResourceManager.GetObject(Constant.AngerSkillRushEffectPath, transform);
                    _Skillobj.transform.localPosition = new Vector3(0, -0.4f, -0.2f);
                    _Skillobj.transform.localRotation = Quaternion.Euler(0, transform.rotation.y, 0);
                }
            }
            else if (CurrentEmotion == Emotion.Happiness && _Skillobj != null)
            {
                Global.Instance.ResourceManager.ReturnObject(Constant.HappySkillEffectPath, _Skillobj);
                _Skillobj = null;
            }
            else if (CurrentEmotion == Emotion.Anger && _Skillobj != null)
            {
                Global.Instance.ResourceManager.ReturnObject(Constant.AngerSkillRushEffectPath, _Skillobj);
                _Skillobj = null;
            }
        }

        public void ChangeStateEffect()
        {
            if (CurrentEmotion == Emotion.Happiness)
            {
                if (Animator.GetCurrentAnimatorStateInfo(1).IsName("Idle") && !_isMakeEffect)
                {
                    _happyIdleEffect = Global.Instance.ResourceManager.GetObject(Constant.HappyIdleEffect, transform);
                    _happyIdleEffect.transform.localPosition = new Vector3(0, -0.5f, 0);
                    _happyIdleEffect.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                    _isMakeEffect = true;
                }
                else if (!Animator.GetCurrentAnimatorStateInfo(1).IsName("Idle") && _happyIdleEffect != null)
                {
                    Global.Instance.ResourceManager.ReturnObject(Constant.HappyIdleEffect, _happyIdleEffect);
                    _happyIdleEffect = null;
                    _isMakeEffect = false;
                }

                if (Animator.GetCurrentAnimatorStateInfo(1).IsName("Walk") && !_isMakeEffect)
                {
                    _happyWalkEffect = Global.Instance.ResourceManager.GetObject(Constant.HappyWalkEffect, transform);
                    _happyWalkEffect.transform.localPosition = new Vector3(0, -0.5f, 0);
                    _happyWalkEffect.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                    _isMakeEffect = true;
                }
                else if (!Animator.GetCurrentAnimatorStateInfo(1).IsName("Walk") && _happyWalkEffect != null)
                {
                    Global.Instance.ResourceManager.ReturnObject(Constant.HappyWalkEffect, _happyWalkEffect);
                    _happyWalkEffect = null;
                    _isMakeEffect = false;
                }

                if (Animator.GetCurrentAnimatorStateInfo(1).IsName("Run") && !_isMakeEffect)
                {
                    _happyRunEffect = Global.Instance.ResourceManager.GetObject(Constant.HappyRunEffect, transform);
                    _happyRunEffect.transform.localPosition = new Vector3(0, -0.5f, 0);
                    _happyRunEffect.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                    _isMakeEffect = true;
                }
                else if (!Animator.GetCurrentAnimatorStateInfo(1).IsName("Run") && _happyRunEffect != null)
                {
                    Global.Instance.ResourceManager.ReturnObject(Constant.HappyRunEffect, _happyRunEffect);
                    _happyRunEffect = null;
                    _isMakeEffect = false;
                }
            }
            else if(_happyIdleEffect != null)
            {
                Global.Instance.ResourceManager.ReturnObject(Constant.HappyIdleEffect, _happyIdleEffect);
                _happyIdleEffect = null;
                _isMakeEffect = false;
            }
            else if (_happyWalkEffect != null)
            {
                Global.Instance.ResourceManager.ReturnObject(Constant.HappyIdleEffect, _happyWalkEffect);
                _happyWalkEffect = null;
                _isMakeEffect = false;
            }
            else if (_happyRunEffect != null)
            {
                Global.Instance.ResourceManager.ReturnObject(Constant.HappyIdleEffect, _happyRunEffect);
                _happyRunEffect = null;
                _isMakeEffect = false;
            }


            if (CurrentEmotion == Emotion.Anger)
            {
                if (Animator.GetCurrentAnimatorStateInfo(2).IsName("Idle") && !_isMakeEffect)
                {
                    _angerIdleEffect = Global.Instance.ResourceManager.GetObject(Constant.AngerIdleEffect, transform);
                    _angerIdleEffect.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                    _isMakeEffect = true;
                }
                else if (!Animator.GetCurrentAnimatorStateInfo(2).IsName("Idle") && _angerIdleEffect != null)
                {
                    Global.Instance.ResourceManager.ReturnObject(Constant.AngerIdleEffect, _angerIdleEffect);
                    _angerIdleEffect = null;
                    _isMakeEffect = false;
                }

                if (Animator.GetCurrentAnimatorStateInfo(2).IsName("Walk") && !_isMakeEffect)
                {
                    _angerWalkEffect = Global.Instance.ResourceManager.GetObject(Constant.AngerWalkEffect, transform);
                    _angerWalkEffect.transform.localPosition = new Vector3(0, -1f, 0);
                    _isMakeEffect = true;
                }
                else if (!Animator.GetCurrentAnimatorStateInfo(2).IsName("Walk") && _angerWalkEffect != null)
                {
                    Global.Instance.ResourceManager.ReturnObject(Constant.AngerWalkEffect, _angerWalkEffect);
                    _angerWalkEffect = null;
                    _isMakeEffect = false;
                }

                if (Animator.GetCurrentAnimatorStateInfo(2).IsName("Run") && !_isMakeEffect)
                {
                    _angerRunEffect = Global.Instance.ResourceManager.GetObject(Constant.AngerRunEffect, transform);
                    _angerRunEffect.transform.localPosition = new Vector3(0, -0.5f, 0);
                    _angerRunEffect.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                    _isMakeEffect = true;
                }
                else if (!Animator.GetCurrentAnimatorStateInfo(2).IsName("Run") && _angerRunEffect != null)
                {
                    Global.Instance.ResourceManager.ReturnObject(Constant.AngerRunEffect, _angerRunEffect);
                    _angerRunEffect = null;
                    _isMakeEffect = false;
                }
            }
            else if (_angerIdleEffect != null)
            {
                Global.Instance.ResourceManager.ReturnObject(Constant.AngerIdleEffect, _angerIdleEffect);
                _angerIdleEffect = null;
                _isMakeEffect = false;
            }
            else if (_angerWalkEffect != null)
            {
                Global.Instance.ResourceManager.ReturnObject(Constant.AngerIdleEffect, _angerWalkEffect);
                _angerWalkEffect = null;
                _isMakeEffect = false;
            }
            else if (_angerRunEffect != null)
            {
                Global.Instance.ResourceManager.ReturnObject(Constant.AngerRunEffect, _angerRunEffect);
                _angerRunEffect = null;
                _isMakeEffect = false;
            }
        }

        public void TransEffect()
        {
            GameObject obj;
            switch (CurrentEmotion)
            {
                case Emotion.Anger:
                    CanMove = false;
                    obj = Global.Instance.ResourceManager.GetObject(Constant.AngerTransEffectPath, transform);
                    Global.Instance.ResourceManager.ReturnParticleSystem(Constant.AngerTransEffectPath, obj);
                    obj.transform.localPosition = new Vector3(0, -1, 0.5f);
                    break;

                case Emotion.Sadness:
                    CanMove = false;
                    obj = Global.Instance.ResourceManager.GetObject(Constant.SadTransEffectPath, transform);
                    Global.Instance.ResourceManager.ReturnParticleSystem(Constant.SadTransEffectPath, obj);
                    obj.transform.localPosition = new Vector3(0, -1, 0);
                    break;

                case Emotion.Happiness:
                    CanMove = false;
                    obj = Global.Instance.ResourceManager.GetObject(Constant.HappyTransEffectPath, transform);
                    Global.Instance.ResourceManager.ReturnParticleSystem(Constant.HappyTransEffectPath, obj);
                    obj.transform.localPosition = new Vector3(0, -1, 0);
                    break;
            }
        }
    }
}
