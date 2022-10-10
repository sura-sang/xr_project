using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SuraSang
{
    public class AngerSkill : ISkill
    {
        public bool IsStopAble => _isStopAble;
        private bool _isStopAble = false;

        //private static readonly LayerMask CheckMask = ~LayerMask.GetMask("Player", "CameraArea");
        private static readonly LayerMask CheckMask = LayerMask.GetMask("Wall");

        private Player _player;
        private CharacterController _controller;

        private float PlusSpeed = 2f;
        private float SkillRunningTime = 2f;

        private float MaxYRot = 20f;
        private float MinYRot = -20f;
        private float _playerMinYAngles;
        private float _playerMaxYAngles;

        private float _radius = 1f;

        private Vector3 _dir;

        private float _dashStartTime;


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
        
        public void InitializeSkill()
        {
            _dir = _player.MoveDir;
            _dir.y = 0;

            if(_dir == Vector3.zero)
            {
                _dir = _player.transform.forward;
                _dir.y = 0;
            }

            _dir.Normalize();

            _dashStartTime = Time.time;
            _isStopAble = false;
        }

        public void UpdateSkill()
        {
            var move = _dir * (_player.Speed + PlusSpeed) * Time.deltaTime;
            _controller.Move(move);
            _player.SmoothRotation(_dir);

            var capsulePoint1 = _player.transform.position + _controller.center + Vector3.down * _controller.height * 0.5f;
            var capsulePoint2 = capsulePoint1 + Vector3.up * _controller.height;

            var result = Physics.CapsuleCastAll(capsulePoint1, capsulePoint2,
                _radius, _dir, (_player.Speed + PlusSpeed) * Time.deltaTime, CheckMask);

            foreach (var hit in result)
            {
                Debug.Log(hit.transform.gameObject.name);
                hit.collider?.GetComponentInParent<PuzzleElements>()?.OnNotify(new PuzzleContextDirection(_player.transform.forward));
            }

            if (result.Length != 0 || (Time.time - _dashStartTime) > SkillRunningTime)
            {
                _player.IsSkill = false;
                _isStopAble = true;
            }
        }

        public void ClearSkill()
        {
            
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