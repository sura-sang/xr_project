using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SuraSang
{
    public class Sadness : Monster
    {
        public override Emotion Emotion => Emotion.Sadness;
        public bool IsFollow;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();

            ChangeState(new SadnessIdle(this));
        }

        public void SleepSound()
        {
            AudioManager.Instance.SoundOneShot3D(AudioManager.Instance.SFX_M_Sleep, transform);
        }
    }
}
