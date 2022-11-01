using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SuraSang
{
    public class PuzzleFresh : PuzzleElements
    {
        public float Size = 3f;
        public float GrowSpeed = 1f;
        private float _time; 
        public BoxCollider Platform;

        [SerializeField]
        private Transform _body;

        private void Start()
        {
            PuzzleManager.Instance.AddObserver(this);
            Platform.enabled = false;
        }

        public override void OnNotify(PuzzleContext context)
        {
            if (context.SkillEmotion == Emotion.Sadness && !IsNotify)
            {
                Platform.enabled = true;
                IsNotify = true;
                _body.transform.DOMoveY(Size, GrowSpeed, false);
                Debug.Log("갓버섯 퍼즐 실행");
            }
        }
    }
}
