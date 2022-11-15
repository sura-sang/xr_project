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

        public AngerStun(CharacterMove characterMove) : base(characterMove) { }

        public override void InitializeState()
        {
            _anger = _monster as Anger;

            _stunStartTime = Time.time;

            _animator.SetTrigger("Stun");
        }

        public override void UpdateState()
        {
            if (Time.time - _stunStartTime > _anger.StunTime)
            {
                _anger.ChangeState(new AngerIdle(_characterMove));
            }
        }

        public override void ClearState() { }
    }
}