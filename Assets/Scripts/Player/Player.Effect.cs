using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public partial class Player
    {
        private GameObject _happyEffect;
        private GameObject _angerSkillRushEffect;
        private GameObject _angerSkillCrashEffect;

        private GameObject _angerIdleEffect;
        private GameObject _happyIdleEffect;

        private bool _isMakeEffect = false;

        private void ChangeEffect()
        {
            if (IsSkill)
            {
                if (CurrentEmotion == Emotion.Happiness && _happyEffect == null)
                {
                    _happyEffect = Global.Instance.ResourceManager.GetObject(Constant.HappySkillEffectPath, transform);
                    _happyEffect.transform.localPosition = new Vector3(0, -1, 0);
                }
                else if (CurrentEmotion == Emotion.Anger && _angerSkillRushEffect == null)
                {
                    _angerSkillRushEffect = Global.Instance.ResourceManager.GetObject(Constant.AngerSkillRushEffectPath, transform);
                    _angerSkillRushEffect.transform.localPosition = new Vector3(0, -0.4f, -0.2f);
                    _angerSkillRushEffect.transform.localRotation = Quaternion.Euler(0, transform.rotation.y, 0);
                }
            }
            else if (_happyEffect != null)
            {
                Global.Instance.ResourceManager.ReturnObject(Constant.HappySkillEffectPath, _happyEffect);
                _happyEffect = null;
            }
            else if (_angerSkillRushEffect != null)
            {
                Global.Instance.ResourceManager.ReturnObject(Constant.AngerSkillRushEffectPath, _angerSkillRushEffect);
                _angerSkillRushEffect = null;
            }
        }

        public void ChangeStateEffect()
        {
            switch(CurrentEmotion)
            {
                case Emotion.Default:

                    if (_angerIdleEffect != null)
                    {
                        Global.Instance.ResourceManager.ReturnObject(Constant.AngerIdleEffect, _angerIdleEffect);
                        _angerIdleEffect = null;
                        _isMakeEffect = false;
                    }

                    if (_happyIdleEffect != null)
                    {
                        Global.Instance.ResourceManager.ReturnObject(Constant.HappyIdleEffect, _happyIdleEffect);
                        _happyIdleEffect = null;
                        _isMakeEffect = false;
                    }
                    break;

                case Emotion.Sadness:
                    if (_angerIdleEffect != null)
                    {
                        Global.Instance.ResourceManager.ReturnObject(Constant.AngerIdleEffect, _angerIdleEffect);
                        _angerIdleEffect = null;
                        _isMakeEffect = false;
                    }

                    if (_happyIdleEffect != null)
                    {
                        Global.Instance.ResourceManager.ReturnObject(Constant.HappyIdleEffect, _happyIdleEffect);
                        _happyIdleEffect = null;
                        _isMakeEffect = false;
                    }
                    break;

                case Emotion.Happiness:
                    if (Animator.GetCurrentAnimatorStateInfo(1).IsName("Idle") && !_isMakeEffect)
                    {
                        _happyIdleEffect.SetActive(true);
                        _happyIdleEffect = Global.Instance.ResourceManager.GetObject(Constant.HappyIdleEffect, transform);
                        _happyIdleEffect.transform.localPosition = new Vector3(0, -0.5f, 0);
                        _happyIdleEffect.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                        _isMakeEffect = true;
                    }
                    else
                        _happyIdleEffect.SetActive(false);

                    if(_angerIdleEffect != null)
                    {
                        Global.Instance.ResourceManager.ReturnObject(Constant.AngerIdleEffect, _angerIdleEffect);
                        _angerIdleEffect = null;
                        _isMakeEffect = false;
                    }
                    break;

                case Emotion.Anger:
                    if (Animator.GetCurrentAnimatorStateInfo(2).IsName("Idle") && !_isMakeEffect)
                    {
                        _angerIdleEffect = Global.Instance.ResourceManager.GetObject(Constant.AngerIdleEffect, transform);
                        _angerIdleEffect.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                        _isMakeEffect = true;
                    }
                    if (_happyIdleEffect != null)
                    {
                        Global.Instance.ResourceManager.ReturnObject(Constant.HappyIdleEffect, _happyIdleEffect);
                        _happyIdleEffect = null;
                        _isMakeEffect = false;
                    }
                    break;
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
                    obj.transform.localPosition = new Vector3(0, -1, 0);
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
