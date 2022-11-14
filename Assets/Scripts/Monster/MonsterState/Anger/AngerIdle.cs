using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class AngerIdle : MonsterMoveState
    {
        private Player _player;
        private Anger _anger;

        private float _timer;
        private float _radius;
        private float _randomSec;
        private Transform _moveRange;

        public AngerIdle(CharacterMove characterMove) : base(characterMove) { }

        public override void InitializeState()
        {
            FineNearestTag();
            _radius = _moveRange.GetComponent<MoveRange>().MoveRadius;
            _randomSec = 1;

            _player = Global.Instance.SceneMaster.Player;
            _anger =_monster as Anger;
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


            if (Vector3.Distance(_monster.transform.position, _player.transform.position) < _anger.ChaseRange)
            {
                _anger.ChangeState(new AngerChase(_characterMove));
            }

            _anger.SmoothRotation(_agent.velocity);
        }

        public override void ClearState() { }

        private void FineNearestTag()
        {
            GameObject[] moveRange = GameObject.FindGameObjectsWithTag("MoveRange");
            float shortDis = 0;

            if (moveRange.Length != 0)
            {
                shortDis = Vector3.Distance(_monster.transform.position, moveRange[0].transform.position);
            }

            foreach (GameObject mRange in moveRange)
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