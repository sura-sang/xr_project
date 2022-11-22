using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class PlayerMoveFalling : PlayerMoveState
    {
        // TODO : 여기서 메달리기 상태로 이어지게 만들면 될 듯

        private bool _isHolding;

        public PlayerMoveFalling(CharacterMove characterMove) : base(characterMove) { }

        public override void InitializeState()
        {
            _player.OnMove = OnMove;
            _player.SetAction(ButtonActions.Hold, isOn => _isHolding = isOn);
            _player.Animator.SetBool("IsFalling", true);
        }

        public override void UpdateState() { }

        public override void ClearState()
        {
            _player.Animator.SetBool("IsFalling", false);
        }

        private void OnMove(Vector2 input)
        {
            var dir = _player.MoveDir;

            var y = dir.y - _player.PlayerData.Gravity * _player.PlayerData.FallingGravityMultiplier * Time.deltaTime;
            y = Mathf.Max(y, -_player.PlayerData.GravityLimit);

            if (input != Vector2.zero)
            {
                dir.y = 0;

                var inputDir = _player.InputToCameraSpace(input);
                inputDir.y = 0;
                inputDir.Normalize();
                dir = Vector3.RotateTowards(dir, inputDir, _player.PlayerData.AirControl * Time.deltaTime, 0);

            }
            dir.y = y;

            _player.MoveDir = dir;
            _player.SmoothRotation(dir);

            if (_controller.isGrounded)
            {
                _characterMove.ChangeState(new PlayerMoveGrounded(_characterMove));
            }
            else if (_player.IsEdgeDetected() && _isHolding && !_controller.isGrounded)
            {
                _characterMove.ChangeState(new PlayerMoveHolding(_characterMove));
            }
        }
    }
}
