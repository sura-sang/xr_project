using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using FMOD.Studio;

namespace SuraSang
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Debug")]
        [SerializeField] private bool _bgmOff;

        [Header("Link Banks")]
        [BankRef] public List<string> Banks;

        [Header("Player")]
        public FMODUnity.EventReference SFX_P_Footstep;
        public FMODUnity.EventReference SFX_P_Jump;
        public FMODUnity.EventReference SFX_P_Drop;
        // public FMODUnity.EventReference SFX_P_Hang;
        // public FMODUnity.EventReference SFX_P_Intro_Fall;
        // public FMODUnity.EventReference SFX_P_Intro_Drop;
        public FMODUnity.EventReference SFX_P_AB;
        public FMODUnity.EventReference SFX_P_AB_2;

        [Header("SKill")]
        public FMODUnity.EventReference SFX_P_Crash;
        public FMODUnity.EventReference SFX_P_Rush;
        public FMODUnity.EventReference SFX_P_Cry;
        public FMODUnity.EventReference SFX_P_Dance;

        [Header("Monster")]
        public FMODUnity.EventReference SFX_M_Crash;
        // public FMODUnity.EventReference SFX_M_Cry;
        // public FMODUnity.EventReference SFX_M_Rush;
        public FMODUnity.EventReference SFX_M_Sleep;
        // public FMODUnity.EventReference SFX_M_Grow;
        public FMODUnity.EventReference SFX_M_POP;

        [Header("Object")]
        public FMODUnity.EventReference SFX_OB_Tree_Fall;
        public FMODUnity.EventReference SFX_OB_Tree_Flight;
        // public FMODUnity.EventReference SFX_OB_Tree_Drop;
        // public FMODUnity.EventReference SFX_OB_Zamiwa;
        public FMODUnity.EventReference SFX_OB_Water;
        public FMODUnity.EventReference SFX_OB_Waterfall;
        public FMODUnity.EventReference SFX_OB_SavePoint;
        public FMODUnity.EventReference SFX_OB_Pad;
        public FMODUnity.EventReference SFX_OB_Pad2;
        public FMODUnity.EventReference SFX_OB_RockMove;
        public FMODUnity.EventReference SFX_OB_RockPile;

        [Header("UI")]
        public FMODUnity.EventReference SFX_UI_Helper;
        public FMODUnity.EventReference SFX_UI_PressKey;
        public FMODUnity.EventReference SFX_UI_Click;
        public FMODUnity.EventReference SFX_UI_Select;

        [Header("AMB")]
        public FMODUnity.EventReference AMB_Forest_1;
        public FMODUnity.EventReference AMB_Forest_2;
        // public FMODUnity.EventReference AMB_Wind;
        // public FMODUnity.EventReference AMB_Zamiwa;

        [Header("BGM")]
        public FMODUnity.EventReference BGM_Full;
        public FMODUnity.EventReference BGM_Hightlight;
        public FMODUnity.EventReference BGM_Nonhighlight;

        [Header("CutScene")]
        // public FMODUnity.EventReference CS_OP;
        // public FMODUnity.EventReference CS_ED;

        public FMOD.Studio.EventInstance TitleState;
        public FMOD.Studio.EventInstance ForestOneState;
        public FMOD.Studio.EventInstance ForestTwoState;

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

        private void Start()
        {
            TitleState = FMODUnity.RuntimeManager.CreateInstance(BGM_Nonhighlight);
            TitleState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
            TitleState.start();

            ForestOneState = FMODUnity.RuntimeManager.CreateInstance(AMB_Forest_1);
            ForestOneState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));

            ForestTwoState = FMODUnity.RuntimeManager.CreateInstance(AMB_Forest_2);
            ForestTwoState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
        }

        private void Update()
        {
            if (_bgmOff)
            {
                if (TitleState.isValid())
                {
                    TitleState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    TitleState.release();
                    TitleState.clearHandle();
                }

                if (ForestTwoState.isValid())
                {
                    ForestTwoState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    ForestTwoState.release();
                    ForestTwoState.clearHandle();
                }

                if (ForestTwoState.isValid())
                {
                    ForestTwoState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    ForestTwoState.release();
                    ForestTwoState.clearHandle();
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

        public void SoundOneShot3DAttributes(EventReference audioEvent, Transform transform)
        {
            FMOD.Studio.EventInstance temp = FMODUnity.RuntimeManager.CreateInstance(audioEvent);
            temp.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
            temp.start();
            temp.release();
        }

        public void SoundOneShot3DOverrideDistance(EventReference audioEvent, Transform transform, float min, float max)
        {
            FMOD.Studio.EventInstance temp = FMODUnity.RuntimeManager.CreateInstance(audioEvent);
            temp.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
            temp.setProperty(FMOD.Studio.EVENT_PROPERTY.MINIMUM_DISTANCE, min);
            temp.setProperty(FMOD.Studio.EVENT_PROPERTY.MAXIMUM_DISTANCE, max);
            temp.start();
            temp.release();
        }

        public void StartInstance(EventInstance inst, EventReference audioEvent)
        {
            inst = FMODUnity.RuntimeManager.CreateInstance(audioEvent);
            inst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
            inst.start();
        }

        public void StopEventInstance(EventInstance inst)
        {
            inst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            inst.release();
            inst.clearHandle();           
        }

        public void StopAllSoundEvents()
        {
            FMOD.Studio.Bus playerBus = FMODUnity.RuntimeManager.GetBus("bus:/");
            playerBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        public bool IsPlaying(EventInstance inst)
        {
            FMOD.Studio.PLAYBACK_STATE state;
            inst.getPlaybackState(out state);
            return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
        }
    }
}
