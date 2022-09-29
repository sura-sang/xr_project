using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace SuraSang
{
    public class PlayerUseSkill : PlayerMoveState
    {
        private float _lastGroundTime;

        public PlayerUseSkill(CharacterMove characterMove) : base(characterMove) { }

        public override void InitializeState()
        {
            _player.SetAction(ButtonActions.Skill, isOn => _player.IsSkill = isOn);

            if (_player.CurrentEmotion == Emotion.Happiness)
            {
                HappySkill();
                HappyAnimation();
                _player.OnMove = HappyMove;
            }
            else if (_player.CurrentEmotion == Emotion.Anger)
            {
                AngerSkill();
                AngerAnimation();
                _player.OnMove = AngerMove;
            }
            else if (_player.CurrentEmotion == Emotion.Sadness)
            {
                SadSkill();
                SadAnimation();
                _player.OnMove = SadMove;
            }
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
                return;
            }
        }

        public override void ClearState() { }

        private void HappySkill()
        {
            // TODO : 기쁨 스킬
        }

        private void AngerSkill()
        {
            // TODO : 분노 스킬
        }

        private void SadSkill()
        {
            // TODO : 슬픔 스킬
        }

        private void HappyMove(Vector2 input)
        {
            // TODO : 기쁨 무브먼트
        }

        private void AngerMove(Vector2 input)
        {
            // TODO : 분노 무브먼트

            //var dir = _player.transform.eulerAngles;
            //dir.y = (dir.y > 180) ? dir.y - 360 : dir.y;
            //float[] rot = _player.AngerSkill.LimitRot(dir);
            //dir.y = Mathf.Clamp(dir.y, rot[0], rot[1]);
            //// _player.EulerRotation(dir);
            //_player.MoveDir = Vector3.zero;
        }

        private void SadMove(Vector2 input)
        {
            // TODO : 슬픔 무브먼트
        }

        private void HappyAnimation()
        {
            // TODO : 기쁨 애니메이션 파라미터
        }

        private void AngerAnimation()
        {
            // TODO : 분노 애니메이션 파라미터
        }

        private void SadAnimation()
        {
            // TODO : 슬픔 애니메이션 파라미터
        }

        private void AnimationClear()
        {
            // TODO : 애니메이션 파라미터 초기화

            // _player.Animator.SetBool("IsUseJoySkill", false);
            // _player.Animator.SetBool("IsUseAngerSkill", false);
        }
    }
}