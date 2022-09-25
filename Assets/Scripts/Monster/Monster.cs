using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public enum Emotion
{
    Happiness,
    Sadness,
    Anger,
    Default,
}

namespace SuraSang
{
    public abstract class Monster : CharacterMove
    {
        public NavMeshAgent Agent { get; protected set; }

        public abstract Emotion Emotion { get; }
        
        public bool IsSleep { get; private set; } = false;

        public void Absorbed()
        {
            ChangeState(new MonsterMoveSleep(this));
        }
    }
}
