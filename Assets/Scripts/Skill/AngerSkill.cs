using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SuraSang
{
    public class AngerSkill : MonoBehaviour
    {
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
            //OnSkill();
        }

        private void FixedUpdate()
        {
            if (isSkillRunning)
            {
                if (!Physics.Raycast(transform.position, transform.forward, out _hit, RayDistance))
                {
                    Vector3 playerEulerAngles = transform.rotation.eulerAngles;
                    playerEulerAngles.y = (playerEulerAngles.y > 180) ? playerEulerAngles.y - 360 : playerEulerAngles.y;

                    playerEulerAngles.y = Mathf.Clamp(playerEulerAngles.y, _playerMinYAngles, _playerMaxYAngles);
                    transform.rotation = Quaternion.Euler(playerEulerAngles);

                    transform.Translate(transform.forward * (_tempSpeed + PlusSpeed) * Time.deltaTime, Space.World);
                }
                else
                {
                    isSkillRunning = false;
                    CoroutineReset();
                }
            }
        }

        private void LimitRot(Vector3 v)
        {
            _playerMinYAngles = v.y + MinYRot;
            _playerMaxYAngles = v.y + MaxYRot;

            _playerMinYAngles = (_playerMinYAngles < -180) ? _playerMinYAngles + 360 : _playerMinYAngles;
            _playerMaxYAngles = (_playerMaxYAngles > 180) ? _playerMaxYAngles - 360 : _playerMaxYAngles;
        }

        public void OnSkill()
        {
            if (!isSkillRunning)
            {
                isSkillRunning = true;
                StartCoroutine(_runningCo);
                _tempSpeed = _player.Speed;
                _player.Speed = 1.0f;
                LimitRot(transform.rotation.eulerAngles);
            }
        }

        private void CoroutineReset()
        {
            isSkillRunning = false;
            StopCoroutine(_runningCo);
            _player.Speed = _tempSpeed;
            _runningCo = Coroutine();
        }

        IEnumerator Coroutine()
        {
            yield return new WaitForSeconds(SkillRunningTime);
            CoroutineReset();
        }
    }
}
