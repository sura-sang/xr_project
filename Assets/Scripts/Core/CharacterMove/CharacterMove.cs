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
        [SerializeField] private float _rotationSpeed;

        private CharacterMoveState _currentState;

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

        public void SmoothRotation(Vector3 dir)
        {
            if (dir == Vector3.zero)
            {
                return;
            }

            dir.y = 0;

            var target = Quaternion.LookRotation(dir, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, target, _rotationSpeed * Time.deltaTime);
        }

        public abstract void MovePosition(Vector3 pos);
    }
}
