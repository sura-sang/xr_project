using System.Collections;
using System.Collections.Generic;
using UnityEditor.Recorder.Input;
using UnityEngine;

namespace SuraSang
{
    public class PlayerMoveJumping : PlayerMoveState
    {
        public PlayerMoveJumping(CharacterMove characterMove) : base(characterMove) { }

        public PlayerMoveJumping(CharacterMove characterMove, Vector3 overrideJumpDir) : base(characterMove)
        {
            _overrideJumpDir = overrideJumpDir;
        }

        private Vector3 _overrideJumpDir = Vector3.zero;
        private bool _isJumpEnd = false;
        private bool _isHolding;
        private float _jumpStartTime;

        public override void InitializeState()
        {
            _jumpStartTime = Time.time;
            _player.OnMove = OnMove;

            _player.SetAction(ButtonActions.Hold, isOn => _isHolding = isOn);
            _player.SetAction(ButtonActions.Jump, OnJump);
            if (_player.CanMove)
                _player.Animator.SetBool("IsJumping", true);

            if (_overrideJumpDir == Vector3.zero)
            {
                var dir = _player.MoveDir * (_player.GetActionValue(ButtonActions.Run) ? _player.PlayerData.JumpRunMultiplier : 1);
                dir.y = _player.PlayerData.JumpPower;
                _player.MoveDir = dir;
            }
            else
            {
                _player.MoveDir = _overrideJumpDir;
            }

            AudioManager.Instance.SoundOneShot3D(AudioManager.Instance.SFX_P_Jump, _player.transform);
        }

        public override void UpdateState() { }

        public override void ClearState()
        {
            _player.Animator.SetBool("IsJumping", false);
        }

        private void OnJump(bool isOn)
        {
            if (!isOn && _player.CanMove)
            {
                _isJumpEnd = true;
            }
        }

        private void OnMove(Vector2 input)
        {
            var dir = _player.MoveDir;
            var y = dir.y;

            if (!_isJumpEnd && Time.time - _jumpStartTime < _player.PlayerData.VariableJumpTime)
            {
                y = _player.PlayerData.JumpPower;
            }
            else
            {
                y -= _player.PlayerData.Gravity * Time.deltaTime;
                y = Mathf.Max(y, -_player.PlayerData.GravityLimit);

                if (y < 0)
                {
                    _characterMove.ChangeState(new PlayerMoveFalling(_characterMove));
                }
            }

            if (_player.IsEdgeDetected() && _isHolding)
            {
                _characterMove.ChangeState(new PlayerMoveHolding(_characterMove));
            }

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
        }
    }
}