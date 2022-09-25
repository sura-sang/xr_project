using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class PlayerMoveJumping : PlayerMoveState
    {
        public PlayerMoveJumping(CharacterMove characterMove) : base(characterMove) { }

        private bool _isJumpEnd = false;
        private bool _isHolding;
        private float _jumpStartTime;

        public override void InitializeState()
        {
            _jumpStartTime = Time.time;
            _player.OnMove = OnMove;

            _player.SetAction(ButtonActions.Hold, isOn => _isHolding = isOn);
            _player.SetAction(ButtonActions.Jump, OnJump);


            var dir = _player.MoveDir;
            dir.y = _player.JumpPower;
            _player.MoveDir = dir;
        }

        public override void UpdateState() { }

        public override void ClearState() { }

        private void OnJump(bool isOn)
        {
            if (!isOn)
            {
                _isJumpEnd = true;
            }
        }

        private void OnMove(Vector2 input)
        {
            var dir = _player.MoveDir;
            var y = dir.y;

            if (!_isJumpEnd && Time.time - _jumpStartTime < _player.VariableJumpTime)
            {
                y = _player.JumpPower;
            }
            else
            {
                y -= _player.Gravity * Time.deltaTime;
                y = Mathf.Max(y, -_player.GravityLimit);

                if (y < 0)
                {
                    _characterMove.ChangeState(new PlayerMoveFalling(_characterMove));
                }
            }
            
            if (_player.IsEdgeDetected() && _isHolding)
            {
                _characterMove.ChangeState(new PlayerMoveHolding(_characterMove));
            }

            dir.y = 0;

            var inputDir = _player.InputToCameraSpace(input) * _player.Speed;
            dir = Vector3.MoveTowards(dir, inputDir, _player.AirControl * Time.deltaTime);

            dir.y = y;

            _player.MoveDir = dir;
        }
    }
}
