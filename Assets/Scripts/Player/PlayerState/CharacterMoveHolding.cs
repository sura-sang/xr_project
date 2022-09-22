using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SuraSang
{
    public class CharacterMoveHolding : CharacterMoveState
    {
        public CharacterMoveHolding(CharacterMove characterMove) : base(characterMove) { }

        private Transform _curEdge;
        private float _speed;
        private bool _isSame;

        public override void InitializeState()
        {
            _speed = _characterMove.Speed;

            _characterMove.OnMove = OnMove;
            _characterMove.SetAction(ButtonActions.Hold, OnHold);
        }

        public override void ClearState()
        {
        }

        public override void UpdateState()
        {
            if ((!_characterMove.IsEdgeDetected && _characterMove.IsHolding) || !_characterMove.IsHolding)
            {
                _characterMove.IsHolding = false;
            }

            if (_characterMove.IsHolding)
            {
                var directionToPoint = (_characterMove.EdgeHit.point - _characterMove.PlayerTransform.position).normalized;
                var normalVector = _characterMove.EdgeHit.normal;
   
                _characterMove.LookVector(-normalVector);

                //_curEdge = _characterMove.EdgeHit.transform;
                var distanceToLedge = Vector3.Distance(_characterMove.PlayerTransform.position, _characterMove.EdgeHit.point);

                if (distanceToLedge > 0.6f)
                {
                    _controller.Move(-normalVector.normalized * _characterMove.MoveToLedgeSpeed * 50f * Time.deltaTime);
                }

                _speed = _characterMove.SlowSpeed;
            }
            else
            {
                _speed = _characterMove.Speed;
            }
        }
       
        private void OnMove(Vector2 input)
        {
            var dir = _characterMove.InputToCameraSpace(input);
            var pDir = _characterMove.MoveDir;
            var y = pDir.y;

            if (dir != Vector3.zero && !_characterMove.IsHolding)
            {
                _characterMove.LookVector(dir);
            }

            dir *= _speed;

            if (!_characterMove.IsHolding)
            {
                dir.y = _controller.isGrounded ? -1 : _characterMove.MoveDir.y - _characterMove.Gravity * Time.deltaTime;
            }
            _characterMove.MoveDir = dir;

            if (_characterMove.isWPressed && _characterMove.IsHolding)
            {
                var normalVector = _characterMove.EdgeHit.normal;
                y = _characterMove.ClimbPower;

                pDir.y = 0;
                pDir = Vector3.MoveTowards(dir, -normalVector, _characterMove.AirControl * Time.deltaTime);
                pDir.x = -normalVector.x;
                pDir.z = -normalVector.z;
                pDir.y = y;

                _characterMove.MoveDir = pDir;

                _characterMove.ChangeState(new CharacterMoveFalling(_characterMove));
            }

            if (_controller.isGrounded && !_characterMove.IsHolding)
            {
                _characterMove.ChangeState(new CharacterMoveGrounded(_characterMove));
            }
            else if(!_controller.isGrounded && !_characterMove.IsHolding)
            {
                _characterMove.ChangeState(new CharacterMoveFalling(_characterMove));
            }
        }

        private void OnHold(bool isOn)
        {
            Hanging(isOn);
        }

        private void Hanging(bool isOn)
        {
            if (_characterMove.IsEdgeDetected && isOn)
            {
                //Vector3 directionToEdge = _curEdge.position - _characterMove.PlayerTransform.position;
                _characterMove.IsHolding = true;
            }
            else
            {
                _characterMove.IsHolding = false;
            }
        }
    }
}
