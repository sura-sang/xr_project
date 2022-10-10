using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class TestPlayer : MonoBehaviour
    {
        public float Speed = 10.0f;
        public float SlowSpeed = 5.0f;
        public int JumpPower;
        public float RotateSpeed;

        private Vector3 _movement;
        private bool _isJumping;
        private Rigidbody _rigidbody;
        private HangingInteraction _hangInteraction;
        private GrabInteraction _grabInteraction;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _hangInteraction = GetComponent<HangingInteraction>();
            _grabInteraction = GetComponent<GrabInteraction>();
        }

        private void Update()
        {
            Jump();
        }

        private void FixedUpdate()
        {
            _movement.x = Input.GetAxis("Horizontal");
            _movement.z = Input.GetAxis("Vertical");

            //transform.Translate(movement*Speed*Time.deltaTime);

            Vector3 dir = new Vector3(_movement.x, 0, _movement.z);

            if (!(_movement.x == 0 && _movement.z == 0))
            {
                transform.position += dir * Speed * Time.deltaTime;

                if (!_hangInteraction.IsHolding && !_grabInteraction.IsGrab)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * RotateSpeed);
                    Speed = 10.0f;
                }
            }
        }

        void Jump()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(!_isJumping)
                {
                    _isJumping = true;
                    _rigidbody.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
                _isJumping = false;
        }
    }
}
