using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace SuraSang
{
    public class AudioTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask _interactionLayer;

        enum Type { TITLE, FOREST1, FOREST2, WATER };
        [SerializeField] private Type _type;

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
                        AudioManager.Instance.ForestOneState.start();
                        break;
                    case Type.FOREST2:
                        AudioManager.Instance.ForestTwoState.start();
                        break;
                    case Type.WATER:
                        Global.Instance.SceneMaster.Player.PlayerInWater = true;
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
                    case Type.TITLE:
                        AudioManager.Instance.TitleState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                        break;
                    case Type.FOREST1:
                        AudioManager.Instance.ForestOneState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                        break;
                    case Type.FOREST2:
                        AudioManager.Instance.ForestTwoState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                        break;
                    case Type.WATER:
                        Global.Instance.SceneMaster.Player.PlayerInWater = false;
                        break;
                }
            }
        }
    }
}
