using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class MonsterMoveSleep : MonsterMoveState
    {
        private bool _effectCheck = false;
        private float _exitTime = 0.6f;
        public MonsterMoveSleep(CharacterMove characterMove) : base(characterMove) { }

        //아무것도 안하고 아무것도 할 수 없음

        public override void InitializeState()
        {

        }

        public override void UpdateState()
        { 
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= _exitTime && 
                _animator.GetCurrentAnimatorStateInfo(0).IsName("Absorbed") && !_effectCheck)
            {
                switch(_monster.Emotion)
                {
                    case Emotion.Happiness:
                        var HappyEffectPos = _monster as Happiness;
                        var obj1 = Global.Instance.ResourceManager.GetObject(Constant.SleepEffect, HappyEffectPos.SleepPos);
                        obj1.transform.localScale = _monster.transform.localScale;
                        obj1.transform.localPosition = new Vector3(0, 0.5f, 0);
                        _effectCheck = true;
                        break;

                    case Emotion.Sadness:
                        var SadEffectPos = _monster as Sadness;
                        var obj2 = Global.Instance.ResourceManager.GetObject(Constant.SleepEffect, SadEffectPos.SleepPos);
                        obj2.transform.localScale = _monster.transform.localScale;
                        obj2.transform.localPosition = new Vector3(0,0.5f,0);
                        _effectCheck = true;
                        break;

                    case Emotion.Anger:
                        var AngerEffectPos = _monster as Anger;
                        var obj3 = Global.Instance.ResourceManager.GetObject(Constant.SleepEffect, AngerEffectPos.SleepPos);
                        obj3.transform.localScale = _monster.transform.localScale;
                        obj3.transform.localPosition = new Vector3(0, 0.5f, 0);
                        _effectCheck = true;
                        break;
                }
            }

        }

        public override void ClearState() { }
    }
}