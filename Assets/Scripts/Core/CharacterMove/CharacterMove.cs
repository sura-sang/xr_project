using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace SuraSang
{
    public abstract class CharacterMove : MonoBehaviour
    {
        private CharacterMoveState _currentState;

        public CharacterController Controller => _controller;
        protected CharacterController _controller;

        public virtual void ChangeState(CharacterMoveState state)
        {
            if (_currentState != null)
            {
                Debug.Log($"상태 종료: {_currentState.GetType()}");
                _currentState.ClearState();
            }
            _currentState = state;
            _currentState.InitializeState();
            Debug.Log($"상태 시작: {state.GetType()}");
        }

        protected void Update()
        {
            _currentState.UpdateState();
        }
    }
}
