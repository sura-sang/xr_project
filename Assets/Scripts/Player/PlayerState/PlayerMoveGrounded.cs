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
        private bool _isDancing = false;
        private bool _isRunning = false;

        public override void InitializeState()
        {
            _player.OnMove = OnMove;
            _player.SetAbsorbAction();

            _player.SetAction(ButtonActions.Run, OnRun);
            _player.SetAction(ButtonActions.Jump, OnJump);
            _player.SetAction(ButtonActions.Skill, OnSkill);
            _player.SetAction(ButtonActions.Dance, OnDance);
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
                _player.Animator.SetTrigger("Dance");
                _player.cantMove();
            }

            if (_player.Animator.GetCurrentAnimatorStateInfo(0).IsName("Dance") &&
                _player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                _player.canMove();
            }

            // TO DO : 나중에 수정해야 될지도?
            // (강제로 Grounded 스테이트에서 CanMove가 false일 경우 Change 트리거 실행 시켜줌)
            if(!_player.CanMove)
            {
                _player.Animator.SetTrigger("Change");
                _player.Animator.SetBool("IsRunning", false);
                _player.Animator.SetBool("IsWalking", false);
            }
        }

        public override void ClearState() { }


        private void OnRun(bool isOn)
        {
            if (isOn && _player.CurrentEmotion != Emotion.Sadness && _player.CanMove)
            {
                _isRunning = isOn;
                _player.Animator.SetBool("IsRunning", true);
            }
            else
                _player.Animator.SetBool("IsRunning", false);
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

            var speed = _player.GetPlayerSpeed();

            if (_isRunning)
            {
                speed *= _player.PlayerData.RunMultiplier;
            }

            dir *= speed;
            dir.y = _controller.isGrounded ? -1 : _player.MoveDir.y - _player.PlayerData.Gravity * Time.deltaTime;
            _player.MoveDir = dir;

            if (input != Vector2.zero && _player.CanMove) _player.Animator.SetBool("IsWalking", true);
            else _player.Animator.SetBool("IsWalking", false);
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