using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace SuraSang
{
    public class PuzzleClown : PuzzleElements
    {
        public float Size = 5f;
        public float GrowSpeed = 1f;
        private float _time;
        [SerializeField]
        private Transform _hat;
        [SerializeField]
        private PuzzleTrampolin _trampolin;

        private void Start()
        {
            PuzzleManager.Instance.AddObserver(this);
            _trampolin.Enable = false;
        }

        public override void OnNotify(PuzzleContext context)
        {
            if (context.SkillEmotion == Emotion.Sadness && !IsNotify)
            {
                IsNotify = true;
                _hat.transform.DOScale(new Vector3(Size, Size, Size), GrowSpeed);
                Debug.Log("광대 버섯 퍼즐 실행");

                _trampolin.Enable = true;
            }
        }
    }
}
