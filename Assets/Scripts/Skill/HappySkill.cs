using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class HappySkill : ISkill
    {
        public const float CheckRange = 5;

        private Player _player;
        private CharacterController _controller;

        public HappySkill(Player player, CharacterController controller)
        {
            _player = player;
            _controller = controller;
        }

        public void OnMove(Vector2 input)
        {
            // TO DO : 기쁨 무브먼트
        }

        public void OnSkill()
        {
            // TO DO : 기쁨 스킬
        }

        public void Animation()
        {
            // TO DO : 기쁨 애니메이션 파라미터
        }



        /*
        public LayerMask SkillTarget;
        public static float CheckRange = 10f;

        private List<Monster> _monsterList;
        private Transform _playerTransform;

        void Start()
        {
            _playerTransform = GameObject.Find("PlayerDummy").GetComponent<Transform>();
            _monsterList = new List<Monster>();
        }

        void Update()
        {
        }

        public void SkillHappy()
        {
            CheckMonster();

            foreach (Monster monster in _monsterList)
            {
                var speed = Vector3.zero;
                //monster.transform.position = Vector3.SmoothDamp(monster.transform.position, _playerTransform.position, ref speed , 0.1f);
                //monster.Agent.SetDestination(_playerTransform.position);
                monster.ChangeState(new SadnessMove(monster));
            }

            _monsterList.Clear();
        }

        void CheckMonster()
        {
            Collider[] hitedTargets = Physics.OverlapSphere(transform.position, CheckRange, SkillTarget);

            foreach (Collider monster in hitedTargets)
            {
                if (!_monsterList.Contains(monster.GetComponent<Monster>()) && monster.GetComponent<Monster>().Emotion == Emotion.Sadness)
                    _monsterList.Add(monster.GetComponent<Monster>());
            }
        }
        */
    }
}
