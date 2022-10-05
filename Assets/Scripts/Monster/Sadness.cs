using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SuraSang
{
    public class Sadness : Monster
    {
        public override Emotion Emotion => Emotion.Sadness;
        public bool IsFollow;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            
            ChangeState(new SadnessIdle(this,this));
        }
    }
}
