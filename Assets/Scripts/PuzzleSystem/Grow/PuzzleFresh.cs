using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class PuzzleFresh : PuzzleElements
    {
        public float Size = 3f;
        public float GrowSpeed = 1f;
        [SerializeField] private float _time = 0f;
        [SerializeField] private float _duration = 5f;
        private Animator _animator;
        private Sadness _sadness;
        public BoxCollider Platform;

        private void Start()
        {
            _sadness = GetComponent<Sadness>();
            _animator = GetComponent<Animator>();
            PuzzleManager.Instance.AddObserver(this);
            Platform.enabled = false;
        }

        public override void OnNotify(PuzzleContext context)
        {
            if (context.SkillEmotion == Emotion.Sadness && !IsNotify)
            {
                IsNotify = true;
                _animator.SetBool("Growth",true);
                Debug.Log("갓버섯 퍼즐 실행");
            }
        }

        private void Update()
        {
            if (_sadness.IsSleep)
                Platform.enabled = true;

            if(IsNotify && _time <= _duration)
            {
                _time += Time.deltaTime;
            }
            else if(_time >= _duration)
            {
                _animator.SetFloat("Speed", -1);
                _animator.SetBool("Growth", false);
                _animator.Play("Growth");

                _sadness.UnAbsorbed(_sadness);
            }
        }
    }
}
