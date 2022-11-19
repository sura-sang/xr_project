using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class SadnessMove : MonsterMoveState
    {
        private Sadness _sadness;
        private Player _player;
        private bool _isMade = false;

        private GameObject _eff1 = null;
        private GameObject _eff2 = null;

        public SadnessMove(CharacterMove characterMove) : base(characterMove) { }

        public override void InitializeState()
        {
            _sadness = _monster as Sadness;
            _player = Global.Instance.SceneMaster.Player;
        }

        public override void UpdateState() 
        {
            if (Global.Instance.SceneMaster.Player.CurrentEmotion == Emotion.Happiness
                && Vector3.Distance(Global.Instance.SceneMaster.Player.transform.position, _monster.transform.position) < HappySkill.CheckRange)
            {
                _agent.isStopped = false;
                _agent.SetDestination(Global.Instance.SceneMaster.Player.transform.position);
            }
            else if(_player.CurrentEmotion == Emotion.Happiness && !_player.IsSkill)
            {
                _animator.SetBool("IsWalking", false);
                _monster.ChangeState(new SadnessIdle(_monster));
            }
            else
            {
                _animator.SetBool("IsWalking", false);
                _monster.ChangeState(new SadnessIdle(_monster));
            }

            //플레이어와 몬스터의 거리 계산해서 CheckRange보다 작을 경우 이펙트 재생
            if (Vector3.Distance(_player.transform.position, _sadness.transform.position) <= HappySkill.CheckRange)
            {
                if (!_isMade)
                {
                    _eff1 = Global.Instance.ResourceManager.GetObject(Constant.HappyFloorAuraEffect, _sadness.transform);
                    _eff2 = Global.Instance.ResourceManager.GetObject(Constant.HappyMuEffect, _sadness.transform);
                    _isMade = true;
                }
            }
            else
            {
                _isMade = false;

                if (_eff1 != null)
                {
                    Global.Instance.ResourceManager.ReturnObject(Constant.HappyFloorAuraEffect, _eff1);
                    _eff1 = null;
                }

                if (_eff2 != null)
                {
                    Global.Instance.ResourceManager.ReturnObject(Constant.HappyMuEffect, _eff2);
                    _eff2 = null;
                }
            }
        }

        public override void ClearState()
        {
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
            _sadness.IsFollow = false;


            if (_eff1 != null)
            {
                Global.Instance.ResourceManager.ReturnObject(Constant.HappyFloorAuraEffect, _eff1);
                _eff1 = null;
            }

            if (_eff2 != null)
            {
                Global.Instance.ResourceManager.ReturnObject(Constant.HappyMuEffect, _eff2);
                _eff2 = null;
            }
        }
    }
}
