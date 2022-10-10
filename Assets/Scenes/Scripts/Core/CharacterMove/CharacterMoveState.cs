using UnityEngine;

namespace SuraSang
{
    public abstract class CharacterMoveState
    {
        protected CharacterMove _characterMove;

        public abstract void InitializeState();
        public abstract void UpdateState();
        public abstract void ClearState();

        public CharacterMoveState(CharacterMove characterMove)
        {
            _characterMove = characterMove;
        }
    }
}
