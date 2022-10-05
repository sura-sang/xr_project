using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class AngerIdle : MonsterMoveState
    {
        private Player _player;
        private Anger _anger;

        public AngerIdle(CharacterMove characterMove) : base(characterMove) { }

        public override void InitializeState()
        {
            _player = GameManager.Instance.Player;
            _anger =_monster as Anger;
        }

        public override void UpdateState()
        {
            if (Vector3.Distance(_monster.transform.position,
                    _player.transform.position) < _anger.ChaseRange)
            {
                _anger.ChangeState(new AngerChase(_characterMove));
            }
        }

        public override void ClearState() { }
    }
}