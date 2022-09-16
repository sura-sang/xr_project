using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class HangingInteraction : MonoBehaviour
    {
        public bool IsHolding;
        public LayerMask DetectedEdge;

        public float EdgeDetectLength;
        public float EdgeSphereCastRadius;

        private RaycastHit _edgeHit;
        private Rigidbody _rigidbody;
        private Transform _curEdge;
        private TestPlayer _tPlayer;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _tPlayer = GetComponent<TestPlayer>();
        }

        void Update()
        {
            Hanging();
        }

        private void Hanging()
        {
            bool isEdgeDetected = Physics.SphereCast(transform.position, EdgeSphereCastRadius, transform.forward, out _edgeHit, EdgeDetectLength, DetectedEdge);

            if (isEdgeDetected)
            {
                _curEdge = _edgeHit.transform;

                Vector3 directionToEdge = _curEdge.position - transform.position;

                if (Input.GetMouseButton(0))
                {
                    IsHolding = true;
                    _rigidbody.useGravity = false;
                    _rigidbody.velocity = Vector3.zero;
                }
                else
                {
                    IsHolding = false;
                    _rigidbody.useGravity = true;
                }
            }
            else if ((!isEdgeDetected && IsHolding) || !IsHolding)
            {
                IsHolding = false;
                _rigidbody.useGravity = true;
            }
            else
            {
                IsHolding = false;
                _rigidbody.useGravity = true;
            }

            if (IsHolding)
            {
                _tPlayer.Speed = _tPlayer.SlowSpeed;
            }
        }
        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;

        //    bool isEdgeDetected = Physics.SphereCast(transform.position, EdgeSphereCastRadius, transform.forward, out _edgeHit, EdgeDetectLength, DetectedEdge);

        //    if (isEdgeDetected)
        //    {
        //        _curEdge = _edgeHit.transform;

        //        Vector3 directionToEdge = _curEdge.position - transform.position;

        //        if (Input.GetMouseButton(0))
        //        {
        //            IsHolding = true;
        //            _rigidbody.useGravity = false;
        //            _rigidbody.velocity = Vector3.zero;
        //        }
        //        else
        //        {
        //            IsHolding = false;
        //            _rigidbody.useGravity = true;
        //        }

        //        Gizmos.DrawRay(transform.position, transform.forward * _edgeHit.distance);
        //        Gizmos.DrawWireSphere(transform.position + transform.forward * _edgeHit.distance, transform.lossyScale.x / 2);
        //    }
        //    else if ((!isEdgeDetected && IsHolding) || !IsHolding)
        //    {
        //        IsHolding = false;
        //        _rigidbody.useGravity = true;
        //    }
        //    else
        //    {
        //        IsHolding = false;
        //        _rigidbody.useGravity = true;
        //        Gizmos.DrawRay(transform.position, transform.forward * EdgeDetectLength);
        //    }

        //    if (IsHolding)
        //    {
        //        _tPlayer.Speed = _tPlayer.SlowSpeed;
        //    }
        //}
    }
}
