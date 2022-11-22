using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SuraSang
{
    public class TitleScene : MonoBehaviour
    {
        [SerializeField] private GameObject _titleObject;
        [SerializeField] private GameObject _menuObject;

        private int _sequence = 0;

        private AsyncOperation _operation;

        private void Awake()
        {
            _sequence = 0;
            _titleObject.SetActive(true);
            _menuObject.SetActive(false);

            AudioManager.Instance.PlayBGM(AudioManager.Instance.BGM_Nonhighlight);
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                switch (_sequence++)
                {
                    case 0:
                        AudioManager.Instance.SoundOneShot2D(AudioManager.Instance.SFX_UI_PressKey);
                        _titleObject.SetActive(false);
                        _menuObject.SetActive(true);
                        break;
                }
            }
        }

        public void PlaySelectSound()
        {
            AudioManager.Instance.SoundOneShot2D(AudioManager.Instance.SFX_UI_Select);
        }

        public void StartGame()
        {
            AudioManager.Instance.SoundOneShot2D(AudioManager.Instance.SFX_UI_Click);
            SceneManager.LoadSceneAsync(1);
        }

        public void QuitGame()
        {
            AudioManager.Instance.SoundOneShot2D(AudioManager.Instance.SFX_UI_Click);
            Application.Quit();
        }

        public void OpenCredit()
        {
            AudioManager.Instance.SoundOneShot2D(AudioManager.Instance.SFX_UI_Click);
            Debug.Log("열려라 크레딧");
        }
    }
}
