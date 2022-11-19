using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SuraSang
{
    public class AudioUIPlayer : MonoBehaviour
    {
        public void SelectUI()
        {
            AudioManager.Instance.SoundOneShot2D(AudioManager.Instance.SFX_UI_Select);
        }

        public void ClickUI()
        {
            AudioManager.Instance.SoundOneShot2D(AudioManager.Instance.SFX_UI_Click);
        }

        public void Test_PressAnyKey()
        {
            AudioManager.Instance.SoundOneShot2D(AudioManager.Instance.SFX_UI_PressKey);
            AudioManager.Instance.GameStart();
        }

        public void Test_LevelCMS()
        {
            SceneManager.LoadScene("Level_CMS _Cleaning");
        }

        public void Test_EQ0()
        {
            AudioManager.Instance.PlayerState.setParameterByName("EQ", 0);
        }

        public void Test_EQ1()
        {
            AudioManager.Instance.PlayerState.setParameterByName("EQ", 1);
        }

        public void Test_Helper()
        {
            AudioManager.Instance.SoundOneShot2D(AudioManager.Instance.SFX_UI_Helper);
        }
    }
}
