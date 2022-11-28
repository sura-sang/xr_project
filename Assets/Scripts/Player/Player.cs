using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Cinemachine;

namespace SuraSang
{
    public partial class Player : CharacterMove
    {
        private readonly int MoveSpeedParam = Animator.StringToHash("MoveSpeed");

        [SerializeField] private bool _debugMode = false;

        public CharacterController Controller { get; private set; }
        public Animator Animator { get; private set; }
        public PlayerData PlayerData { get; private set; }

        // 슬픔 스킬용
        public SadEye[] SadEyes => _sadEyes;
        [SerializeField] private SadEye[] _sadEyes;

        // 분노 스킬 이펙트용
        public Transform EffectTransform;

        [Header("Change Avater Camera")]
        [SerializeField]
        private CinemachineVirtualCamera _camera;

        [SerializeField] private CinemachineDollyCart _cart;
        [SerializeField] private float _cartSpeed;

        [ReadOnly] public bool CanMove = true;

        private Transform _cameraTransform;

        // 사운드 버그 때문에 트램펄린 체크 용도
        public bool IsTrampolin = false;

        private void Awake()
        {
            PlayerData = Global.Instance.SODataManager.GetData<PlayerData>();

            Controller = GetComponent<CharacterController>();
            Animator = GetComponentInChildren<Animator>();
            _currentCharacter = _characterDefault;

            if (SceneMaster.SceneInstance != null && !SceneMaster.SceneInstance.isDebug)
            {
                Controller.enabled = false;
                transform.position = SceneMaster.SceneInstance.CurrentCheckPoint.SpawnPos.position;
                ReturnEmotion();
                Controller.enabled = true;
            }

            _cameraTransform = Camera.main.transform;

            InitInputs();
            InitAbsorb();

            ChangeState(new PlayerMoveFalling(this));
        }

        public override void ChangeState(CharacterMoveState state)
        {
            _buttonEvents.Clear();
            OnMove = null;
            base.ChangeState(state);
        }

        protected new void Update()
        {
            SetAction(ButtonActions.Reset, OnReset);
            SetAction(ButtonActions.CheckPointInteraction, OnCheckPointClick);

            base.Update();

            UpdateInputs();
            UpdateAbsorb();

            Controller.Move(MoveDir * Time.deltaTime);

            Animator.SetFloat(MoveSpeedParam, new Vector2(Controller.velocity.x, Controller.velocity.z).magnitude);

            if (IsReset)
            {
                SceneMaster.SceneInstance.LoadLevel(0);
            }

            ChangeEffect();
            ChangeStateEffect();

            DetermineTerrain();
        }

        public override void MovePosition(Vector3 pos)
        {
            Controller.enabled = false;
            transform.position = pos;
            Controller.enabled = true;
        }

        //private void OnControllerColliderHit(ControllerColliderHit hit)
        //{
        //    var rig = hit.collider.attachedRigidbody;

        //    if (rig != null)
        //    {
        //        var forceDir = hit.gameObject.transform.position - transform.position;
        //        forceDir.y = 0;
        //        forceDir.Normalize();

        //        rig.AddForceAtPosition(forceDir * ForceMagnitude, transform.position, ForceMode.Impulse);
        //    }
        //}

        public void ChangeCharacter()
        {
            switch (CurrentEmotion)
            {
                case Emotion.Default:
                    _currentCharacter.SetActive(false);
                    _currentCharacter = _characterDefault;
                    _characterDefault.SetActive(true);
                    Animator.avatar = _characterDefault.GetComponent<Animator>().avatar;
                    break;

                case Emotion.Anger:
                    _currentCharacter.SetActive(false);
                    _currentCharacter = _characterAnger;
                    _characterAnger.SetActive(true);
                    Animator.avatar = _characterAnger.GetComponent<Animator>().avatar;
                    Animator.Play("Change", 0, 0.4f);
                    Animator.SetBool("Change", false);
                    break;

                case Emotion.Happiness:
                    _currentCharacter.SetActive(false);
                    _currentCharacter = _characterHappy;
                    _characterHappy.SetActive(true);
                    Animator.avatar = _characterHappy.GetComponent<Animator>().avatar;
                    Animator.Play("Change", 0, 0.4f);
                    Animator.SetBool("Change", false);
                    break;

                case Emotion.Sadness:
                    _currentCharacter.SetActive(false);
                    _currentCharacter = _characterSad;
                    _characterSad.SetActive(true);
                    Animator.avatar = _characterSad.GetComponent<Animator>().avatar;
                    Animator.Play("Change", 0, 0.4f);
                    Animator.SetBool("Change", false);
                    break;
            }

            if (CurrentEmotion != Emotion.Default)
            {
                AudioManager.Instance.SoundOneShot3D(AudioManager.Instance.SFX_P_AB_2, gameObject.transform);
            }
        }

        public void SwitchAnimatorLayer()
        {
            switch (CurrentEmotion)
            {
                case Emotion.Happiness:
                    Animator.SetLayerWeight(0, 0f);
                    Animator.SetLayerWeight(1, 1f);
                    Animator.SetLayerWeight(2, 0f);
                    Animator.SetLayerWeight(3, 0f);
                    break;

                case Emotion.Anger:
                    Animator.SetLayerWeight(0, 0f);
                    Animator.SetLayerWeight(1, 0f);
                    Animator.SetLayerWeight(2, 1f);
                    Animator.SetLayerWeight(3, 0f);
                    break;

                case Emotion.Sadness:
                    Animator.SetLayerWeight(0, 0f);
                    Animator.SetLayerWeight(1, 0f);
                    Animator.SetLayerWeight(2, 0f);
                    Animator.SetLayerWeight(3, 1f);
                    break;
            }
        }

        public void canMove()
        {
            CanMove = true;
        }

        public void cantMove()
        {
            CanMove = false;
        }

        public void CameraStart()
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);

            _camera.m_Priority = 11;

            _cart.m_Position = 0f;
            _cart.m_Speed = _cartSpeed;
        }

        public void CameraStop()
        {
            _camera.m_Priority = 9;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_debugMode)
            {
                Gizmos.color = Color.red;

                var edgeInfo = GetEdgeDetectInfo();

                if (edgeInfo.Item1)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawRay(transform.position, transform.forward * edgeInfo.Item2.distance);
                }
                else
                {
                    Gizmos.DrawRay(transform.position, transform.forward * PlayerData.EdgeDetectLength);
                }

                Handles.color = _isFind ? Color.red : Color.blue;

                Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, PlayerData.AbsorbAngle / 2,
                    PlayerData.AbsorbRange);
                Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -PlayerData.AbsorbAngle / 2,
                    PlayerData.AbsorbRange);

                Gizmos.DrawWireSphere(transform.position, HappySkill.CheckRange);
            }
        }
#endif

        private void OnReset(bool isOn)
        {
            IsReset = isOn;
        }

        private void OnCheckPointClick(bool isOn)
        {
            IsCheckPointClick = isOn;
        }
    }
}