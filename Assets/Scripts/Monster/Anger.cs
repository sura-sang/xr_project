using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SuraSang
{
    public class Anger : Monster
    {
        [SerializeField] private Transform _moveDummy;
        public Transform MoveDummy => _moveDummy;

        public override Emotion Emotion => Emotion.Anger;

        public float ChaseRange;
        public float SkillRange;
        public float SkillCooltime;

        public LayerMask DashCheckLayerMask;

        public float DashReadyTime;
        public float DashSpeed;
        public float MaxDashTime;

        public float StunTime;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();

            Agent.updateRotation = false;

            ChangeState(new AngerIdle(this));
        }

        public void WalkEffect()
        {
            var obj = Global.Instance.ResourceManager.GetObject(Constant.AngerWalkEffect, transform);
            Global.Instance.ResourceManager.ReturnParticleSystem(Constant.AngerWalkEffect, obj);
        }
    }
}