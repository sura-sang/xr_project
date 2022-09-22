using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Emotion
{
    Happiness,
    Sadness,
    Anger,
    Sleep,
    Default,
}

namespace SuraSang
{
    public abstract class Monster : MonoBehaviour
    {
        public abstract Emotion getEmotion();
    }
}
