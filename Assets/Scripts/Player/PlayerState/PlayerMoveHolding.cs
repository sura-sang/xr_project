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

        public override void InitializeState()
        {
            _speed = _player.Speed;

            _player.OnMove = OnMove;
            _player.SetAction(ButtonActions.Hold, OnHold);
        }

        public override void ClearState()
        {
        }

        public override void UpdateState()
        {
            //UpdateState가 OnMove보다 먼저 호출됨
            (_isEdgeDetected, _edgeHit) = _player.GetEdgeDetectInfo();

            if ((!_isEdgeDetected && _isHolding) || !_isHolding)
            {
                _isHolding = false;
            }

            if (_isHolding)
            {
                var directionToPoint = (_edgeHit.point - _player.transform.position).normalized;
                var normalVector = _edgeHit.normal;

                _player.LookVector(-normalVector);

                //_curEdge = _characterMove.EdgeHit.transform;
                var distanceToLedge = Vector3.Distance(_player.transform.position, _edgeHit.point);

                if (distanceToLedge > 0.6f)
                {
                    _controller.Move(-normalVector.normalized * _player.MoveToLedgeSpeed * 50f * Time.deltaTime);
                }

                _speed = _player.SlowSpeed;
            }
            else
            {
                _speed = _player.Speed;
            }
        }

        private void OnMove(Vector2 input)
        {
            var dir = _player.InputToCameraSpace(input);
            var pDir = _player.MoveDir;
            var y = pDir.y;

            if (dir != Vector3.zero && !_isHolding)
            {
                _player.LookVector(dir);
            }

            dir *= _speed;

            if (!_isHolding)
            {
                dir.y = _controller.isGrounded ? -1 : _player.MoveDir.y - _player.Gravity * Time.deltaTime;
            }
            _player.MoveDir = dir;

            if (_player.isWPressed && _isHolding)
            {
                var normalVector = _edgeHit.normal;
                y = _player.ClimbPower;

                pDir.y = 0;
                pDir = Vector3.MoveTowards(dir, -normalVector, _player.AirControl * Time.deltaTime);
                pDir.x = -normalVector.x;
                pDir.z = -normalVector.z;
                pDir.y = y;

                _player.MoveDir = pDir;

                _characterMove.ChangeState(new PlayerMoveFalling(_characterMove));
            }

            if (_controller.isGrounded && !_isHolding)
            {
                _characterMove.ChangeState(new PlayerMoveGrounded(_characterMove));
            }
            else if (!_controller.isGrounded && !_isHolding)
            {
                _characterMove.ChangeState(new PlayerMoveFalling(_characterMove));
            }
        }

        private void OnHold(bool isOn)
        {
            Hanging(isOn);
        }

        private void Hanging(bool isOn)
        {
            if (_isEdgeDetected && isOn)
            {
                //Vector3 directionToEdge = _curEdge.position - _characterMove.PlayerTransform.position;
                _isHolding = true;
            }
            else
            {
                _isHolding = false;
            }
        }
    }
}
