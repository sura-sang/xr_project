using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace SuraSang
{
    public class PlayerUseSkill : PlayerMoveState
    {

        public PlayerUseSkill(CharacterMove characterMove) : base(characterMove) { }

        public override void InitializeState()
        {
            _player.SetAction(ButtonActions.Skill, isOn => _player.IsSkill = isOn);
        }

        public override void UpdateState()
        {
            if (!_player.IsSkill)
            {
                // 애니메이션 초기화
                _player.Animator.SetBool("IsUseJoySkill", false);
                _player.Animator.SetBool("IsUseAngerSkill", false);

                _player.ChangeState(new PlayerMoveGrounded(_characterMove));
                return;
            }

            if (_player.CurrentEmotion == Emotion.Happiness)
            {
                _player.Animator.SetBool("IsUseJoySkill", true);
                HappySkill();
            }
            else if (_player.CurrentEmotion == Emotion.Anger)
            {
                _player.Animator.SetBool("IsUseAngerSkill", true);
                AngerSkill(); 
            }
            else if (_player.CurrentEmotion == Emotion.Sadness)
            {
                SadSkill();
            }
        }

        public override void ClearState() { }

        private void HappySkill()
        {
            _player.OnMove = HappyMove;
            _player.HappySkill.SkillHappy();
        }

        private void AngerSkill()
        {
            _player.OnMove = AngerMove;
            _player.AngerSkill.OnSkill();
        }

        private void SadSkill()
        {
            _player.OnMove = SadMove;
            _player.SadSkill.OnSkill();
        }

        private void HappyMove(Vector2 input)
        {
            var dir = _player.InputToCameraSpace(input);
            
            _player.SmoothRotation(dir);
            dir *= _player.Speed;
            _player.MoveDir = dir;
        }

        private void AngerMove(Vector2 input)
        {
            var dir = _player.transform.eulerAngles;
            dir.y = (dir.y > 180) ? dir.y - 360 : dir.y;
            float[] rot = _player.AngerSkill.LimitRot(dir);
            dir.y = Mathf.Clamp(dir.y, rot[0], rot[1]);
            // _player.EulerRotation(dir);
            _player.MoveDir = Vector3.zero;
        }

        private void SadMove(Vector2 input)
        {
            var dir = _player.InputToCameraSpace(input);
            if (dir != Vector3.zero)
            {
                _player.SmoothRotation(dir);
            }
            _player.MoveDir = Vector3.zero;
        }
    }
}