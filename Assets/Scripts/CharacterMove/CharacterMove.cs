using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace SuraSang
{
    public enum ButtonActions
    {
        Run,
        Jump,
        Crouch,
        Catch,
    }

    public class CharacterMove : MonoBehaviour
    {

        public LayerMask HeadCheckLayer;

        // TODO : 다른곳으로 옮기자
        public float CharacterHeight;
        public float CharacterCrouchHeight;

        public float Gravity;
        public float FallingGravityMultiplier;// 떨어질때의 중력
        public float GravityLimit;

        public float RunMultiplier;
        public float CrouchMultiplier;
        public float Speed;

        public float CoyoteTime;// 절벽 끝나고 플레이어가 공중에 있어도 일정 시간동안 점프 가능
        public float VariableJumpTime;// 꾸욱 누르면 더 올라가는 기능
        public float AirControl;// 공중에서 컨트롤가능한 정도

        public float JumpPower;

        public float ForceMagnitude;//물체 미는 힘

        public Vector3 MoveDir { get; set; }


        // 벡터를 넘겨주기 위해 move만 따로 처리
        public UnityAction<Vector2> OnMove { get; set; }
        private Dictionary<ButtonActions, UnityAction<bool>> _buttonEvents;

        private CharacterMoveState _currentState;

        private CharacterActions _inputActions;
        private InputAction _moveInputAction;

        public CharacterController Controller => _controller;
        private CharacterController _controller;

        private Transform _cameraTransform;

        private void Awake()
        {
            _buttonEvents = new Dictionary<ButtonActions, UnityAction<bool>>();

            _controller = GetComponent<CharacterController>();
            _cameraTransform = Camera.main.transform;

            SetInputActions();

            ChangeState(new CharacterMoveFalling(this));
        }


        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void Update()
        {
            var moveInput = _moveInputAction.ReadValue<Vector2>();

            OnMove?.Invoke(moveInput);
            _currentState.UpdateState();

            _controller.Move(MoveDir * Time.deltaTime);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            var rig = hit.collider.attachedRigidbody;

            if (rig != null)
            {
                var forceDir = hit.gameObject.transform.position - transform.position;
                forceDir.y = 0;
                forceDir.Normalize();

                rig.AddForceAtPosition(forceDir * ForceMagnitude, transform.position, ForceMode.Impulse);
            }
        }

        public void ChangeState(CharacterMoveState state)
        {
            if (_currentState != null)
            {
                Debug.Log($"상태 종료: {_currentState.GetType()}");
                _currentState.ClearState();
                _buttonEvents.Clear();
                OnMove = null;
            }
            _currentState = state;
            _currentState.InitializeState();
            Debug.Log($"상태 시작: {state.GetType()}");
        }

        private void SetInputActions()
        {
            _inputActions = new global::CharacterActions();
            _moveInputAction = _inputActions.Player.Move;

            _inputActions.Player.Run.performed += (x) => GetAction(ButtonActions.Run)?.Invoke(true);
            _inputActions.Player.Run.canceled += (x) => GetAction(ButtonActions.Run)?.Invoke(false);

            _inputActions.Player.Jump.started += (x) => GetAction(ButtonActions.Jump)?.Invoke(true);
            _inputActions.Player.Jump.canceled += (x) => GetAction(ButtonActions.Jump)?.Invoke(false);

            _inputActions.Player.Crouch.performed += (x) => GetAction(ButtonActions.Crouch)?.Invoke(true);
            _inputActions.Player.Crouch.canceled += (x) => GetAction(ButtonActions.Crouch)?.Invoke(false);

            _inputActions.Player.Catch.performed += (x) => GetAction(ButtonActions.Catch)?.Invoke(true);
            _inputActions.Player.Catch.canceled += (x) => GetAction(ButtonActions.Catch)?.Invoke(false);
        }

        private UnityAction<bool> GetAction(ButtonActions type)
        {
            if (_buttonEvents.TryGetValue(type, out var action))
            {
                return action;
            }
            return null;
        }

        public void SetAction(ButtonActions type, UnityAction<bool> action)
        {
            if (!_buttonEvents.ContainsKey(type))
            {
                _buttonEvents.Add(type, action);
            }
            else
            {
                _buttonEvents[type] = action;
            }
        }

        public Vector3 InputToCameraSpace(Vector2 input)
        {
            var foward = _cameraTransform.forward;
            var right = _cameraTransform.right;
            foward.y = 0;
            right.y = 0;
            foward.Normalize();
            right.Normalize();

            var fowardReVerticalInput = input.y * foward;
            var rightRelHorizontalInput = input.x * right;

            return fowardReVerticalInput + rightRelHorizontalInput;
        }

        public void LookVector(Vector3 dir)
        {
            dir.y = 0;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }

        public bool Crouch(bool active)
        {
            if (active)
            {
                _controller.height = CharacterCrouchHeight;
            }
            else
            {
                if (IsHeadblocked())
                {
                    return false;
                }

                _controller.height = CharacterHeight;
            }

            return true;
        }

        public bool IsHeadblocked()
        {
            var headPos = transform.position + Vector3.up * (CharacterHeight * 0.5f);
            return Physics.OverlapSphere(headPos, 0.1f, HeadCheckLayer).Length > 0;
        }
    }
}
