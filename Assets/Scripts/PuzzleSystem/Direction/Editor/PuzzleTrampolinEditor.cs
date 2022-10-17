using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SuraSang
{
    [CustomEditor(typeof(PuzzleTrampolin))]
    public class ExampleInspector : Editor
    {
        private const int _dirCount = 6;

        private const float _debugLineLength = 50;
        private const float _delta = 0.01f;

        private const float _handleSize = 3;

        private PuzzleTrampolin _component;


        void OnSceneGUI()
        {
            Vector3 GetVector(float radians)
            {
                return new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));
            }

            _component = target as PuzzleTrampolin;

            // 거리
            Handles.color = Color.red;

            var handlePos = _component.JumpPoint.position +
                            GetVector(_component.StartAngle * Mathf.Deg2Rad) * _handleSize;

            _component.JumpVelocity = Handles.ScaleValueHandle(_component.JumpVelocity, handlePos,
                Quaternion.Euler(0, -_component.StartAngle + 90, 0), 3, Handles.ConeHandleCap, 1);


            // 높이 (그냥 표시용)
            var gravity = _component._playerData.Gravity * _component._playerData.FallingGravityMultiplier;
            
            for (int i = 0; i < _component.DebugHeight.Length; i++)
            {
                Random.InitState(i);
                Handles.color = new Color(Random.Range(0.25f, 0.75f), Random.Range(0.25f, 0.75f), Random.Range(0.25f, 0.75f), 1);
                
                handlePos = _component.JumpPoint.position + Vector3.up * _component.DebugHeight[i];

                Handles.Label(handlePos, $"높이 {i} : {_component.DebugHeight[i]}");
                
                _component.DebugHeight[i] = Handles.ScaleValueHandle(_component.DebugHeight[i], handlePos,
                    Quaternion.Euler(-90, 0, 0), 3, Handles.SphereHandleCap, 1);

                
                float vel = Mathf.Sqrt(gravity * 2 * _component.DebugHeight[i]) * _component.PowerReduction;

                var plusAngle = 360f / _dirCount;

                for (int j = 0; j < _dirCount; j++)
                {
                    var dir = GetVector((_component.StartAngle + j * plusAngle) * Mathf.Deg2Rad).normalized * 3;
                    DrawArc(dir, vel);
                }
                
            }
        }

        private void DrawArc(Vector3 dir, float jumpPower)
        {
            var velocity = dir * _component.JumpVelocity;
            velocity.y = jumpPower;

            var pos = _component.JumpPoint.position;
            
            for (int i = 1; i < _debugLineLength + 1; i++)
            {
                velocity.y -= (_component._playerData.Gravity *
                               (velocity.y < 0 ? _component._playerData.FallingGravityMultiplier : 1)) * _delta;
                velocity.y = Mathf.Max(velocity.y, -_component._playerData.GravityLimit);

                Handles.DrawLine(pos, pos + velocity * _delta, 3);
                pos += velocity * _delta;
            }
        }
    }
}