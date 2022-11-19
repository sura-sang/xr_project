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

        private float _timer;
        private float _radius;
        private Transform _moveRange;
        private float _randomSec;
        private Happiness _happiness;

        private GameObject _HappyEff;

        public override void InitializeState() 
        {
            _happiness = _monster as Happiness;
            FineNearestTag();

            _radius = _moveRange.GetComponent<MoveRange>().MoveRadius;         
            _randomSec = 1f;

            _monster.RandomNavSphere(_moveRange.position, _radius);

            _HappyEff = Global.Instance.ResourceManager.GetObject(Constant.HappySmileEffect, _happiness.transform);
            _HappyEff.transform.localPosition = new Vector3(-0.7f, 1.56f, 0);
            _HappyEff.transform.localRotation = Quaternion.Euler(-36, -90, 0);
        }

        public override void UpdateState() 
        {
            _timer += Time.deltaTime;

            if (_timer >= _randomSec)
            {
                _monster.RandomNavSphere(_moveRange.position, _radius);
                _timer = 0;
            }

            _animator?.SetBool("IsWalking", _agent.isStopped);

            _monster.SmoothRotation(_agent.velocity);
        }

        public override void ClearState() 
        {
            if(_HappyEff != null)
            {
                Global.Instance.ResourceManager.ReturnObject(Constant.HappySmileEffect, _HappyEff);
                _happiness = null;
            }
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
    }
}
