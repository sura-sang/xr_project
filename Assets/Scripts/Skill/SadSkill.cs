using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class SadSkill : MonoBehaviour
    {
        [SerializeField] LayerMask _tearBlock;

        [SerializeField] float _speed;
        [SerializeField] float _gravity;
        [SerializeField] float _downGravity;

        [SerializeField] int _sampleCount;
        [SerializeField] int _skipCount;

        [SerializeField] float _accuracy;


        [SerializeField] Transform[] _eyes;
        [SerializeField] LineRenderer[] _lineRenderers;


        public void OnSkill(bool isOn)
        {
            if (isOn)
            {
                for (int i = 0; i < _eyes.Length && i < _lineRenderers.Length; i++)
                {
                    SetTearLine(_eyes[i], _lineRenderers[i]);
                }
            }
            else
            {
                SkillEnd();
            }
        }

        public void SkillEnd()
        {
            for (int i = 0; i < _lineRenderers.Length; i++)
            {
                _lineRenderers[i].positionCount = 0;
            }
        }

        private List<Vector3> GetTearsPositions(Transform eye)
        {
            List<Vector3> positions = new List<Vector3>();
            positions.Add(eye.position);

            var eyeDir = eye.forward * _speed;
            var time = 0.1f;

            for (int i = 1; i < _sampleCount; i++)
            {
                var pos = (eyeDir + (Vector3.down * _gravity * time)) * time;

                time += _accuracy;
                positions.Add(eye.position + pos);
            }

            return positions;
        }

        private (int lastIndex, RaycastHit hit) GetTearHitPoints(List<Vector3> tears, int skip)
        {
            var lastIndex = tears.Count - 1;
            var hit = new RaycastHit();

            for (int i = skip; i < tears.Count; i += skip)
            {
                var dir = tears[i] - tears[i - skip];
                var distance = dir.magnitude;

                if (Physics.Raycast(tears[i - skip], dir, out hit, distance, _tearBlock))
                {
                    return (i, hit);
                }
            }

            return (lastIndex, hit);
        }

        private void SetTearLine(Transform eye, LineRenderer line)
        {
            var positions = GetTearsPositions(eye);
            (var lastIndex, var hitPoint) = GetTearHitPoints(positions, 1);

            if (lastIndex != positions.Count - 1)
            {
                lastIndex++;

                positions.RemoveRange(lastIndex, positions.Count - lastIndex);
                positions[^1] = hitPoint.point;
            }

            line.positionCount = lastIndex;
            line.SetPositions(positions.ToArray());
        }

        private void OnDrawGizmos()
        {

        }

    }
}
