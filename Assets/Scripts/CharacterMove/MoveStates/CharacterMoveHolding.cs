using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                Vector3 directionToPoint = (_characterMove.EdgeHit.point - _characterMove.PlayerTransform.position).normalized;
                _characterMove.LookVector(directionToPoint);
                _characterMove.PlayerTransform.rotation = _characterMove.EdgeHit.transform.rotation;

                //_curEdge = _characterMove.EdgeHit.transform;
                var distanceToLedge = Vector3.Distance(_characterMove.PlayerTransform.position, _characterMove.EdgeHit.point);

                if (distanceToLedge > 0.6f)
                {
                    _controller.Move(directionToPoint.normalized * _characterMove.MoveToLedgeSpeed * 10f * Time.deltaTime);
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

            if (_controller.isGrounded)
            {
                _characterMove.ChangeState(new CharacterMoveGrounded(_characterMove));
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
