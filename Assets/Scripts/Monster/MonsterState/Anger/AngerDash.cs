using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SuraSang
{
    public class AngerDash : MonsterMoveState
    {
        private Player _player;
        private Anger _anger;

        private Vector3 _dir;

        private float _dashStartTime;

        private float _radius;

        public AngerDash(CharacterMove characterMove) : base(characterMove) { }

        public override void InitializeState()
        {
            _player = Global.Instance.SceneMaster.Player;
            _anger = _monster as Anger;

            _dir = (_player.transform.position - _anger.transform.position).normalized;

            _dashStartTime = Time.time;
            
            

            _radius = _agent.radius;
        }

        public override void UpdateState()
        {
            var move = _dir * _anger.DashSpeed * Time.deltaTime;
            _agent.Move(move);
            _anger.SmoothRotation(_dir);

            var capsulePoint1 = _anger.transform.position - Vector3.up * _agent.baseOffset;
            var capsulePoint2 = capsulePoint1 + Vector3.up * _agent.height;
            
            var result = Physics.CapsuleCastAll(capsulePoint1, capsulePoint2,
                _radius, _dir, _anger.DashSpeed * Time.deltaTime, _anger.DashCheckLayerMask);

            foreach (var hit in result)
            {
                // 충돌 행동
                Debug.Log(hit.transform.gameObject.name);
            }

            if (result.Length != 0 || (Time.time - _dashStartTime) > _anger.MaxDashTime)
            {
                _agent.SetDestination(_anger.transform.position);
                _anger.ChangeState(new AngerIdle(_characterMove));
            }
        }

        public override void ClearState() { }
    }
}