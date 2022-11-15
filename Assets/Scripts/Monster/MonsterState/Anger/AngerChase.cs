using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class AngerChase : MonsterMoveState
    {
        private Player _player;
        private Anger _anger;
        
        public AngerChase(CharacterMove characterMove) : base(characterMove) { }

        public override void InitializeState()
        {
            _player = Global.Instance.SceneMaster.Player;
            _anger = _monster as Anger;
        }

        public override void UpdateState()
        {
            _agent.SetDestination(_player.transform.position);
            _anger.SmoothRotation(_agent.velocity);

            var distance = Vector3.Distance(_monster.transform.position,
                _player.transform.position);

            if (distance > _anger.ChaseRange)
            {
                _anger.ChangeState(new AngerIdle(_characterMove));
            }
            else if (distance < _anger.SkillRange)
            {
                _anger.ChangeState(new AngerReady(_characterMove));
            }
        }

        public override void ClearState() { }
    }
}