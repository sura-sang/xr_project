using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class CharacterMoveJumping : CharacterMoveState
    {
        public CharacterMoveJumping(CharacterMove characterMove) : base(characterMove) { }

        private bool _isJumpEnd = false;
        private float _jumpStartTime;

        public override void InitializeState()
        {
            _jumpStartTime = Time.time;
            _characterMove.OnMove = OnMove;

            _characterMove.SetAction(ButtonActions.Jump, OnJump);


            var dir = _characterMove.MoveDir;
            dir.y = _characterMove.JumpPower;
            _characterMove.MoveDir = dir;
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
            var dir = _characterMove.MoveDir;
            var y = dir.y;

            if (!_isJumpEnd && Time.time - _jumpStartTime < _characterMove.VariableJumpTime)
            {
                y = _characterMove.JumpPower;
            }

            else if(_characterMove.IsEdgeDetected)
            {
                _characterMove.ChangeState(new CharacterMoveHolding(_characterMove));
            }

            else
            {
                y -= _characterMove.Gravity * Time.deltaTime;
                y = Mathf.Max(y, -_characterMove.GravityLimit);

                if (y < 0)
                {
                    _characterMove.ChangeState(new CharacterMoveFalling(_characterMove));
                }
            }

            dir.y = 0;

            var inputDir = _characterMove.InputToCameraSpace(input) * _characterMove.Speed;
            dir = Vector3.MoveTowards(dir, inputDir, _characterMove.AirControl * Time.deltaTime);

            dir.y = y;

            _characterMove.MoveDir = dir;
        }
    }
}
