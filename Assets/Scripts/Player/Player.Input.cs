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
        Skill,
        Reset,
        Dance,
        CheckPointInteraction,
    }

    public partial class Player
    {
        // 벡터를 넘겨주기 위해 move만 따로 처리
        public UnityAction<Vector2> OnMove { get; set; }
        private Dictionary<ButtonActions, InputAction> _buttonActions;
        private Dictionary<ButtonActions, UnityAction<bool>> _buttonEvents;

        private CharacterActions _inputActions;
        private InputAction _moveInputAction;

        public Vector3 MoveDir { get; set; }

        [ReadOnly] public bool IsSkill;
        [ReadOnly] public bool IsReset = false;

        private void InitInputs()
        {
            _buttonActions = new Dictionary<ButtonActions, InputAction>();
            _buttonEvents = new Dictionary<ButtonActions, UnityAction<bool>>();
           
            _inputActions = new global::CharacterActions();
            _moveInputAction = _inputActions.Player.Move;

            _buttonActions.Add(ButtonActions.Run, _inputActions.Player.Run);
            _inputActions.Player.Run.performed += (x) => GetAction(ButtonActions.Run)?.Invoke(true);
            _inputActions.Player.Run.canceled += (x) => GetAction(ButtonActions.Run)?.Invoke(false);

            _buttonActions.Add(ButtonActions.Jump, _inputActions.Player.Jump);
            _inputActions.Player.Jump.started += (x) => GetAction(ButtonActions.Jump)?.Invoke(true);
            _inputActions.Player.Jump.canceled += (x) => GetAction(ButtonActions.Jump)?.Invoke(false);

            _buttonActions.Add(ButtonActions.Crouch, _inputActions.Player.Crouch);
            _inputActions.Player.Crouch.performed += (x) => GetAction(ButtonActions.Crouch)?.Invoke(true);
            _inputActions.Player.Crouch.canceled += (x) => GetAction(ButtonActions.Crouch)?.Invoke(false);

            _buttonActions.Add(ButtonActions.Catch, _inputActions.Player.Catch);
            _inputActions.Player.Catch.performed += (x) => GetAction(ButtonActions.Catch)?.Invoke(true);
            _inputActions.Player.Catch.canceled += (x) => GetAction(ButtonActions.Catch)?.Invoke(false);

            _buttonActions.Add(ButtonActions.Hold, _inputActions.Player.Hold);
            _inputActions.Player.Hold.performed += (x) => GetAction(ButtonActions.Hold)?.Invoke(true);
            _inputActions.Player.Hold.canceled += (x) => GetAction(ButtonActions.Hold)?.Invoke(false);

            _buttonActions.Add(ButtonActions.Absorb, _inputActions.Player.Absorb);
            _inputActions.Player.Absorb.performed += (x) => GetAction(ButtonActions.Absorb)?.Invoke(true);
            _inputActions.Player.Absorb.canceled += (x) => GetAction(ButtonActions.Absorb)?.Invoke(false);

            _buttonActions.Add(ButtonActions.Skill, _inputActions.Player.Skill);
            _inputActions.Player.Skill.performed += (x) => GetAction(ButtonActions.Skill)?.Invoke(true);           
            _inputActions.Player.Skill.canceled += (x) => GetAction(ButtonActions.Skill)?.Invoke(false);

            _buttonActions.Add(ButtonActions.Reset, _inputActions.Player.Reset);
            _inputActions.Player.Reset.performed += (x) => GetAction(ButtonActions.Reset)?.Invoke(true);
            _inputActions.Player.Reset.canceled += (x) => GetAction(ButtonActions.Reset)?.Invoke(false);

            _buttonActions.Add(ButtonActions.Dance, _inputActions.Player.Dance);
            _inputActions.Player.Dance.performed += (x) => GetAction(ButtonActions.Dance)?.Invoke(true);
            _inputActions.Player.Dance.canceled += (x) => GetAction(ButtonActions.Dance)?.Invoke(false);

            _buttonActions.Add(ButtonActions.CheckPointInteraction, _inputActions.Player.CheckPointInteraction);
            _inputActions.Player.CheckPointInteraction.performed += (x) => GetAction(ButtonActions.CheckPointInteraction)?.Invoke(true);
            _inputActions.Player.CheckPointInteraction.canceled += (x) => GetAction(ButtonActions.CheckPointInteraction)?.Invoke(false);
        }

        private void UpdateInputs()
        {
            var moveInput = _moveInputAction.ReadValue<Vector2>();

            OnMove?.Invoke(moveInput);
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
            
            if (_buttonActions.TryGetValue(type, out var input))
            {
                action?.Invoke(input.IsPressed());
            }
        }

        public Vector3 InputToCameraSpace(Vector2 input)
        {
            return Vector3Extentions.InputToTransformSpace(input, _cameraTransform);
        }

        public bool IsHeadblocked()
        {
            var headPos = transform.position + Vector3.up * (Controller.height * 0.5f);
            return Physics.OverlapSphere(headPos, 0.1f, PlayerData.HeadCheckLayer).Length > 0;
        }

        public bool IsEdgeDetected()
        {
            return Physics.Raycast(transform.position, transform.forward, out var edgeHit, PlayerData.EdgeDetectLength, PlayerData.EdgeCheckLayer);
        }

        public (bool, RaycastHit) GetEdgeDetectInfo()
        {
            return (Physics.Raycast(transform.position, transform.forward, out var edgeHit, PlayerData.EdgeDetectLength, PlayerData.EdgeCheckLayer), edgeHit);
        }
    }
}
