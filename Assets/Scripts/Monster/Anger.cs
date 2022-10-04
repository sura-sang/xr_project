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
            PlayerTransform = GameObject.Find("PlayerDummy").GetComponent<Transform>();

            ChangeState(new MonsterMoveChase(this));
        }
    }
}
