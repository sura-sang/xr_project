using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SuraSang
{
    public class Anger : Monster
    {
        public override Emotion Emotion => Emotion.Anger;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            
            ChangeState(new MonsterMoveChase(this));
        }
    }
}
