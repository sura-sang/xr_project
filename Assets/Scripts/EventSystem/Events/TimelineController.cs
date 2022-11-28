using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace SuraSang
{
    public class TimelineController : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _director;
        [SerializeField] private TimelineAsset _timeline;
        [SerializeField] private GameObject _camera;
        [SerializeField] private LayerMask _interactionLayer;

        [SerializeField] private bool _onlyOne;
        [SerializeField] private bool _enddingDestroy;

        [SerializeField] private bool _dontMoveWhilePlaying;

        private void OnEnable()
        {
            if (_enddingDestroy)
            {
                _director.stopped += OnPlayableDirectorStopped;
            }
        }

        private void Start()
        {
            EventManager.Instance.TimelineStartAction += TimelineStart;
            EventManager.Instance.TimelineStopAction += TimelineStop;

            if (_director.playOnAwake)
            {
                if (_dontMoveWhilePlaying)
                {
                    Global.Instance.SceneMaster.Player.CanMove = false;
                    Global.Instance.UIManager.Get<UITimelineSkipPanelModel>().Init(TimelineStop);
                }
            }
            else
            {
                _director.Pause();
                _camera.SetActive(false);
            }
        }

        private void OnDisable()
        {
            EventManager.Instance.TimelineStartAction -= TimelineStart;
            EventManager.Instance.TimelineStopAction -= TimelineStop;

            if (_enddingDestroy)
            {
                _director.stopped -= OnPlayableDirectorStopped;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _interactionLayer) != 0)
            {
                TimelineStart();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & _interactionLayer) != 0)
            {
                TimelineStop();
            }
        }

        private void TimelineStart()
        {
            if (_camera != null)
            {
                _camera.SetActive(true);
            }

            _director.Play(_timeline);

            if (_dontMoveWhilePlaying)
            {
                Global.Instance.SceneMaster.Player.CanMove = false;
                Global.Instance.UIManager.Get<UITimelineSkipPanelModel>().Init(TimelineStop);
            }
        }

        private void TimelineStop()
        {
            _director.time = _director.playableAsset.duration;
            _director.Evaluate();
            _director.Stop();

            if (_camera != null)
            {
                _camera.SetActive(false);
            }

            if (_onlyOne)
            {
                Destroy(this.gameObject);
            }

            Global.Instance.UIManager.Get<UITimelineSkipPanelModel>().ReleaseUI();
            Global.Instance.SceneMaster.Player.CanMove = true;
        }

        void OnPlayableDirectorStopped(PlayableDirector aDirector)
        {
            if (_director == aDirector)
            {
                //_director.time = _director.playableAsset.duration;
                //_director.Stop();
                //_director.Evaluate();

                if (_camera != null)
                {
                    _camera.SetActive(false);
                }

                Destroy(gameObject);

                Global.Instance.UIManager.Get<UITimelineSkipPanelModel>().ReleaseUI();
                Global.Instance.SceneMaster.Player.CanMove = true;
            }
        }
    }
}
