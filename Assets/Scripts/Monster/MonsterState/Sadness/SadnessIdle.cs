using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class SadnessIdle : MonsterMoveState
    {
        private Sadness _sadness;
        public SadnessIdle(CharacterMove characterMove) : base(characterMove) { }
        private GameObject _sadEff;

        public override void InitializeState() 
        {
            _sadness = _monster as Sadness;
            _sadEff = Global.Instance.ResourceManager.GetObject(Constant.SadPoolEffect, _sadness.transform);
            _sadEff.transform.localScale = _sadness.transform.localScale / 2;
        }

        public override void UpdateState()
        {
            if (_sadness.IsFollow)
            {
                _animator.SetBool("IsWalking", true);
                _monster.ChangeState(new SadnessMove(_monster));
            }          
        }

        public override void ClearState() 
        { 
            if(_sadEff != null)
            {
                Global.Instance.ResourceManager.ReturnObject(Constant.SadPoolEffect, _sadEff);
                _sadEff = null;
            }
        }
    }
}
