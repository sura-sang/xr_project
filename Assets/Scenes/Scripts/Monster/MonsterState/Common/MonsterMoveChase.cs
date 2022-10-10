using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class MonsterMoveChase : MonsterMoveState
    {
        public MonsterMoveChase(CharacterMove characterMove) : base(characterMove) { }

        public override void InitializeState() { }

        public override void UpdateState()
        {
            //_agent.SetDestination(Vector3.zero);
        }

        public override void ClearState() { }
    }
}
