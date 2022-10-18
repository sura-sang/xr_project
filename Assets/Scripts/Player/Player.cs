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
        [SerializeField] private bool _debugMode = false;
        
        public CharacterController Controller { get; private set; }
        public Animator Animator { get; private set; }
        public PlayerData PlayerData => _playerData;
        [SerializeField] private PlayerData _playerData;
        
        // 슬픔 스킬용
        public SadEye[] SadEyes => _sadEyes;
        [SerializeField] private SadEye[] _sadEyes;


        [Header("Change Avater Camera")] [SerializeField]
        private CinemachineVirtualCamera _camera;

        [SerializeField] private CinemachineDollyCart _cart;

        [ReadOnly] public bool CanMove = true;
        private GameObject HappyEffect;

        private Transform _cameraTransform;

        private void Awake()
        {
            Controller = GetComponent<CharacterController>();
            Animator = GetComponentInChildren<Animator>();
            _currentCharacter = _characterDefault;

            if (SceneMaster.SceneInstance != null)
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

            if (CanMove)
            {
                Controller.Move(MoveDir * Time.deltaTime);
            }

            if (IsReset)
            {
                SceneMaster.SceneInstance.LoadLevel(0);
            }

            if (IsSkill)
            {
                if (CurrentEmotion == Emotion.Happiness && HappyEffect == null)
                    HappyEffect = Instantiate(GameManager.Instance.HappySkillEffect, transform);
            }
            else
            {
                Destroy(HappyEffect);
            }
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
                    Animator.SetFloat("Emotion", (int)CurrentEmotion);
                    break;

                case Emotion.Anger:
                    _currentCharacter.SetActive(false);
                    _currentCharacter = _characterAnger;
                    _characterAnger.SetActive(true);
                    Animator.avatar = _characterAnger.GetComponent<Animator>().avatar;
                    Animator.Play("Change", 0, 0.4f);
                    Animator.SetFloat("Emotion", (int)CurrentEmotion);
                    break;

                case Emotion.Happiness:
                    _currentCharacter.SetActive(false);
                    _currentCharacter = _characterHappy;
                    _characterHappy.SetActive(true);
                    Animator.avatar = _characterHappy.GetComponent<Animator>().avatar;
                    Animator.Play("Change", 0, 0.4f);
                    Animator.SetFloat("Emotion", (int)CurrentEmotion);
                    break;

                //TO DO : 아직 슬픔 모델 안나옴.
                case Emotion.Sadness:
                    _currentCharacter.SetActive(false);
                    _currentCharacter = _characterDefault;
                    _characterDefault.SetActive(true);
                    Animator.avatar = _characterDefault.GetComponent<Animator>().avatar;
                    Animator.Play("Change", 0, 0.4f);
                    Animator.SetFloat("Emotion", (int)CurrentEmotion);
                    break;
            }
        }

        public void TransEffect()
        {
            switch (CurrentEmotion)
            {
                case Emotion.Anger:
                    CanMove = false;
                    Instantiate(GameManager.Instance.AngerTrans, transform);
                    break;

                case Emotion.Sadness:
                    CanMove = false;
                    Instantiate(GameManager.Instance.SadTrans, transform);
                    break;

                case Emotion.Happiness:
                    CanMove = false;
                    Instantiate(GameManager.Instance.HappyTrans, transform);
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
            _camera.m_Priority = 11;
            _cart.m_Speed = 0.25f;
        }

        public void CameraStop()
        {
            _camera.m_Priority = 9;
            _cart.m_Speed = 0f;
            _cart.m_Position = 0f;
        }

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