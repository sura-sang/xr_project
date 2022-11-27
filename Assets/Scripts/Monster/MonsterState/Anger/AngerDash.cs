using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SuraSang
{
    public class AngerDash : MonsterMoveState
    {
        private Anger _anger;

        private Vector3 _dir;

        private float _dashStartTime;

        private float _radius;

        public AngerDash(CharacterMove characterMove, Vector3 dir) : base(characterMove)
        {
            _dir = dir;
        }

        public override void InitializeState()
        {
            _anger = _monster as Anger;

            _dashStartTime = Time.time;

            _animator.SetTrigger("Attack");

            _radius = _agent.radius * 2;
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

                if (hit.transform.gameObject.tag == "Player")
                {
                    Debug.LogError("플레이어 맞음");
                    hit.transform.gameObject.GetComponent<Animator>().SetTrigger("Hit");
                }

            }

            if (result.Length != 0 || (Time.time - _dashStartTime) > _anger.MaxDashTime)
            {
                _agent.SetDestination(_anger.transform.position);

                _anger.ChangeState(new AngerStun(_characterMove));
            }
        }

        public override void ClearState() { }
    }
}