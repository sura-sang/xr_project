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
        public bool IsDisableAgent = false;

        public Transform SleepPos;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();

            FMOD.Studio.EventInstance inst = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/NPC/SFX_M_Cry_X");
            inst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            inst.start();
            inst.release();

            ChangeState(new SadnessIdle(this));
        }

        public void SleepSound()
        {
            AudioManager.Instance.SoundOneShot3D(AudioManager.Instance.SFX_M_Sleep, transform);
        }
    }
}
