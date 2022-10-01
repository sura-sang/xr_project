using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class SadnessIdle : MonsterMoveState
    {
        public SadnessIdle(CharacterMove characterMove) : base(characterMove) { }
        private Sadness _sadness;

        public override void InitializeState() 
        {
            _sadness = GameObject.Find("Sadness").GetComponent<Sadness>();
        }

        public override void UpdateState()
        { 
            if(_sadness.IsFollow)
            {
                _monster.ChangeState(new SadnessMove(_monster));
            }
        }

        public override void ClearState() { }
    }
}
