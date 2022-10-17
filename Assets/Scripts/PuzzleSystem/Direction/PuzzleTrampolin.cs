using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SuraSang
{
    public class PuzzleTrampolin : PuzzleDirectionBase
    {
        [Header("높이 체크 (디버그용)")]
        public float[] DebugHeight; // 특정 높이에서 떨어졌을 때 얼마나 튀어오르는지 씬 GUI로 보여주기 위해
        public PlayerData _playerData;

        public Transform JumpPoint => _jumpPoint;
        [SerializeField] private Transform _jumpPoint;
        
        public float JumpVelocity; // 횡으로 이동하는 속도
        public float PowerReduction; // Y속도 줄이기

        public override void OnNotify(PuzzleContext context)
        {
            base.OnNotify(context);

            if (_context == null)
            {
                return;
            }

            var angle = GetNearestAngle(GetAngle(_context.Dir)) + 180;
            var vector = GetVector(angle * Mathf.Deg2Rad);
            
            
        }

        private void OnDrawGizmos() { }// override용
    }
}
