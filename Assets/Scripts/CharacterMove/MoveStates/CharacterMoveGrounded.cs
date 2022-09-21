using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class CharacterMoveGrounded : CharacterMoveState
    {
        public CharacterMoveGrounded(CharacterMove characterMove) : base(characterMove) { }

        private float _lastGroundTime;

        private float _speed;

        private bool _isCrouch;
        private bool _isCrouchFailed = false;

        public override void InitializeState()
        {
            _characterMove.OnMove = OnMove;

            _characterMove.SetAction(ButtonActions.Run, OnRun);
            //_characterMove.SetAction(ButtonActions.Crouch, OnCrouch);
            _characterMove.SetAction(ButtonActions.Jump, OnJump);

            _speed = _characterMove.Speed;
        }
        

        public override void UpdateState()
        {
            //if (_isCrouchFailed)
            //{
            //    OnCrouch(_isCrouch);
            //}

            if (!_controller.isGrounded)
            {
                if (Time.time - _lastGroundTime > _characterMove.CoyoteTime)
                {
                    _characterMove.ChangeState(new CharacterMoveFalling(_characterMove));
                }
            }
            else
            {
                _lastGroundTime = Time.time;
            }
        }

        public override void ClearState()
        {
            _characterMove.Crouch(false);
        }


        private void OnRun(bool isOn)
        {
            _characterMove.Crouch(false);
            _speed = isOn ? _characterMove.Speed * _characterMove.RunMultiplier : _characterMove.Speed;
        }

        private void OnCrouch(bool isOn)
        {
            _isCrouch = isOn;
            if (_characterMove.Crouch(isOn))
            {
                _isCrouchFailed = true;
                return;
            }
            _speed = isOn ? _characterMove.Speed * _characterMove.CrouchMultiplier : _characterMove.Speed;
        }

        private void OnJump(bool isOn)
        {
            if (isOn && !_characterMove.IsHeadblocked())
            {
                _characterMove.ChangeState(new CharacterMoveJumping(_characterMove));
            }
        }

        private void OnMove(Vector2 input)
        {
            var dir = _characterMove.InputToCameraSpace(input);

            if (dir != Vector3.zero)
            {
                _characterMove.LookVector(dir);
            }

            dir *= _speed;

            dir.y = _controller.isGrounded ? -1 : _characterMove.MoveDir.y - _characterMove.Gravity * Time.deltaTime;

            _characterMove.MoveDir = dir;
        }
    }
}
