using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class FairyMove : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _yPos;
        [SerializeField] private float _radius = 1.0f;
        [SerializeField] private float _angularVelocity = 100.0f;
        [SerializeField] private float _angle = 0.0f;

        void Start()
        {
        
        }

        void Update()
        {
            _yPos = _target.transform.position.y + 0.5f;
            _angle += _angularVelocity * Time.deltaTime;
            var offset = Quaternion.Euler(0.0f, _angle, 0.0f) * new Vector3(0.0f, 0.0f, _radius);
            transform.position = new Vector3(_target.transform.position.x, _yPos, _target.transform.position.z) + offset;
        }
    }
}
