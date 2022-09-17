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

            dir.y -= _characterMove.Gravity * _characterMove.FallingGravityMultiplier * Time.deltaTime;
            dir.y = Mathf.Max(dir.y, -_characterMove.GravityLimit);

            _characterMove.MoveDir = dir;

            if (_controller.isGrounded)
            {
                _characterMove.ChangeState(new CharacterMoveGrounded(_characterMove));
            }
        }

    }
}
