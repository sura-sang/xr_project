using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class ObserverFresh : PuzzleObserver
    {
        public float Size = 3f;
        public float GrowSpeed = 1f;
        private float _time;
        private Vector3 _originScale;

        private void Start()
        {
            _originScale = transform.localScale;
            PuzzleManager.Instance.AddObserver(this);
        }

        public override void OnNotify()
        {
            StartCoroutine(GrowUp());
            Debug.Log("갓버섯 퍼즐 실행");
        }

        IEnumerator GrowUp()
        {
            while (transform.localScale.x < Size)
            {
                transform.localScale = _originScale * (1f + _time * GrowSpeed);
                _time += Time.deltaTime;

                if (transform.localScale.x >= Size)
                {
                    _time = 0;
                    break;
                }
                yield return null;
            }
        }
    }
}
