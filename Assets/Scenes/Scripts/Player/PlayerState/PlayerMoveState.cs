using UnityEngine;

namespace SuraSang
{
    public abstract class PlayerMoveState : CharacterMoveState
    {
        protected Player _player;
        protected CharacterController _controller;
        
        public PlayerMoveState(CharacterMove characterMove) : base(characterMove) 
        {
            _player = characterMove as Player;

            if (_player == null)
            {
                Debug.LogError($"{this.GetType()} 은 PlayerMoveState 이지만 Player가 사용하지 않습니다.");
            }
            else
            {
                _controller = _player.Controller;
            }
        }
    }
}
