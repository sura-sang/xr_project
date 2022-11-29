using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace SuraSang
{
    public class EndingEvent : MonoBehaviour
    {
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private VideoClip _clip;
        [SerializeField] private GameObject _videoPlayer;
        [SerializeField] private GameObject _fadeImage;
        [SerializeField] private GameObject _canvas;
        [SerializeField] private GameObject _rawImage;

        private double _videoLen;
        private float _curTime;

        private bool _on;

        private void Start()
        {
            _videoLen = _clip.length;
        }

        private void Update()
        {
            if (_canvas.activeSelf == true)
            {
                if (_fadeImage.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Ending") &&
                    _fadeImage.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !_on)
                {
                    PlayEnding();
                    _rawImage.SetActive(true);
                    _videoPlayer.SetActive(true);
                }

                if (_on)
                {
                    _curTime += 1 * Time.deltaTime;

                    if (_curTime >= _videoLen)
                    {
                        _on = false;
                        AudioManager.Instance.TitleState.start();
                        ReturnTitle();
                    }
                }
            }

        }
        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _interactionLayer) != 0)
            {
                Global.Instance.SceneMaster.Player.CanMove = false;
                _canvas.SetActive(true);
            }
        }

        public void PlayEnding()
        {
            _on = true;
            SceneMaster.SceneInstance._replyCount = 0;
            SceneMaster.SceneInstance.CurrentCheckPoint = GameObject.Find("Start").GetComponent<CheckPoint>();
            AudioManager.Instance.TitleState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            AudioManager.Instance.StageState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            SceneManager.MoveGameObjectToScene(GameObject.Find("SceneMaster"), SceneManager.GetActiveScene());
            SceneManager.MoveGameObjectToScene(GameObject.Find("CheckPointManager"), SceneManager.GetActiveScene());

            Global.Instance.UIManager.Get<UITimelineSkipPanelModel>().Init(ReturnTitle);
        }

        public void ReturnTitle()
        {
            AudioManager.Instance.TitleState.start();
            SceneMaster.SceneInstance.LoadLevel(1);

            Global.Instance.UIManager.Get<UITimelineSkipPanelModel>().ReleaseUI();
        }
    }
}
