using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class PuzzleContext
    {
        public Emotion SkillEmotion;

        public PuzzleContext(Emotion emotion)
        {
            SkillEmotion = emotion;
        }
    }
    
    public abstract class PuzzleElements : MonoBehaviour
    {
        public bool Enable { get; set; } = true;

        public bool IsNotify { get; protected set; }
        //상태 update 메서드
        public abstract void OnNotify(PuzzleContext context);
    }
}
