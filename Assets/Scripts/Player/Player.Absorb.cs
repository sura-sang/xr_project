using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

namespace SuraSang
{
    public partial class Player
    {
        public Emotion CurrentEmotion;

        [Header("View Config")]
        [SerializeField] private bool _debugMode = false;
        [Range(0f, 360f)]
        [SerializeField] private float _ViewAngle = 0f; // 시야각
        [SerializeField] private float _viewRadius = 1f; //시야 범위
        [SerializeField] private LayerMask _viewTargetMask; //인식 가능한 타켓의 레이어
        [SerializeField] private LayerMask _viewObstacleMask; //인식 방해물의 레이어 

        private List<Collider> _hitTargetContainer = new List<Collider>();//인식한 물체 보관하는 리스트

        private float _horizontalViewHalfAngle = 0f; //시야각의 절반 값
        private bool _isFind;
        private bool _isAbsorb;
        private float _timer;

        [SerializeField] private Renderer _renderer;
        private Material _defaultMaterial;

        private void InitAbsorb()
        {
            _defaultMaterial = _renderer.material;
        }

        private void UpdateAbsorb()
        {
            if (_isAbsorb)
            {
                _timer += Time.deltaTime;

                if (_timer >= 3f)
                {
                    // ReturnEmotion();
                }
            }
        }

        public void SetAbsorbAction()
        {
            SetAction(ButtonActions.Absorb, OnAbsorb);
        }
        
        private Collider[] FindViewTargets()
        {
            _hitTargetContainer.Clear();

            var originPos = transform.position;

            Collider[] hitedTargets = Physics.OverlapSphere(originPos, _viewRadius, _viewTargetMask);

            foreach (Collider target in hitedTargets)
            {
                var targetPos = target.transform.position;
                var dist = targetPos - transform.position;

                if (dist.magnitude <= _viewRadius)
                {
                    var dot = Vector3.Dot(dist.normalized, transform.forward);
                    var theta = Mathf.Acos(dot);
                    var degree = Mathf.Rad2Deg * theta;

                    if (degree <= _ViewAngle / 2f)
                    {
                        _isFind = true;
                        _hitTargetContainer.Add(target);
                    }
                    else
                    {
                        _isFind = false;
                        continue;
                    }
                }
            }

            if (_hitTargetContainer.Count > 0)
                return _hitTargetContainer.ToArray();
            else
                return null;
        }

        private void OnAbsorb(bool isOn)
        {
            FindViewTargets();

            if (isOn && _hitTargetContainer.Count != 0)
            {
                for (int i = 0; i < _hitTargetContainer.Count; i++)
                {
                    _isAbsorb = true;
                    _timer = 0;
                    CurrentEmotion = _hitTargetContainer[i].gameObject.GetComponent<Monster>().Emotion;
                    _renderer.material = _hitTargetContainer[i].GetComponent<Renderer>().material;
                }
            }
        }

        private void ReturnEmotion()
        {
            _renderer.material = _defaultMaterial;
            CurrentEmotion = Emotion.Default;
            _timer = 0;
            _isAbsorb = false;
        }
    }
}
