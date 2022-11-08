using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SuraSang
{
    public class FairyMove : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _yPos;
        [SerializeField] private float _radius = 1.0f;
        [SerializeField] private float _angularVelocity = 100.0f;
        private float _angle = 0.0f;
        private Transform _contactTransform;
        private float maxDist = 1.5f;

        [SerializeField] private bool _onWall = false;

        void Start()
        {
            var offset = Quaternion.Euler(0.0f, _angle, 0.0f) * new Vector3(0.0f, 0.0f, _radius);
            transform.position = new Vector3(_target.transform.position.x, _yPos, _target.transform.position.z) + offset;
        }

        void Update()
        {
            if (!_onWall)
            {
                _yPos = _target.transform.position.y + 0.5f;
                _angle += _angularVelocity * Time.deltaTime;
                var offset = Quaternion.Euler(0.0f, _angle, 0.0f) * new Vector3(0.0f, 0.0f, _radius);
                transform.position = new Vector3(_target.transform.position.x, _yPos, _target.transform.position.z) + offset;
            }
            else
            {
                transform.position = _contactTransform.position;
            }

            if (Vector3.Distance(transform.position, _target.position) > maxDist)
            {
                var offset = Quaternion.Euler(0.0f, _angle, 0.0f) * new Vector3(0.0f, 0.0f, _radius);
                transform.position = new Vector3(_target.transform.position.x, _yPos, _target.transform.position.z) + offset;
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            _onWall = true;
            _contactTransform = transform;
        }

        private void OnCollisionExit(Collision collision)
        {
            _onWall = false;
        }
    } 
}
