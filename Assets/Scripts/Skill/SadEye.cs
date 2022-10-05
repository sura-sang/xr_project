using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SuraSang
{
    public class SadEye : MonoBehaviour
    {
        public RaycastHit? LastHit => _lastHit;
        private RaycastHit? _lastHit = null;

        [SerializeField] private float _targetLength = 1.0f;

        [SerializeField] private float _smoothSpeed = 0.02f;
        [SerializeField] private LayerMask _tearBlock;

        [SerializeField] private float _speed = 0.3f;
        [SerializeField] private float _gravity = 0.125f;
        [SerializeField] private float _downGravity = 0.25f;
        [SerializeField] private int _sampleCount = 7;

        private LineRenderer _lineRenderer;

        private Vector3[] _curTearLines;
        private Vector3[] _curTearVelocity;

        private int _lastIndex;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _curTearLines = new Vector3[_sampleCount];
        }

        public void ResetTears()
        {
            _curTearLines = Enumerable.Repeat(transform.position, _sampleCount).ToArray();
            _curTearVelocity = new Vector3[_sampleCount];
            _lineRenderer.positionCount = 0;
        }

        public void SetTearLine()
        {
            _curTearLines[0] = transform.position;

            float curLength = 0;
            var power = transform.forward * _speed;

            for (int i = 1; i < _curTearLines.Length; i++)
            {
                _curTearLines[i] = Vector3.SmoothDamp(_curTearLines[i], _curTearLines[i - 1] + power,
                    ref _curTearVelocity[i], _smoothSpeed);

                power += Vector3.down * (power.y > 0 ? _gravity : _downGravity);

                curLength += (_curTearLines[i] - _curTearLines[i - 1]).sqrMagnitude;
            }

            _lineRenderer.positionCount = _lastIndex + (_lastIndex == _sampleCount ? 0 : 1);
            _lineRenderer.SetPositions(_curTearLines);

            var key = _lineRenderer.widthCurve[1];
            key.time = Mathf.Clamp01(curLength / (_targetLength * _targetLength));
            _lineRenderer.widthCurve.MoveKey(1, key);
        }

        private void FixedUpdate()
        {
            _lastIndex = _curTearLines.Length;
            _lastHit = null;

            for (int i = 1; i < _curTearLines.Length; i++)
            {
                var dir = _curTearLines[i] - _curTearLines[i - 1];
                var distance = dir.magnitude;

                if (Physics.Raycast(_curTearLines[i - 1], dir, out var hit, distance, _tearBlock))
                {
                    _lastHit = hit;
                    _lastIndex = i;
                    return;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 0.1f);

            Vector3 point = transform.position;

            var power = transform.forward * _speed;
            for (int i = 1; i < _sampleCount; i++)
            {
                point += power;

                Gizmos.DrawWireSphere(point, 0.1f);
                power += Vector3.down * (power.y > 0 ? _gravity : _downGravity);
            }
        }
    }
}