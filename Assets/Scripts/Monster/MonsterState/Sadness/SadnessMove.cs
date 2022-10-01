using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class SadnessMove : MonsterMoveState
    {
        public SadnessMove(CharacterMove characterMove) : base(characterMove) { }
        private Sadness _sadness;

        public override void InitializeState()
        {
            _sadness = GameObject.Find("Sadness").GetComponent<Sadness>();
        }

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
                _monster.ChangeState(new SadnessIdle(_monster));
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
