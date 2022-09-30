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

        public override void InitializeState() 
        {
            _radius = 5f;
            FineNearestTag();
            //_moveRange = GameObject.FindWithTag("MoveRange").transform;

            _destination = RandomNavSphere(_monster.transform.position, _radius, -1);
        }

        public override void UpdateState() 
        {
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

                if(dist <= shortDis)
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
