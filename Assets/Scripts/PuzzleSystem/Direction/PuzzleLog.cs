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

        private void Awake()
        {
            _joint = GetComponent<HingeJoint>();
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
        }

        public override void OnNotify(PuzzleContext context)
        {
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
        }
    }
}
