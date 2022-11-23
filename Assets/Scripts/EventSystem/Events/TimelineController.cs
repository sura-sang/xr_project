using System.Collections;
using System.Collections.Generic;
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

            _director.Pause();
            _director.playOnAwake = false;
            _camera.SetActive(false);
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
            _camera.SetActive(true);
            _director.Play(_timeline);
        }

        private void TimelineStop()
        {
            _director.time = 0;
            _director.Stop();
            _director.Evaluate();
            _camera.SetActive(false);

            if (_onlyOne)
            {
                Destroy(gameObject);
            }
        }

        void OnPlayableDirectorStopped(PlayableDirector aDirector)
        {
            if (_director == aDirector)
            {
                _director.time = 0;
                _director.Stop();
                _director.Evaluate();
                _camera.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
