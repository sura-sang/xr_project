using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class HappySkill : ISkill
    {
        private Player _player;
        private CharacterController _controller;
        public LayerMask SkillTarget;
        public const float CheckRange = 10f;

        private List<Monster> _monsterList;
        private Transform _playerTransform;

        readonly int IsWalking = Animator.StringToHash("IsWalking");
        readonly int IsUseJoySkill = Animator.StringToHash("IsUseJoySkill");

        private float _speed;

        public HappySkill(Player player, CharacterController controller)
        {
            _player = player;
            _controller = controller;
            _playerTransform = GameObject.Find("PlayerDummy").GetComponent<Transform>();
            _monsterList = new List<Monster>();
            _speed = _player.Speed;
            SkillTarget = LayerMask.GetMask("Monster");
        }

        //void Start()
        //{
        //    _playerTransform = GameObject.Find("PlayerDummy").GetComponent<Transform>();
        //    _monsterList = new List<Monster>();
        //    _speed = _player.Speed;
        //}

        public void OnMove(Vector2 input)
        {
            var dir = _player.InputToCameraSpace(input);

            if (dir != Vector3.zero)
            {
                _player.LookVector(dir);
            }

            dir *= _speed;
            dir.y = _controller.isGrounded ? -1 : _player.MoveDir.y - _player.Gravity * Time.deltaTime;
            _player.MoveDir = dir;

            _player.Animator.SetBool(IsWalking, _player.Controller.velocity.sqrMagnitude > 0.01f);
        }

        public void OnSkill()
        {
            CheckMonster();

            foreach (Monster monster in _monsterList)
            {
                monster.GetComponent<Sadness>().IsFollow = true;
            }

            _monsterList.Clear();
        }

        public void Animation()
        {
            _player.Animator.SetBool(IsUseJoySkill, true);
        }

        void CheckMonster()
        {
            Collider[] hitedTargets = Physics.OverlapSphere(_controller.transform.position, CheckRange, SkillTarget);

            foreach (Collider monster in hitedTargets)
            {
                if (!_monsterList.Contains(monster.GetComponent<Monster>()) && monster.GetComponent<Monster>().Emotion == Emotion.Sadness)
                    _monsterList.Add(monster.GetComponent<Monster>());
            }
        }
    }
}
