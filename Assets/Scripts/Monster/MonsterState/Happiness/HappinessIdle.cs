using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SuraSang
{
    public class HappinessIdle : MonsterMoveState
    {
        public HappinessIdle(CharacterMove characterMove) : base(characterMove) { }

        private Vector3 _destination;
        private float _timer;
        private float _radius;
        private Transform _moveRange;
        private bool _canMove;

        public override void InitializeState() 
        {
            _radius = 5f;
            _moveRange = GameObject.FindWithTag("MoveRange").transform;
            _destination = RandomNavSphere(_monster.transform.position, _radius, -1);
        }

        public override void UpdateState() 
        {
            _canMove = CanMove();
            _timer += Time.deltaTime;

            if (_timer >= 1f)
            {
                _destination = RandomNavSphere(_monster.transform.position, _radius, -1);
                _timer = 0;
            }

            if (CanMove())
            {
                _agent.isStopped = false;
                _agent.SetDestination(_destination);
            }
            else
            {
                _agent.isStopped = true;
            }
        }

        public override void ClearState() 
        { 
        }

        private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
        {
            Vector3 randDirection = Random.insideUnitSphere * dist;
            randDirection += origin;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

            return navHit.position;
        }

        private bool CanMove()
        {
            if (Vector3.Distance(_moveRange.transform.position, _destination) <= 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
