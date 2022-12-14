using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public enum Emotion
{
    Default,
    Anger,
    Happiness,
    Sadness,
}

namespace SuraSang
{
    public abstract class Monster : CharacterMove
    {
        [SerializeField] private bool _isAbsorbAble = true;
        public bool IsAbsorbAble => _isAbsorbAble;

        public NavMeshAgent Agent { get; protected set; }
        public Animator Animator { get; protected set; }

        public abstract Emotion Emotion { get; }

        public bool IsSleep { get; private set; } = false;

        public virtual void Absorbed()
        {
            ChangeState(new MonsterMoveSleep(this));
            //this.gameObject.GetComponentInChildren<Light>().enabled = false;
            this.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            IsSleep = true;

            foreach(var renderer in this.gameObject.GetComponentsInChildren<Renderer>())
            {
                renderer.material.SetFloat("_UseOutline", 0);
            }
        }

        public virtual void UnAbsorbed(Monster monster)
        {
            var obj = monster as Sadness;
            if (!obj.IsDisableAgent)
                this.GetComponent<NavMeshAgent>().enabled = true;
            IsSleep = true;
        }

        public virtual void NextState()
        {
            // TODO 공용으로 사용하는 상태의 경우 다음 상태를 어케 해야할지?
        }

        public override void MovePosition(Vector3 pos)
        {
            transform.position = pos;
            Agent.SetDestination(pos);
        }

        public void RandomNavSphere(Vector3 origin, float dist, int layermask = NavMesh.AllAreas)
        {
            Vector3 randPos = origin + Random.insideUnitSphere * dist;

            if (NavMesh.SamplePosition(randPos, out var navHit, 1.0f, layermask))
            {
                Agent.SetDestination(navHit.position);
            }
        }
    }
}
