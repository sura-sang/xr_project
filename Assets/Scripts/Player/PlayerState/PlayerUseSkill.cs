using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class PlayerUseSkill : PlayerMoveState
    {
        private float _lastGroundTime;
        private ISkill _skill;

        public PlayerUseSkill(CharacterMove characterMove) : base(characterMove) { }

        public override void InitializeState()
        {
            _player.SetAction(ButtonActions.Skill, OnSkill);
            _player.SetAction(ButtonActions.Reset, OnReset);

            if (_player.CurrentEmotion == Emotion.Happiness)
            {
                _skill = new HappySkill(_player, _controller);
            }
            else if (_player.CurrentEmotion == Emotion.Anger)
            {
                _skill = new AngerSkill(_player, _controller);
            }
            else if (_player.CurrentEmotion == Emotion.Sadness)
            {
                _skill = new SadSkill(_player, _controller);
            }

            _player.OnMove = _skill.OnMove;
            _skill.InitializeSkill();
        }

        public override void UpdateState()
        {
            if (!_controller.isGrounded)
            {
                if (Time.time - _lastGroundTime > _player.CoyoteTime)
                {
                    _characterMove.ChangeState(new PlayerMoveFalling(_characterMove));
                }
            }
            else
            {
                _lastGroundTime = Time.time;
            }

            if (!_player.IsSkill)
            {
                _player.ChangeState(new PlayerMoveGrounded(_characterMove));
                AnimationClear();
            }

            _skill.UpdateSkill();
        }

        public override void ClearState()
        {
            _skill.ClearSkill();
        }

        private void OnSkill(bool isOn)
        {
            if(_player.CurrentEmotion != Emotion.Anger) _player.IsSkill = isOn;
        }

        private void AnimationClear()
        {
            // TODO : 전체 애니메이션 파라미터 초기화
            _player.Animator.SetBool("IsUseJoySkill", false);
            _player.Animator.SetBool("IsUseAngerSkill", false);
        }
        private void OnReset(bool isOn)
        {
            _player.IsReset = isOn;
        }

    }
}