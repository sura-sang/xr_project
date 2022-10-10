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
            _player.Animator.SetBool("IsUseSkill", true);
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

            _skill.UpdateSkill();

            if (!_player.IsSkill && _skill.IsStopAble)
            {
                _player.ChangeState(new PlayerMoveGrounded(_characterMove));
            }
        }

        public override void ClearState()
        {
            _skill.ClearSkill();
            _player.Animator.SetBool("IsUseSkill", false);
        }

        private void OnSkill(bool isOn)
        {
            if(_player.CurrentEmotion != Emotion.Anger) _player.IsSkill = isOn;
        }
    }
}