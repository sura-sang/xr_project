using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class ObserverClown : PuzzleObserver
    {
        public float Size = 5f;
        public float GrowSpeed = 1f;
        private float _time;
        public Vector3 _upSize;
        private Vector3 _originScale;

        private void Start()
        {
            _originScale = transform.Find("Cap").localScale;
            PuzzleManager.Instance.AddObserver(this);
        }

        public override void OnNotify()
        {
            StartCoroutine(GrowUp());
            Debug.Log("광대 버섯 퍼즐 실행");
        }

        IEnumerator GrowUp()
        {
            while (transform.Find("Cap").localScale.x < Size)
            {
                transform.Find("Cap").localScale = _upSize * (1f + _time * GrowSpeed);

                _time += Time.deltaTime;

                if (transform.Find("Cap").localScale.x >= Size)
                {
                    _time = 0;
                    break;
                }
                yield return null;
            }
        }
    }
}
