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
            _player.SetAction(ButtonActions.Hold, isOn => _isHolding = isOn);
        }

        public override void ClearState() { }

        public override void UpdateState() { }

        private void OnMove(Vector2 input)
        {
            (_isEdgeDetected, _edgeHit) = _player.GetEdgeDetectInfo();


            if (_isHolding && _isEdgeDetected)
            {
                var dir = Vector3Extentions.InputToTransformSpace(new Vector2(input.x, 0), _player.transform);
                var normalVector = _edgeHit.normal;
                //_player.ClimbPower;

                dir -= normalVector;
                
                //나중에 위로 올라가는거 만들기

                _player.MoveDir = dir * _speed;
                _player.LookVector(-normalVector);
            }
            else
            {
                _characterMove.ChangeState(new PlayerMoveFalling(_characterMove));
            }
        }
    }
}