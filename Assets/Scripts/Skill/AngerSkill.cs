using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class AngerSkill : MonoBehaviour, ISkill
    {
        private Player _player;
        private CharacterController _controller;

        public AngerSkill(Player player, CharacterController controller)
        {
            _player = player;
            _controller = controller;
        }

        public void OnMove(Vector2 input)
        {
            var dir = _player.InputToCameraSpace(input);


            if (dir != Vector3.zero)
            {
                _player.LookVector(dir);
            }

            dir *= _player.Speed;
            dir.y = _controller.isGrounded ? -1 : _player.MoveDir.y - _player.Gravity * Time.deltaTime;
            _player.MoveDir = dir;
        }

        public void OnSkill()
        {
            // TO DO : 분노 스킬
        }

        public void Animation()
        {
            // TO DO : 분노 애니메이션 파라미터
        }



        /*
        public float PlusSpeed = 2f;
        public float SkillRunningTime = 5f;
        public float RayDistance = 1f;
        public float MaxYRot = 20f;
        public float MinYRot = -20f;
        public bool isSkillRunning = false;


        private RaycastHit _hit;
        private IEnumerator _runningCo;
        private Player _player;
        private float _playerMinYAngles;
        private float _playerMaxYAngles;
        private float _tempSpeed;


        private void Start()
        {
            _runningCo = Coroutine();
            _player = GetComponent<Player>();
        }

        private void Update()
        {
            if (isSkillRunning) _player.IsSkill = true;
        }

        private void AngerMove(Vector2 input)
        {
            // TODO : 분노 무브먼트

            //var dir = _player.transform.eulerAngles;
            //dir.y = (dir.y > 180) ? dir.y - 360 : dir.y;
            //float[] rot = _player.AngerSkill.LimitRot(dir);
            //dir.y = Mathf.Clamp(dir.y, rot[0], rot[1]);
            //// _player.EulerRotation(dir);
            //_player.MoveDir = Vector3.zero;
        }

        private void FixedUpdate()
        {
            if (isSkillRunning)
            {
                if (!Physics.Raycast(transform.position, transform.forward, out _hit, RayDistance))
                {
                    transform.Translate(transform.forward * (_player.Speed + PlusSpeed) * Time.deltaTime, Space.World);
                }
                else
                {
                    Clear();
                }
            }
        }

        public void OnSkill()
        {
            if (!isSkillRunning)
            {
                isSkillRunning = true;
                StartCoroutine(_runningCo);
            }
        }

        private void Clear()
        {
            _player.IsSkill = false;
            isSkillRunning = false;
            StopCoroutine(_runningCo);
            _runningCo = Coroutine();
        }

        public float[] LimitRot(Vector3 v)
        {
            float[] rot = new float[2];

            _playerMinYAngles = v.y + MinYRot;
            _playerMaxYAngles = v.y + MaxYRot;

            _playerMinYAngles = (_playerMinYAngles < -180) ? _playerMinYAngles + 360 : _playerMinYAngles;
            _playerMaxYAngles = (_playerMaxYAngles > 180) ? _playerMaxYAngles - 360 : _playerMaxYAngles;

            return rot;
        }

        IEnumerator Coroutine()
        {
            yield return new WaitForSeconds(SkillRunningTime);
            Clear();
        }
        */
    }
}
