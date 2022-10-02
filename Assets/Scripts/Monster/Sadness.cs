using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SuraSang
{
    public class Sadness : Monster
    {
        public override Emotion Emotion => Emotion.Sadness;
        
        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            PlayerTransform = GameObject.Find("PlayerDummy").GetComponent<Transform>();

            ChangeState(new SadnessIdle(this));
        }
    }
}
