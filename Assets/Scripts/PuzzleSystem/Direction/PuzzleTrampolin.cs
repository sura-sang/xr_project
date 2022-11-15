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

        public Transform JumpPoint => _jumpPoint;
        [SerializeField] private Transform _jumpPoint;

        public float Cooltime;
        public float MinHeight;

        public float JumpVelocity; // 횡으로 이동하는 속도
        public float PowerReduction; // Y속도 줄이기

        private float _minVelocity;
        private float _lastJumpTime;

        [SerializeField]
        private Monster _monster;

        private void Awake()
        {
            var data = Global.Instance.SODataManager.GetData<PlayerData>();
            var gravity = data.Gravity * data.FallingGravityMultiplier;
            _minVelocity = Mathf.Sqrt(gravity * 2 * MinHeight);
            _monster = GetComponentInParent<Monster>();
        }

        public override void OnNotify(PuzzleContext context)
        {
            base.OnNotify(context);
            
            if (_context == null)
            {
                return;
            }

            if (!(_context.Character is Player))
            {
                return; // 몬스터도 트램펄린 띄우고 싶다...
            }

            if (Time.time - _lastJumpTime < Cooltime)
            {
                return;
            }

            if(!_monster.IsSleep)
            {
                return;
            }
            
            if(_context.SkillEmotion == Emotion.Anger && _context.Player.IsSkill)
            {
                return;
            }

#if UNITY_EDITOR
            // 에디터에서 MinHeight를 계속 수정하는 경우를 반영
            var data = Global.Instance.SODataManager.GetData<PlayerData>();
            var gravity = data.Gravity * data.FallingGravityMultiplier;
            _minVelocity = Mathf.Sqrt(gravity * 2 * MinHeight);
#endif

            _lastJumpTime = Time.time;

            var angle = GetNearestAngle(GetAngle(_context.Dir));
            var jumpDir = GetVector(angle * Mathf.Deg2Rad) * JumpVelocity;

            jumpDir.y = Mathf.Max(_minVelocity, -_context.Dir.y) * PowerReduction;

            var character = _context.Character;
            character.ChangeState(new PlayerMoveJumping(character, jumpDir));

            var pos = character.transform.position;
            pos.y = JumpPoint.position.y;
            character.MovePosition(pos);
        }

        private void OnDrawGizmos() { }// override용

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.TryGetComponent<Player>(out var player))
            {
                OnNotify(new PuzzleContextDirection(player.MoveDir, player, player.CurrentEmotion, player));
            }

        }
    }
}
