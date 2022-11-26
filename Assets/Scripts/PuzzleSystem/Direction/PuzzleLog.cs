using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class PuzzleLog : PuzzleDirectionBase
    {
        [SerializeField] private float _pushPower;
        private HingeJoint _joint;
        private Rigidbody _rigidbody;
        [SerializeField] private Transform _effectPos;

        private void Awake()
        {
            _joint = GetComponent<HingeJoint>();
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
        }

        public override void OnNotify(PuzzleContext context)
        {
            if (context.SkillEmotion == Emotion.Anger)
            {
                if (!_rigidbody.isKinematic)
                {
                    return;
                }

                base.OnNotify(context);

                if (_context == null)
                {
                    return;
                }

                var angle = GetNearestAngle(GetAngle(_context.Dir));
                var vector = GetVector(angle * Mathf.Deg2Rad);

                _joint.axis = new Vector3(-vector.z, 0, vector.x);

                _rigidbody.isKinematic = false;
                _rigidbody.AddForceAtPosition(vector * _pushPower, _rigidbody.centerOfMass + Vector3.up);

                _effectPos.localPosition = new Vector3(0, 2, 0);

                var effect = Global.Instance.ResourceManager.GetObject(Constant.LogEffect, _effectPos);
                effect.transform.localScale = new Vector3(1, 1, 2.8f);

                switch(angle)
                {
                    case 0:
                        effect.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        Global.Instance.ResourceManager.ReturnObjectWithDelay(Constant.LogEffect, effect, 2f);
                        break;

                    case 90:
                        effect.transform.localRotation = Quaternion.Euler(0, 90, 0);
                        Global.Instance.ResourceManager.ReturnObjectWithDelay(Constant.LogEffect, effect, 2f);
                        break;

                    case 180:
                        effect.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        Global.Instance.ResourceManager.ReturnObjectWithDelay(Constant.LogEffect, effect, 2f);
                        break;

                    case 270:
                        effect.transform.localRotation = Quaternion.Euler(0, 90, 0);
                        Global.Instance.ResourceManager.ReturnObjectWithDelay(Constant.LogEffect, effect, 2f);
                        break;
                }

                AudioManager.Instance.SoundOneShot3D(AudioManager.Instance.SFX_OB_Tree_Fall, transform);
            }
        }
    }
}
