using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class PuzzleOyster : PuzzleElements
    {
        public float Size = 3f;
        public float GrowSpeed = 1f;
        private float _time;
        private Vector3 _originScale;
        private Monster _monster;
        private Anger _anger;
        public CapsuleCollider CapsuleCol;

        private void Start()
        {
            _monster = GetComponent<Monster>();
            _originScale = transform.localScale;
            PuzzleManager.Instance.AddObserver(this);
        }

        public override void OnNotify(PuzzleContext context)
        {
            if (context.SkillEmotion == Emotion.Sadness && !IsNotify)
            {
                IsNotify = true;
                StartCoroutine(GrowUp());
                Debug.Log("새송이 버섯 퍼즐 실행");
            }
        }
        

        private void Update()
        {
            if (_monster.IsSleep)
                CapsuleCol.enabled = false;
        }


        IEnumerator GrowUp()
        {
            while(transform.localScale.x < Size)
            {
                transform.localScale = _originScale * (1f + _time * GrowSpeed);
                _time += Time.deltaTime;

                if(transform.localScale.x>=Size)
                {
                    _time = 0;
                    break;
                }
                yield return null;
            }        
        }
    }
}
