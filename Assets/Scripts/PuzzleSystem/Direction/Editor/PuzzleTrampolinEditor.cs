using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SuraSang
{
    [CustomEditor(typeof(PuzzleTrampolin))]
    public class ExampleInspector : Editor
    {
        private const float _debugLineLength = 50;
        private const float _delta = 0.01f;

        private const float _handleSize = 3;

        private PuzzleTrampolin _component;

        private PlayerData _playerData;

        private void Awake()
        {
            _playerData = Global.Instance.SODataManager.GetData<PlayerData>();
        }

        private void OnSceneGUI()
        {

            EditorGUI.BeginChangeCheck();

            _component = target as PuzzleTrampolin;

            // 거리
            Handles.color = Color.red;

            var handlePos = _component.JumpPoint.position +
                            GetVector(_component.StartAngle * Mathf.Deg2Rad) * _handleSize;

            _component.JumpVelocity = Handles.ScaleValueHandle(_component.JumpVelocity, handlePos,
                Quaternion.Euler(0, -_component.StartAngle + 90, 0), 3, Handles.ConeHandleCap, 1);

            //최소 높이
            Handles.color = Color.white;

            handlePos = _component.JumpPoint.position + Vector3.up * _component.MinHeight;

            _component.MinHeight = Handles.ScaleValueHandle(_component.MinHeight, handlePos,
                Quaternion.Euler(90, 0, 0), 3, Handles.ConeHandleCap, 1);

            DrawArcs(_component.MinHeight);

            // 높이 (그냥 표시용)
            var gravity = _playerData.Gravity * _playerData.FallingGravityMultiplier;

            for (int i = 0; i < _component.DebugHeight.Length; i++)
            {
                Handles.color = Color.HSVToRGB((float)i / _component.DebugHeight.Length, 1, 1);

                handlePos = _component.JumpPoint.position + Vector3.up * _component.DebugHeight[i];

                Handles.Label(handlePos, $"높이 {i} : {_component.DebugHeight[i]}");

                _component.DebugHeight[i] = Handles.ScaleValueHandle(_component.DebugHeight[i], handlePos,
                    Quaternion.Euler(-90, 0, 0), 3, Handles.SphereHandleCap, 1);

                DrawArcs(_component.DebugHeight[i]);
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
                Undo.RecordObject(target, "Trampolin");
            }
        }

        private Vector3 GetVector(float radians)
        {
            return new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));
        }

        private void DrawArcs(float height)
        {
            var gravity = _playerData.Gravity * _playerData.FallingGravityMultiplier;
            float vel = Mathf.Sqrt(gravity * 2 * height) * _component.PowerReduction;

            var plusAngle = 360f / _component.DirCount;

            for (int j = 0; j < _component.DirCount; j++)
            {
                var dir = GetVector((_component.StartAngle + j * plusAngle) * Mathf.Deg2Rad).normalized;
                var velocity = dir * _component.JumpVelocity;
                velocity.y = vel;

                var pos = _component.JumpPoint.position;

                for (int i = 1; i < _debugLineLength + 1; i++)
                {
                    var y = velocity.y;

                    y -= (_playerData.Gravity *
                                   (y < 0 ? _playerData.FallingGravityMultiplier : 1)) * _delta;
                    y = Mathf.Max(y, -_playerData.GravityLimit);

                    velocity.y = 0;
                    velocity = Vector3.MoveTowards(velocity, Vector3.zero, _playerData.AirControl * _delta);

                    velocity.y = y;

                    Handles.DrawDottedLine(pos, pos + velocity * _delta, 3);
                    pos += velocity * _delta;
                }
            }
        }
    }
}