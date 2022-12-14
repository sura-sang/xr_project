using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace SuraSang
{
    public class TitleScene : MonoBehaviour
    {
        private int _sequence = 0;

        private AsyncOperation _operation;
        private Animator _animator;

        private VideoPlayer _videoPlayer;

        private AsyncOperation _asyncOperation;

        [SerializeField] private GameObject _credit;
        [SerializeField] private float _introDelay;

        private void Awake()
        {
            _sequence = 0;
            _animator = GetComponent<Animator>();
            _videoPlayer = GetComponent<VideoPlayer>();

            _videoPlayer.Prepare();
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                switch (_sequence++)
                {
                    case 0:
                        AudioManager.Instance.SoundOneShot2D(AudioManager.Instance.SFX_UI_PressKey);
                        _animator.SetTrigger("PressKey");
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
            _animator.SetTrigger("PressKey");

            AudioManager.Instance.TitleState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            AudioManager.Instance.SoundOneShot2D(AudioManager.Instance.SFX_UI_Click);
;
            StartCoroutine(LoadSequence((float)_videoPlayer.length));
        }

        private IEnumerator LoadSequence(float waitTime)
        {
            _videoPlayer.Prepare();

            yield return new WaitForSeconds(_introDelay);

            _videoPlayer.enabled = true;
            _videoPlayer.Play();

            _asyncOperation = SceneManager.LoadSceneAsync(1);
            _asyncOperation.allowSceneActivation = false;

            yield return new WaitForSeconds(waitTime);

            _asyncOperation.allowSceneActivation = true;
        }

        public void SkipIntro()
        {
            StopAllCoroutines();

            if (_asyncOperation != null)
            {
                _asyncOperation.allowSceneActivation = true;
            }
            else
            {
                SceneManager.LoadSceneAsync(1);
            }
        }



        public void QuitGame()
        {
            AudioManager.Instance.SoundOneShot2D(AudioManager.Instance.SFX_UI_Click);
            AudioManager.Instance.StopAllSoundEvents();
            Application.Quit();
        }

        public void OpenCredit()
        {
            AudioManager.Instance.TitleState.setParameterByName("EQ", 1);
            AudioManager.Instance.SoundOneShot2D(AudioManager.Instance.SFX_UI_Click);
            _credit.SetActive(true);
            _animator.SetTrigger("Credit");
        }

        public void CloseCredit()
        {
            AudioManager.Instance.TitleState.setParameterByName("EQ", 0);
            _credit.SetActive(false);
        }

        public void BackTitle()
        {
            _animator.SetTrigger("BackTitle");
        }
    }
}
