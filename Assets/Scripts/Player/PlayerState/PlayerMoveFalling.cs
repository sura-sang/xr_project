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
            _player.SetAction(ButtonActions.Reset, OnReset);
        }

        public override void UpdateState() { }

        public override void ClearState() { }

        private void OnMove(Vector2 input)
        {
            var dir = _player.MoveDir;

            var y = dir.y - _player.Gravity * _player.FallingGravityMultiplier * Time.deltaTime;
            y = Mathf.Max(y, -_player.GravityLimit);

            dir.y = 0;

            var inputDir = _player.InputToCameraSpace(input) * _player.Speed;
            dir = Vector3.MoveTowards(dir, inputDir, _player.AirControl * Time.deltaTime);

            dir.y = y;

            _player.MoveDir = dir;

            if (_controller.isGrounded)
            {
                _characterMove.ChangeState(new PlayerMoveGrounded(_characterMove));
            }
            else if (_player.IsEdgeDetected() && _isHolding && !_controller.isGrounded)
            {
                _characterMove.ChangeState(new PlayerMoveHolding(_characterMove));
            }
        }

        private void OnReset(bool isOn)
        {
            _player.IsReset = isOn;
        }
    }
}
