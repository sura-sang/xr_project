using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SuraSang
{
    public class PlayerMoveHolding : PlayerMoveState
    {
        public PlayerMoveHolding(CharacterMove characterMove) : base(characterMove) { }

        private Transform _curEdge;
        private float _speed;
        private bool _isSame;
        private bool _isHolding;

        private bool _isEdgeDetected;
        private RaycastHit _edgeHit;
        private bool _isHit;

        public override void InitializeState()
        {
            AudioManager.Instance.SoundOneShot2D(AudioManager.Instance.SFX_P_Hang);

            _speed = _player.PlayerData.Speed * _player.PlayerData.GrabMultiplier;

            _player.OnMove = OnMove;
            _player.SetAction(ButtonActions.Hold, isOn => _isHolding = isOn);
            _player.Animator.SetBool("IsHolding", true);
        }

        public override void ClearState()
        {
            _player.Animator.SetBool("IsHolding", false);
        }

        public override void UpdateState()
        {
        }

        private void OnMove(Vector2 input)
        {
            (_isEdgeDetected, _edgeHit) = _player.GetEdgeDetectInfo();

            if (_isHolding && _isEdgeDetected)
            {
                var normalVector = _edgeHit.normal;
                //var dir = new Vector3(input.x, 0, input.y);
                var dir = Vector3Extentions.InputToTransformSpace(new Vector2(input.x, 0), _player.transform);

                //var dot = Vector3.Dot(-normalVector, -dir);

                dir -= normalVector;
                if (/*dot > Mathf.Cos(45)*/input.y == 1)
                {
                    _player.Animator.SetFloat("Forward", 1);
                    dir = Vector3.MoveTowards(dir, -normalVector, _player.PlayerData.AirControl * Time.deltaTime);
                    dir.y = _player.PlayerData.ClimbPower;
                    _player.MoveDir = dir;
                }
                else
                {
                    _player.Animator.SetFloat("Forward", 0);
                }

                _player.MoveDir = dir * _speed;
                _player.SmoothRotation(-normalVector);
            }
            else if (_controller.isGrounded)
            {
                _characterMove.ChangeState(new PlayerMoveGrounded(_characterMove));
            }
            else
            {
                _characterMove.ChangeState(new PlayerMoveFalling(_characterMove));
            }
        }
    }
}