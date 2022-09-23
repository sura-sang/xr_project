using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace SuraSang
{
    public enum ButtonActions
    {
        Run,
        Jump,
        Crouch,
        Catch,
        Hold,
        Absorb,
    }

    public partial class Player
    {
        // 벡터를 넘겨주기 위해 move만 따로 처리
        public UnityAction<Vector2> OnMove { get; set; }
        private Dictionary<ButtonActions, UnityAction<bool>> _buttonEvents;

        private CharacterActions _inputActions;
        private InputAction _moveInputAction;


        public bool isWPressed;
        public Vector3 MoveDir { get; set; }


        private void InitInputs()
        {
            _buttonEvents = new Dictionary<ButtonActions, UnityAction<bool>>();

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


            _inputActions.Player.Hold.performed += (x) => GetAction(ButtonActions.Hold)?.Invoke(true);
            _inputActions.Player.Hold.canceled += (x) => GetAction(ButtonActions.Hold)?.Invoke(false);

            _inputActions.Player.Hold.performed += (x) => GetAction(ButtonActions.Absorb)?.Invoke(true);
            _inputActions.Player.Hold.canceled += (x) => GetAction(ButtonActions.Absorb)?.Invoke(false);

            _inputActions.Player.Move.performed += (x) => isWPressed = true;
            _inputActions.Player.Move.canceled += (x) => isWPressed = false;
        }

        private void UpdateInputs()
        {
            var moveInput = _moveInputAction.ReadValue<Vector2>();

            OnMove?.Invoke(moveInput);

            if (moveInput.x == 0.0f && moveInput.y == 1.0f)
            {
                isWPressed = true;
            }
            else
            {
                isWPressed = false;
            }

        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
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

        public bool IsEdgeDetected()
        {
            return Physics.Raycast(transform.position, transform.forward, out var edgeHit, EdgeDetectLength, DetectedEdge);
        }

        public (bool, RaycastHit) GetEdgeDetectInfo()
        {
            return (Physics.Raycast(transform.position, transform.forward, out var edgeHit, EdgeDetectLength, DetectedEdge), edgeHit);
        }
    }
}
