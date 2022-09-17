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

            if (!_isJumpEnd && Time.time - _jumpStartTime < _characterMove.VariableJumpTime)
            {
                dir.y = _characterMove.JumpPower;
                _characterMove.MoveDir = dir;
            }
            else
            {
                dir.y -= _characterMove.Gravity * Time.deltaTime;
                dir.y = Mathf.Max(dir.y, -_characterMove.GravityLimit);

                if (dir.y < 0)
                {
                    _characterMove.ChangeState(new CharacterMoveFalling(_characterMove));
                }
            }

            _characterMove.MoveDir = dir;
        }
    }
}
