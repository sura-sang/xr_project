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
            if (isSkillRunning) _player.IsSkill = true;
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
    }
}
