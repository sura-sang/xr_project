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
        [SerializeField] private LayerMask _interactionLayer;

        [SerializeField] private bool _onlyOne;


        private void Start()
        {
            EventManager.Instance.TimelineStartAction += TimelineStart;
            EventManager.Instance.TimelineStopAction += TimelineStop;

            _director.playOnAwake = false;
        }

        private void OnDisable()
        {
            EventManager.Instance.TimelineStartAction -= TimelineStart;
            EventManager.Instance.TimelineStopAction -= TimelineStop;
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
            _director.Play(_timeline);
        }

        private void TimelineStop()
        {
            _director.time = 0;
            _director.Stop();
            _director.Evaluate();

            if (_onlyOne) Destroy(gameObject);
        }
    }
}
