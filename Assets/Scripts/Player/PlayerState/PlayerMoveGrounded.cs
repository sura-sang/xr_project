using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class PlayerMoveGrounded : PlayerMoveState
    {
        readonly int IsWalking = Animator.StringToHash("IsWalking");
        readonly int IsRunning = Animator.StringToHash("IsRunning");

        public PlayerMoveGrounded(CharacterMove characterMove) : base(characterMove) { }

        private float _lastGroundTime;

        private float _speed;

        private bool _isCrouch;
        private bool _isCrouchFailed = false;

        public override void InitializeState()
        {
            _player.OnMove = OnMove;
            _player.SetAbsorbAction();

            _player.SetAction(ButtonActions.Run, OnRun);
            //_characterMove.SetAction(ButtonActions.Crouch, OnCrouch);
            _player.SetAction(ButtonActions.Jump, OnJump);
            _player.SetAction(ButtonActions.Skill, OnSkill);

            _speed = _player.Speed;
        }

        public override void UpdateState()
        {
            //if (_isCrouchFailed)
            //{
            //    OnCrouch(_isCrouch);
            //}

            if (!_controller.isGrounded)
            {
                if (Time.time - _lastGroundTime > _player.CoyoteTime)
                {
                    _characterMove.ChangeState(new PlayerMoveFalling(_characterMove));
                }
            }
            else
            {
                _lastGroundTime = Time.time;
            }

            if (_player.IsSkill && _player.CurrentEmotion != Emotion.Default)
            {
                _characterMove.ChangeState(new PlayerUseSkill(_characterMove));
            }
        }

        public override void ClearState()
        {
            _player.Crouch(false);
        }


        private void OnRun(bool isOn)
        {
            _player.Crouch(false);
            _speed = isOn ? _player.Speed * _player.RunMultiplier : _player.Speed;
        }

        private void OnCrouch(bool isOn)
        {
            _isCrouch = isOn;
            if (_player.Crouch(isOn))
            {
                _isCrouchFailed = true;
                return;
            }
            _speed = isOn ? _player.Speed * _player.CrouchMultiplier : _player.Speed;
        }

        private void OnJump(bool isOn)
        {
            if (isOn && !_player.IsHeadblocked())
            {
                _characterMove.ChangeState(new PlayerMoveJumping(_characterMove));
            }
        }

        private void OnMove(Vector2 input)
        {
            var dir = _player.InputToCameraSpace(input);

            if (_player.CanMove)
            {
                _player.SmoothRotation(dir);
            }

            dir *= _speed;
            dir.y = _controller.isGrounded ? -1 : _player.MoveDir.y - _player.Gravity * Time.deltaTime;
            _player.MoveDir = dir;
            
            if (input != Vector2.zero) _player.Animator.SetFloat("Speed", _speed);
            else _player.Animator.SetFloat("Speed", 0f);
        }

        private void OnSkill(bool isOn)
        {
            _player.IsSkill = isOn;
        }
    }
}