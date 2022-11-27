using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class HappySkill : ISkill
    {
        public bool IsStopAble => true;

        private Player _player;
        private CharacterController _controller;
        public const float CheckRange = 3.5f;
        public LayerMask SkillTarget;

        private List<Monster> _monsterList;

        readonly int IsWalking = Animator.StringToHash("IsWalking");
        readonly int IsUseJoySkill = Animator.StringToHash("IsUseJoySkill");

        private float _speed;

        private FMOD.Studio.EventInstance _danceState;

        public HappySkill(Player player, CharacterController controller)
        {
            _player = player;
            _controller = controller;
            _monsterList = new List<Monster>();
            _speed = _player.PlayerData.Speed;
            SkillTarget = LayerMask.GetMask("Monster");
        }

        //void Start()
        //{
        //    _playerTransform = GameObject.Find("PlayerDummy").GetComponent<Transform>();
        //    _monsterList = new List<Monster>();
        //    _speed = _player.Speed;
        //}
        public void InitializeSkill()
        {
            _danceState = FMODUnity.RuntimeManager.CreateInstance(AudioManager.Instance.SFX_P_Dance);
            _danceState.start();
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(_danceState, _player.transform);

            if (AudioManager.Instance.ForestOneState.isValid())
            {
                AudioManager.Instance.ForestOneState.setParameterByName("Volume", 1);
            }

            if (AudioManager.Instance.ForestTwoState.isValid())
            {
                AudioManager.Instance.ForestTwoState.setParameterByName("Volume", 1);
            }
        }

        public void OnMove(Vector2 input)
        {
            var dir = _player.InputToCameraSpace(input);

            _player.SmoothRotation(dir);

            dir *= _speed;
            dir.y = _controller.isGrounded ? -1 : _player.MoveDir.y - _player.PlayerData.Gravity * Time.deltaTime;
            _player.MoveDir = dir;
        }

        public void UpdateSkill()
        {
            CheckMonster();

            foreach (Monster monster in _monsterList)
            {
                monster.GetComponent<Sadness>().IsFollow = true;
            }

            _monsterList.Clear();
        }

        
        public void ClearSkill()
        {
            _danceState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _danceState.release();
            _danceState.clearHandle();

            if (AudioManager.Instance.ForestOneState.isValid())
            {
                AudioManager.Instance.ForestOneState.setParameterByName("Volume", 0);
            }

            if (AudioManager.Instance.ForestTwoState.isValid())
            {
                AudioManager.Instance.ForestTwoState.setParameterByName("Volume", 0);
            }
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
