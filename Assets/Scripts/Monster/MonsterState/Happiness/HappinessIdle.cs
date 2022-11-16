using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace SuraSang
{
    public class HappinessIdle : MonsterMoveState
    {
        public HappinessIdle(CharacterMove characterMove) : base(characterMove) { }

        private Vector3 _destination;
        private float _timer;
        private float _radius;
        private Transform _moveRange;
        private float _randomSec;

        public override void InitializeState() 
        {
            FineNearestTag();

            _radius = _moveRange.GetComponent<HappyMoveRange>().MoveRadius;         
            _randomSec = 1f;

            _destination = RandomNavSphere(_monster.transform.position, _radius, -1);
        }

        public override void UpdateState() 
        {
            _timer += Time.deltaTime;

            if (_timer >= _randomSec)
            {
                _destination = RandomNavSphere(_monster.transform.position, _radius, -1);
                _timer = 0;
            }

            if (CanMove())
            {
                _agent.isStopped = false;
                _agent.SetDestination(_destination);
                _animator.SetBool("IsWalking", true);
            }
            else
            {
                _agent.isStopped = true;
                _animator.SetBool("IsWalking", false);
            }
        }

        public override void ClearState() 
        {
            
        }

        private void FineNearestTag()
        {
            GameObject[] moveRange = GameObject.FindGameObjectsWithTag("MoveRange");
            float shortDis = 0;

            if (moveRange.Length != 0)
            {
                shortDis = Vector3.Distance(_monster.transform.position, moveRange[0].transform.position);
            }

            foreach(GameObject mRange in moveRange)
            {
                var dist = Vector3.Distance(_monster.transform.position, mRange.transform.position);

                if (dist <= shortDis)
                {
                    _moveRange = mRange.transform;                 
                }             
            }
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
            if (Vector3.Distance(_moveRange.transform.position, _destination) <= _radius)
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
