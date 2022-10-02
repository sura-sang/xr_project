using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SuraSang
{
    public class AngerSkill : ISkill
    {
        private Player _player;
        private CharacterController _controller;

        private float PlusSpeed = 2f;
        private float SkillRunningTime = 2f;
        private bool isSkillRunning = false;

        private float MaxYRot = 20f;
        private float MinYRot = -20f;
        private float _playerMinYAngles;
        private float _playerMaxYAngles;

        private float _radius = 1f;
        private float _timer;

        public AngerSkill(Player player, CharacterController controller)
        {
            _player = player;
            _controller = controller;
        }

        public void OnMove(Vector2 input)
        {
            _player.MoveDir = Vector3.down;

            /*var dir = _player.transform.eulerAngles;
            dir.y = (dir.y > 180) ? dir.y - 360 : dir.y;
            float[] rot = _player.AngerSkill.LimitRot(dir);
            dir.y = Mathf.Clamp(dir.y, rot[0], rot[1]);
            _player.EulerRotation(dir);
            _player.MoveDir = Vector3.zero;*/
        }

        public void OnSkill()
        {
            if (!isSkillRunning)
            {
                isSkillRunning = true;
            }
            else if (isSkillRunning)
            {
                Cooldown();
                _controller.Move(_player.transform.forward * (_player.Speed + PlusSpeed) * Time.deltaTime);

                Vector3 vec = _player.transform.position;
                Collider[] colliders = Physics.OverlapSphere(_player.transform.position, _radius);
                foreach (Collider col in colliders)
                {
                    if (col.gameObject.layer != LayerMask.NameToLayer("Player")) Clear();
                }
            }
        }

        public void Animation()
        {
            _player.Animator.SetBool("IsUseAngerSkill", true);
        }

        private void Cooldown()
        {
            if (isSkillRunning)
            {
                _timer += Time.deltaTime;
                Debug.Log(_timer);

                if (_timer >= SkillRunningTime)
                {
                    Clear();
                }
            }
        }

        private void Clear()
        {
            _player.IsSkill = false;
            isSkillRunning = false;
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
    }
}