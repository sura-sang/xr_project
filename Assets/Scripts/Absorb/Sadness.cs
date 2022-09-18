using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class Sadness : Monster
    {
        public Emotion MyEmotion;

        public override Emotion getEmotion()
        {
            return MyEmotion;
        }
    }
}
