using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace SuraSang
{
    public class HappySkill : MonoBehaviour
    {
        public LayerMask SkillTarget;

        //TODO : 임시 변수
        public float FollowSpeed;

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
            //animator로 기쁨의 춤 애니메이션 재생
            CheckMonster();

            foreach (Monster monster in _monsterList)
            {
                var speed = Vector3.zero;
                //monster.transform.position = Vector3.SmoothDamp(monster.transform.position, _playerTransform.position, ref speed , 0.1f);
                monster.Agent.SetDestination(_playerTransform.position);
            } 

            _monsterList.Clear();
        }

        void CheckMonster()
        {
            Collider[] hitedTargets = Physics.OverlapSphere(transform.position, 10, SkillTarget);

            foreach (Collider monster in hitedTargets)
            {
                if (!_monsterList.Contains(monster.GetComponent<Monster>()) && monster.GetComponent<Monster>().Emotion == Emotion.Sadness)
                    _monsterList.Add(monster.GetComponent<Monster>());
            }
        }
    }
}
