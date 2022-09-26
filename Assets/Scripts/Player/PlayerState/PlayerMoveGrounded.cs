using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class PlayerMoveGrounded : PlayerMoveState
    {
        public PlayerMoveGrounded(CharacterMove characterMove) : base(characterMove) { }

        private float _lastGroundTime;

        private float _speed;

        private bool _isCrouch;
        private bool _isCrouchFailed = false;
        private bool _isSkill;

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

            // TODO : 임시 스킬 사용
            if (_player.CurrentEmotion == Emotion.Happiness && _isSkill)
            {
                _player.HappySkill.SkillHappy();
            }
            else if (_player.CurrentEmotion == Emotion.Anger && _isSkill)
            {
                _player.AngerSkill.OnSkill();
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


            if (dir != Vector3.zero)
            {
                _player.LookVector(dir);
            }

            dir *= _speed;
            dir.y = _controller.isGrounded ? -1 : _player.MoveDir.y - _player.Gravity * Time.deltaTime;
            _player.MoveDir = dir;
        }

        private void OnSkill(bool isOn)
        {
            _isSkill = isOn;
        }
    }
}