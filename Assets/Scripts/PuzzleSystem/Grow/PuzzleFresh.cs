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
        public BoxCollider platform;

        [SerializeField]
        private Transform _body;

        private void Start()
        {
            PuzzleManager.Instance.AddObserver(this);
            platform.enabled = false;
        }

        public override void OnNotify(PuzzleContext context)
        {
            if (context.SkillEmotion == Emotion.Sadness && !IsNotify)
            {
                platform.enabled = true;
                IsNotify = true;
                _body.transform.DOMoveY(Size, GrowSpeed, false);
                Debug.Log("갓버섯 퍼즐 실행");
            }
        }
    }
}
