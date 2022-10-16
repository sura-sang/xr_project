using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class SadSkill : ISkill
    {
        public bool IsStopAble => true;

        private Player _player;
        private CharacterController _controller;

        private bool _skillInUse = false;

        public SadSkill(Player player, CharacterController controller)
        {
            _player = player;
            _controller = controller;
        }

        public void OnMove(Vector2 input)
        {
            var dir = _player.InputToCameraSpace(input);

            _player.SmoothRotation(dir);

            dir *= _skillInUse ? 0 : _player.Speed;
            dir.y = _controller.isGrounded ? -1 : _player.MoveDir.y - _player.Gravity * Time.deltaTime;
            _player.MoveDir = dir;

            // 실행 순서 상 OnMove가 UpdateSkill보다 뒤 임
            foreach (var eye in _player.SadEyes)
            {
                eye.SetTearLine();
            }
        }

        public void UpdateSkill()
        {
            var hits = GetLastSadHits();

            foreach (var hit in hits)
            {
                hit.collider?.GetComponentInParent<PuzzleElements>()?.OnNotify(new PuzzleContext(_player.CurrentEmotion));
            }
        }

        public void InitializeSkill()
        {
            _skillInUse = true;

            foreach (var eye in _player.SadEyes)
            {
                eye.ResetTears();
            }
        }

        public void ClearSkill()
        {
            _skillInUse = false;

            foreach (var eye in _player.SadEyes)
            {
                eye.ResetTears();
            }
        }


        private List<RaycastHit> GetLastSadHits()
        {
            List<RaycastHit> raycastHits = new();

            foreach (var eye in _player.SadEyes)
            {
                if (eye.LastHit != null)
                {
                    raycastHits.Add(eye.LastHit.Value);
                }
            }

            return raycastHits;
        }
    }
}