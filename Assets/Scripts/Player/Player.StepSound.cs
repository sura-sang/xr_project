using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace SuraSang
{
    public partial class Player
    {
        private enum CURRENT_TERRAIN { GRASS, WOODEN, STONE, WATER };

        [SerializeField]
        private CURRENT_TERRAIN _currentTerrain;

        private FMOD.Studio.EventInstance footstep;

        private void DetermineTerrain()
        {
            RaycastHit[] hit;

            hit = Physics.RaycastAll(transform.position, Vector3.down, 10.0f);

            foreach (RaycastHit rayhit in hit)
            {
                if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Material_Grass"))
                {
                    _currentTerrain = CURRENT_TERRAIN.GRASS;
                }
                else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Material_Wooden"))
                {
                    _currentTerrain = CURRENT_TERRAIN.WOODEN;
                }
                else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Material_Stone"))
                {
                    _currentTerrain = CURRENT_TERRAIN.STONE;
                }
                else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Material_Water"))
                {
                    _currentTerrain = CURRENT_TERRAIN.WATER;
                }
            }
        }

        private void PlayFootStep(int terrain)
        {
            footstep = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/PC/Common/SFX_P_Footstep");
            footstep.setParameterByName("Material", terrain);
            footstep.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            footstep.start();
            footstep.release();
        }

        public void DefaultPlayFootStep()
        {
            if (CurrentEmotion == Emotion.Default)
            {
                Debug.Log("디폴트 스텝");
                switch (_currentTerrain)
                {
                    case CURRENT_TERRAIN.GRASS:
                        PlayFootStep(0);
                        break;
                    case CURRENT_TERRAIN.WOODEN:
                        PlayFootStep(1);
                        break;
                    case CURRENT_TERRAIN.STONE:
                        PlayFootStep(2);
                        break;
                    case CURRENT_TERRAIN.WATER:
                        PlayFootStep(3);
                        break;
                }
            }
        }

        public void EmotionPlayFootStep()
        {
            if (CurrentEmotion != Emotion.Default)
            {
                Debug.Log("감정 스텝");
                switch (_currentTerrain)
                {
                    case CURRENT_TERRAIN.GRASS:
                        PlayFootStep(0);
                        break;
                    case CURRENT_TERRAIN.WOODEN:
                        PlayFootStep(1);
                        break;
                    case CURRENT_TERRAIN.STONE:
                        PlayFootStep(2);
                        break;
                    case CURRENT_TERRAIN.WATER:
                        PlayFootStep(3);
                        break;
                }
            }
        }
    }
}
