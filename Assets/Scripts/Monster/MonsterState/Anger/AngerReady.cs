using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SuraSang
{
    public class AngerReady : MonsterMoveState
    {
        private Player _player;
        private Anger _anger;

        private float _readyStartTime;

        private Vector3 _startPos;
        private Vector3 _dir;

        public AngerReady(CharacterMove characterMove) : base(characterMove) { }

        public override void InitializeState()
        {
            _player = Global.Instance.SceneMaster.Player;
            _anger = _monster as Anger;
            
            _readyStartTime = Time.time;

            _agent.SetDestination(_anger.transform.position);
            _startPos = _anger.transform.position;

            _dir = (_player.transform.position - _anger.transform.position).normalized;

            _animator.SetBool("IsWalking", false);
            _animator.SetTrigger("Ready");
        }

        public override void UpdateState()
        {
            if (Time.time - _readyStartTime > _anger.DashReadyTime)
            {
                _anger.ChangeState(new AngerDash(_characterMove, _dir));
            }

            _anger.SmoothRotation(_dir);

            _anger.MovePosition(_startPos + (_anger.transform.rotation * _anger.MoveDummy.localPosition));
        }

        public override void ClearState() { }
    }
}