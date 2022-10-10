using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace SuraSang
{
    public partial class Player : CharacterMove
    {
        public CharacterController Controller { get; private set; }

        public LayerMask HeadCheckLayer;
        public LayerMask DetectedEdge;//매달리기 레이어 설정
        public LayerMask DetectedObject; //잡기 레이어 설정

        // 슬픔 스킬용
        public SadEye[] SadEyes => _sadEyes;
        [SerializeField] private SadEye[] _sadEyes;
        
        // TODO : 다른곳으로 옮기자
        public float CharacterHeight;
        public float CharacterCrouchHeight;

        public float Gravity;
        public float FallingGravityMultiplier;// 떨어질때의 중력
        public float GravityLimit;

        public float RunMultiplier;
        public float CrouchMultiplier;
        public float Speed;

        public float SlowSpeed;

        public float CoyoteTime;// 절벽 끝나고 플레이어가 공중에 있어도 일정 시간동안 점프 가능
        public float VariableJumpTime;// 꾸욱 누르면 더 올라가는 기능
        public float AirControl;// 공중에서 컨트롤가능한 정도

        public float JumpPower;
        public float ClimbPower;

        public float ForceMagnitude;//물체 미는 힘

        public float EdgeDetectLength;//매달리기 거리
        public float MoveToLedgeSpeed;//매달리기 거리 보다 멀어 질려고 할때 다시 붙는 속도

        public HappySkill HappySkill;
        public AngerSkill AngerSkill;
        public SadSkill SadSkill;

        private Transform _cameraTransform;

        public Animator Animator;
        public GameObject CurrentCharacter;

        [SerializeField] private GameObject _characterDefault;
        [SerializeField] private GameObject _characterAnger;
        [SerializeField] private GameObject _characterHappy;
        [SerializeField] private GameObject _characterSad;

        public bool CanMove = true;

        private void Awake()
        {
            Controller = GetComponent<CharacterController>();

            if (SceneMaster.SceneInstance != null)
            {
                Controller.enabled = false;
                transform.position = SceneMaster.SceneInstance.CurrentCheckPoint.transform.position;
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
            switch(CurrentEmotion)
            {
                case Emotion.Default:
                    CurrentCharacter.SetActive(false);
                    CurrentCharacter = _characterDefault;
                    _characterDefault.SetActive(true);
                    Animator.avatar = _characterDefault.GetComponent<Animator>().avatar;

                    //Animator.Play("Change", 0, 0.4f);
                    //Animator.SetFloat("Emotion", (int)CurrentEmotion);
                    break;

                case Emotion.Anger:
                    CurrentCharacter.SetActive(false);
                    CurrentCharacter = _characterAnger;
                    _characterAnger.SetActive(true);
                    Animator.avatar = _characterAnger.GetComponent<Animator>().avatar;
                    Animator.Play("Change", 0, 0.4f);
                    Animator.SetFloat("Emotion", (int)CurrentEmotion);
                    break;

                case Emotion.Happiness:
                    CurrentCharacter.SetActive(false);
                    CurrentCharacter = _characterHappy;
                    _characterHappy.SetActive(true);
                    Animator.avatar = _characterHappy.GetComponent<Animator>().avatar;
                    Animator.Play("Change", 0, 0.4f);
                    Animator.SetFloat("Emotion", (int)CurrentEmotion);
                    break;

                    //아직 슬픔 모델 안나옴
                case Emotion.Sadness:
                    //CurrentCharacter.SetActive(false);
                    //CurrentCharacter = _characterSad;
                    //_characterSad.SetActive(true);
                    Animator.Play("ChangeTest", 0, 0.4f);
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
                    Gizmos.DrawRay(transform.position, transform.forward * EdgeDetectLength);
                }

                Handles.color = _isFind ? Color.red : Color.blue;

                Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, _ViewAngle / 2, _viewRadius);
                Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -_ViewAngle / 2, _viewRadius);

                Gizmos.DrawWireSphere(transform.position, HappySkill.CheckRange);
            }
        }

        private void OnReset(bool isOn)
        {
            IsReset = isOn;
        }
    }
}
