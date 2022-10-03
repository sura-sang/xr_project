using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class SadnessIdle : MonsterMoveState
    {
        private Sadness _sadness;
        public SadnessIdle(CharacterMove characterMove, Sadness sadness) : base(characterMove)
        {
            _sadness = sadness;
        }

        public override void InitializeState() { }
        public override void UpdateState()
        {
            if (_sadness.IsFollow)
            {
                _monster.ChangeState(new SadnessMove(_monster,_sadness));
            }
        }

        public override void ClearState() { }
    }
}
