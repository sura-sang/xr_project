using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class CharacterMoveFalling : CharacterMoveState
    {
        // TODO : 여기서 메달리기 상태로 이어지게 만들면 될 듯

        public CharacterMoveFalling(CharacterMove characterMove) : base(characterMove) { }

        public override void InitializeState()
        {
            _characterMove.OnMove = OnMove;
            _controller = _characterMove.Controller;
        }

        public override void UpdateState() { }

        public override void ClearState() { }
    
        private void OnMove(Vector2 input)
        {
            var dir = _characterMove.MoveDir;

            var y = dir.y - _characterMove.Gravity * _characterMove.FallingGravityMultiplier * Time.deltaTime;
            y = Mathf.Max(y, -_characterMove.GravityLimit);

            dir.y = 0;

            var inputDir = _characterMove.InputToCameraSpace(input) * _characterMove.Speed;
            dir = Vector3.MoveTowards(dir, inputDir, _characterMove.AirControl * Time.deltaTime);

            dir.y = y;

            _characterMove.MoveDir = dir;

            if (_controller.isGrounded)
            {
                _characterMove.ChangeState(new CharacterMoveGrounded(_characterMove));
            }
        }
    }
}
