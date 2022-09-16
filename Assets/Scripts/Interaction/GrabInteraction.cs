using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class GrabInteraction : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        public bool IsGrab;
        public LayerMask DetectedObject;
        public float ObjectDetectLength;

        private RaycastHit _ObjectHit;
        private TestPlayer _tPlayer;

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>(); 
            _tPlayer = GetComponent<TestPlayer>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void Grab()
        {
            bool isGrabDetected = Physics.Raycast(transform.position, transform.forward, out _ObjectHit, ObjectDetectLength, DetectedObject);
        
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            bool isGrabDetected = Physics.Raycast(transform.position, transform.forward, out _ObjectHit, ObjectDetectLength, DetectedObject);
            Gizmos.DrawRay(transform.position, transform.forward * ObjectDetectLength);

            if(isGrabDetected)
            {
                if(Input.GetMouseButton(0))
                {
                    IsGrab = true;
                    _ObjectHit.transform.parent = gameObject.transform;
                }
                else
                {
                    IsGrab = false;
                    _ObjectHit.transform.parent = null;
                }
            }
            
            if(IsGrab)
            {
                _tPlayer.Speed = _tPlayer.SlowSpeed;
            }
        }
    }
}
