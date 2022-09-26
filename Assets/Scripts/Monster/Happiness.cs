using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SuraSang
{
    public class Happiness : Monster
    {
        public override Emotion Emotion => Emotion.Happiness;
        
        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            
            ChangeState(new MonsterMoveChase(this));
        }
    }
}
