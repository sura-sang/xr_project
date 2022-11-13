using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;

namespace SuraSang
{
    public abstract class MonsterMoveState : CharacterMoveState
    {
        protected Monster _monster;
        protected NavMeshAgent _agent;
        protected Animator _animator;
        
        public MonsterMoveState(CharacterMove characterMove) : base(characterMove) 
        {
            _monster = characterMove as Monster;

            if (_monster == null)
            {
                Debug.LogError($"{this.GetType()} 은 MonsterMoveState 이지만 Monster을 사용하지 않습니다.");
            }
            else
            {
                _animator = _monster.Animator;
                _agent = _monster.Agent;
            }
        }
    }
}