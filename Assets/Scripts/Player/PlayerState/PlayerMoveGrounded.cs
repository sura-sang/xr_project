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
        private bool _isDancing = false;

        public override void InitializeState()
        {
            _player.OnMove = OnMove;
            _player.SetAbsorbAction();

            _player.SetAction(ButtonActions.Run, OnRun);
            _player.SetAction(ButtonActions.Jump, OnJump);
            _player.SetAction(ButtonActions.Skill, OnSkill);
            _player.SetAction(ButtonActions.Dance, OnDance);

            _speed = _player.PlayerData.Speed;
        }

        public override void UpdateState()
        {

            if (!_controller.isGrounded)
            {
                if (Time.time - _lastGroundTime > _player.PlayerData.CoyoteTime)
                {
                    _characterMove.ChangeState(new PlayerMoveFalling(_characterMove));
                }
            }
            else
            {
                _lastGroundTime = Time.time;
            }

            if (_player.IsSkill && _player.CurrentEmotion != Emotion.Default && _player.CanMove)
            {
                _characterMove.ChangeState(new PlayerUseSkill(_characterMove));
            }

            if (_isDancing && _player.CurrentEmotion == Emotion.Happiness && _player.CanMove)
            {
                _player.Animator.SetTrigger("IsDance");
                _player.cantMove();
            }

            if (_player.Animator.GetCurrentAnimatorStateInfo(0).IsName("Dance") &&
                _player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                _player.canMove();
            }

        }

        public override void ClearState() { }


        private void OnRun(bool isOn)
        {
            _speed = isOn ? _player.PlayerData.Speed * _player.PlayerData.RunMultiplier : _player.PlayerData.Speed;
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
            dir.y = _controller.isGrounded ? -1 : _player.MoveDir.y - _player.PlayerData.Gravity * Time.deltaTime;
            _player.MoveDir = dir;
            
            if (input != Vector2.zero) _player.Animator.SetFloat("Speed", _speed);
            else _player.Animator.SetFloat("Speed", 0f);
        }

        private void OnSkill(bool isOn)
        {
            _player.IsSkill = isOn;
        }

        private void OnDance(bool isOn)
        {
            _isDancing = isOn;
        }
    }
}