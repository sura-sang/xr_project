using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class SadnessMove : MonsterMoveState
    {
        private Sadness _sadness;
        public SadnessMove(CharacterMove characterMove,Sadness sadness) : base(characterMove) 
        {
            _sadness = sadness;
        }

        public override void InitializeState() { }

        public override void UpdateState() 
        {
            if (_monster.PlayerTransform.GetComponent<Player>().CurrentEmotion == Emotion.Happiness
                && Vector3.Distance(_monster.PlayerTransform.position, _monster.transform.position) < HappySkill.CheckRange)
            {
                _agent.isStopped = false;
                _agent.SetDestination(_monster.PlayerTransform.position);
            }
            else
            {
                _monster.ChangeState(new SadnessIdle(_monster, _sadness));
            }
        }

        public override void ClearState()
        {
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
            _sadness.IsFollow = false;
        }
    }
}
