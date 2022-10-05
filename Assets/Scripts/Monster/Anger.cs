using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SuraSang
{
    public class Anger : Monster
    {
        public override Emotion Emotion => Emotion.Anger;

        public float ChaseRange;
        public float SkillRange;
        public float SkillCooltime;

        public LayerMask DashCheckLayerMask;

        public float DashSpeed;
        public float MaxDashTime;


        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Agent.updateRotation = false;

            ChangeState(new AngerIdle(this));
        }
    }
}