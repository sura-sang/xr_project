using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class PuzzleFresh : PuzzleElements
    {
        public float Size = 3f;
        public float GrowSpeed = 1f;
        private float _time; 
        public Vector3 _upSize;

        private void Start()
        {
            PuzzleManager.Instance.AddObserver(this);
        }

        public override void OnNotify(PuzzleContext context)
        {
            if (context.SkillEmotion == Emotion.Sadness && !IsNotify)
            {
                IsNotify = true;
                StartCoroutine(GrowUp());
                Debug.Log("갓버섯 퍼즐 실행");
            }
        }
     
        IEnumerator GrowUp()
        {
            while (transform.Find("Body").localScale.y < Size)
            {
                var pos = transform.Find("Cap").localPosition;

                transform.Find("Body").localScale += _upSize * (_time * GrowSpeed);
                pos.y = transform.Find("Body").localScale.y;
                transform.Find("Cap").localPosition = pos;

                _time += Time.deltaTime;

                if (transform.Find("Body").localScale.y >= Size)
                {
                    _time = 0;
                    break;
                }
                yield return null;
            }
        }
    }
}
