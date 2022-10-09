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
            _speed = _player.SlowSpeed;

            _player.OnMove = OnMove;
            _player.SetAction(ButtonActions.Hold, isOn => _isHolding = isOn);
            _player.SetAction(ButtonActions.Reset, OnReset);
        }

        public override void ClearState() { }

        public override void UpdateState()
        {
        }

        private void OnMove(Vector2 input)
        {
            (_isEdgeDetected, _edgeHit) = _player.GetEdgeDetectInfo();

            if (_isHolding && _isEdgeDetected)
            {
                var dir = Vector3Extentions.InputToTransformSpace(new Vector2(input.x, 0), _player.transform);
                var normalVector = _edgeHit.normal;

                dir -= normalVector;

                if (input.y == 1f )
                {
                    dir = Vector3.MoveTowards(dir, -normalVector, _player.AirControl * Time.deltaTime);
                    dir.y = _player.ClimbPower;
                    _player.MoveDir = dir;
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
        private void OnReset(bool isOn)
        {
            _player.IsReset = isOn;
        }

    }
}