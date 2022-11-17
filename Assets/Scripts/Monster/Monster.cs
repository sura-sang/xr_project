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
        public NavMeshAgent Agent { get; protected set; }
        public Animator Animator { get; protected set; }

        public abstract Emotion Emotion { get; }

        public bool IsSleep { get; private set; } = false;

        public void Absorbed()
        {
            //Material material = Resources.Load<Material>("Sleep");
            ChangeState(new MonsterMoveSleep(this));
            //this.gameObject.GetComponentInChildren<Renderer>().material = material;
            this.gameObject.GetComponentInChildren<Light>().enabled = false;
            this.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            IsSleep = true;
        }

        public virtual void NextState()
        {
            // TODO 공용으로 사용하는 상태의 경우 다음 상태를 어케 해야할지?
        }

        public override void MovePosition(Vector3 pos)
        {
            transform.position = pos;
            Agent.Move(pos);
        }
    }
}
