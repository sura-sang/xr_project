using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SuraSang
{
    public class AngerStun : MonsterMoveState
    {
        private Anger _anger;

        private float _stunStartTime;
        private GameObject _stunEffect;

        public AngerStun(CharacterMove characterMove) : base(characterMove) { }

        public override void InitializeState()
        {
            _anger = _monster as Anger;

            _stunStartTime = Time.time;

            _animator.SetTrigger("Stun");
            _stunEffect = Global.Instance.ResourceManager.GetObject(Constant.AngerDizzyEffect, _anger.transform);
            _stunEffect.transform.localPosition = new Vector3(0, 2, 0);

            AudioManager.Instance.SoundOneShot3D(AudioManager.Instance.SFX_M_Crash, _anger.transform);
        }

        public override void UpdateState()
        {
            if (Time.time - _stunStartTime > _anger.StunTime)
            {
                _anger.ChangeState(new AngerIdle(_characterMove));

                if(_stunEffect != null)
                {
                    Global.Instance.ResourceManager.ReturnObject(Constant.AngerDizzyEffect, _stunEffect);
                }
            }
        }

        public override void ClearState() { }
    }
}