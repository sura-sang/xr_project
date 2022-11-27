using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace SuraSang
{
    public class AudioTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask _interactionLayer;

        enum Type { TITLE, FOREST1, FOREST2, WATER };
        [SerializeField] private Type _type;

        private int _switchCount = 0;

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _interactionLayer) != 0)
            {
                switch (_type)
                {
                    case Type.TITLE:
                        AudioManager.Instance.TitleState.start();
                        break;
                    case Type.FOREST1:
                        if (!AudioManager.Instance.IsPlaying(AudioManager.Instance.StageState))
                        {
                            AudioManager.Instance.StageState = FMODUnity.RuntimeManager.CreateInstance(AudioManager.Instance.AMB_Forest_1);
                            AudioManager.Instance.StageState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
                            AudioManager.Instance.StageState.start();

                        }
                        break;
                    case Type.FOREST2:
                        if (AudioManager.Instance.IsPlaying(AudioManager.Instance.StageState) && _switchCount == 0)
                        {
                            AudioManager.Instance.StageState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                            AudioManager.Instance.StageState = FMODUnity.RuntimeManager.CreateInstance(AudioManager.Instance.AMB_Forest_2);
                            AudioManager.Instance.StageState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
                            AudioManager.Instance.StageState.start();
                            _switchCount++;
                        }
                        break;
                    case Type.WATER:
                        Global.Instance.SceneMaster.Player.PlayerInWater = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & _interactionLayer) != 0)
            {
                switch (_type)
                {
                    case Type.WATER:
                        Global.Instance.SceneMaster.Player.PlayerInWater = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
