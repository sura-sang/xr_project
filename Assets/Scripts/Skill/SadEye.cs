using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SuraSang
{
    public class SadEye : MonoBehaviour
    {
        [SerializeField] private float _tearJiggleSpeed;
        [SerializeField] private AnimationCurve _tearAnimation;
        [SerializeField] private LayerMask _tearBlock;

        [SerializeField] private float _speed;
        [SerializeField] private float _gravity;
        [SerializeField] private int _sampleCount;
        [SerializeField] private float _accuracy;

        private Player _player;
        private LineRenderer _lineRenderer;

        private Vector3[] _curTearLines;

        private int _lastIndex;
        private RaycastHit _lastHit;

        private Vector3 _curLastPoint;

        private void Awake()
        {
            _player = GetComponentInParent<Player>();
            _lineRenderer = GetComponent<LineRenderer>();
            _curTearLines = new Vector3[_sampleCount];
        }

        public void ResetTears()
        {
            _curTearLines = GetTearsPositions().ToArray();
            _lineRenderer.positionCount = 0;
        }

        public void SetTearLine()
        {
            var targetPositions = GetTearsPositions();

            for (int i = 0; i < targetPositions.Count; i++)
            {
                var target = targetPositions[i];
                var current = _curTearLines[i];

                var newPos = Vector3.Lerp(Vector3.Lerp(current, target, _tearJiggleSpeed * Time.deltaTime),
                    target, _tearAnimation.Evaluate((float)i / _sampleCount));

                _curTearLines[i] = targetPositions[i] = newPos;
            }

            var lastIndex = _lastIndex;

            if (lastIndex != targetPositions.Count)
            {
                lastIndex++;
                targetPositions.RemoveRange(lastIndex, targetPositions.Count - lastIndex);
            }

            _lineRenderer.positionCount = lastIndex;
            _lineRenderer.SetPositions(targetPositions.ToArray());
        }

        private List<Vector3> GetTearsPositions()
        {
            List<Vector3> positions = new List<Vector3>();
            positions.Add(transform.position);

            var eyeDir = transform.forward * _speed;
            var time = _accuracy;

            for (int i = 1; i < _sampleCount; i++)
            {
                var pos = transform.position + (eyeDir + (Vector3.down * _gravity * time)) * time;

                time += _accuracy;
                positions.Add(pos);
            }

            return positions;
        }

        private void FixedUpdate()
        {
            _lastIndex = _curTearLines.Length;
            _lastHit = new RaycastHit();

            for (int i = 1; i < _curTearLines.Length; i++)
            {
                var dir = _curTearLines[i] - _curTearLines[i - 1];
                var distance = dir.magnitude;

                if (Physics.Raycast(_curTearLines[i - 1], dir, out _lastHit, distance, _tearBlock))
                {
                    _lastIndex = i;
                    return;
                }
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var point in GetTearsPositions())
            {
                Gizmos.DrawWireSphere(point, 0.1f);
            }
        }
    }
}