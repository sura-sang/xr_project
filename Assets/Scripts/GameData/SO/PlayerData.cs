using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    [System.Serializable]
    public class EmotionData
    {
        [SerializeField] private Emotion _emotion;

        public float SpeedMultiplier => _speedMultiplier;
        [SerializeField] private float _speedMultiplier = 1;

        public EmotionData(Emotion emotion)
        {
            _emotion = emotion;
        }
    }

    [CreateAssetMenu(fileName = "Player Data", menuName = "SOData/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [Header("레이어 마스크")]
        [SerializeField] private LayerMask _headCheckLayer;
        public LayerMask HeadCheckLayer => _headCheckLayer; // 머리 체크

        [SerializeField] private LayerMask _edgeCheckLayer;
        public LayerMask EdgeCheckLayer => _edgeCheckLayer; // 잡기 체크

        [SerializeField] private LayerMask _enemyCheckLayer;
        public LayerMask EnemyCheckLayer => _enemyCheckLayer; // 적 체크


        [Header("이동")]
        [SerializeField] private float _speed;
        public float Speed => _speed;

        [SerializeField] private float _runMultiplier;
        public float RunMultiplier => _runMultiplier; // 달릴때 속도에 곱해지는 값 

        [SerializeField] private float _grabMultiplier;
        public float GrabMultiplier => _grabMultiplier; // 절벽 잡고 움직일 때 속도에 곱해지는 값


        [Header("중력")]
        [SerializeField] private float _gravity;
        public float Gravity => _gravity;

        [SerializeField] private float _fallingGravityMultiplier;
        public float FallingGravityMultiplier => _fallingGravityMultiplier; // 떨어질때의 중력

        [SerializeField] private float _gravityLimit;
        public float GravityLimit => _gravityLimit; // 최대로 떨어지는 속도


        [Header("점프")]
        [SerializeField] private float _jumpPower;
        public float JumpPower => _jumpPower;

        [SerializeField] private float _coyoteTime;
        public float CoyoteTime => _coyoteTime; // 절벽 끝나고 플레이어가 공중에 있어도 일정 시간동안 점프 가능

        [SerializeField] private float _variableJumpTime;
        public float VariableJumpTime => _variableJumpTime; // 꾸욱 누르면 더 올라가는 기능

        [SerializeField] private float _airControl;
        public float AirControl => _airControl; // 공중에서 컨트롤가능한 정도


        [Header("메달리기")]
        [SerializeField] private float _climbPower;
        public float ClimbPower => _climbPower;

        [SerializeField] private float _edgeDetectLength;
        public float EdgeDetectLength => _edgeDetectLength; // 매달리기 거리

        [SerializeField] private float _moveToLedgeSpeed;
        public float MoveToLedgeSpeed => _moveToLedgeSpeed; // 매달리기 거리 보다 멀어 질려고 할때 다시 붙는 속도


        [Header("흡수")]
        [Range(0f, 360f)]
        [SerializeField] private float _absorbAngle;
        public float AbsorbAngle => _absorbAngle;

        [SerializeField] private float _absorbRange;
        public float AbsorbRange => _absorbRange;


        [Header("감정 데이터")]
        [SerializeField] private EmotionData[] _emotionDatas = new EmotionData[3];
        public EmotionData[] EmotionDatas => _emotionDatas;


        //private void Awake()
        //{
        //    _emotionDatas = new EmotionData[3];
        //    _emotionDatas[0] = new EmotionData((Emotion)1);
        //    _emotionDatas[1] = new EmotionData((Emotion)2);
        //    _emotionDatas[2] = new EmotionData((Emotion)3);
        //}
    }
}