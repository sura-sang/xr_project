using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class MonsterMoveSleep : MonsterMoveState
    {
        public MonsterMoveSleep(CharacterMove characterMove) : base(characterMove) { }

        //아무것도 안하고 아무것도 할 수 없음

        public override void InitializeState() { }

        public override void UpdateState() { }

        public override void ClearState() { }
    }
}