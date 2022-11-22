using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace SuraSang
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [BankRef] public List<string> Banks;

        [Header("Player")]
        public FMODUnity.EventReference SFX_P_Footstep;
        public FMODUnity.EventReference SFX_P_Jump;
        public FMODUnity.EventReference SFX_P_Drop;
        public FMODUnity.EventReference SFX_P_Hang;
        public FMODUnity.EventReference SFX_P_Intro_Fall;
        public FMODUnity.EventReference SFX_P_Intro_Drop;
        public FMODUnity.EventReference SFX_P_AB;

        [Header("SKill")]
        public FMODUnity.EventReference SFX_P_Crash;
        public FMODUnity.EventReference SFX_P_Rush;
        public FMODUnity.EventReference SFX_P_Cry;
        public FMODUnity.EventReference SFX_P_Dance;

        [Header("Monster")]
        public FMODUnity.EventReference SFX_M_Crash;
        public FMODUnity.EventReference SFX_M_Cry;
        public FMODUnity.EventReference SFX_M_Rush;
        public FMODUnity.EventReference SFX_M_Sleep;
        public FMODUnity.EventReference SFX_M_Grow;
        public FMODUnity.EventReference SFX_M_POP;

        [Header("Object")]
        public FMODUnity.EventReference SFX_OB_Tree_Fall;
        public FMODUnity.EventReference SFX_OB_Tree_Flight;
        public FMODUnity.EventReference SFX_OB_Tree_Drop;
        public FMODUnity.EventReference SFX_OB_Zamiwa;
        public FMODUnity.EventReference SFX_OB_Water;
        public FMODUnity.EventReference SFX_OB_Waterfall;

        [Header("UI")]
        public FMODUnity.EventReference SFX_UI_Helper;
        public FMODUnity.EventReference SFX_UI_PressKey;
        public FMODUnity.EventReference SFX_UI_Click;
        public FMODUnity.EventReference SFX_UI_Select;

        [Header("AMB")]
        public FMODUnity.EventReference AMB_Forest_1;
        public FMODUnity.EventReference AMB_Forest_2;
        public FMODUnity.EventReference AMB_Wind;
        public FMODUnity.EventReference AMB_Zamiwa;

        [Header("BGM")]
        public FMODUnity.EventReference BGM_Full;
        public FMODUnity.EventReference BGM_Hightlight;
        public FMODUnity.EventReference BGM_Nonhighlight;

        [Header("CutScene")]
        public FMODUnity.EventReference CS_OP;
        public FMODUnity.EventReference CS_ED;

        public FMOD.Studio.EventInstance PlayerState;

        private void Awake()
        {
            foreach (var bankRef in Banks)
            {
                try
                {
                    RuntimeManager.LoadBank(bankRef, false);
                }
                catch (BankLoadException e)
                {
                    RuntimeUtils.DebugLogException(e);
                }
            }

            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (Instance != this)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        public void SoundOneShot2D(EventReference audioEvent)
        {
            RuntimeManager.PlayOneShot(audioEvent);
        }

        public void SoundOneShot3D(EventReference audioEvent, Transform transform)
        {
            RuntimeManager.PlayOneShot(audioEvent, transform.position);
        }

        public void PlayBGM(EventReference audioEvent)
        {
            PlayerState.release();
            PlayerState = FMODUnity.RuntimeManager.CreateInstance(audioEvent);
            PlayerState.start();
        }
    }
}
