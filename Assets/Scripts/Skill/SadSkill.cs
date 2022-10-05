using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class SadSkill : ISkill
    {
        private Player _player;
        private CharacterController _controller;

        public SadSkill(Player player, CharacterController controller)
        {
            _player = player;
            _controller = controller;
        }

        public void OnMove(Vector2 input)
        {
            var dir = _player.InputToCameraSpace(input);

            _player.SmoothRotation(dir);

            dir *= _player.Speed;
            dir.y = _controller.isGrounded ? -1 : _player.MoveDir.y - _player.Gravity * Time.deltaTime;
            _player.MoveDir = dir;
            
            foreach (var eye in _player.SadEyes)
            {
                eye.SetTearLine();
            }
        }

        public void UpdateSkill() { }

        public void InitializeSkill()
        {
            foreach (var eye in _player.SadEyes)
            {
                eye.ResetTears();
            }
        }

        public void ClearSkill()
        {
            foreach (var eye in _player.SadEyes)
            {
                eye.ResetTears();
            }
        }
    }
}