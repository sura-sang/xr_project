using Cinemachine.Utility;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace SuraSang
{
    public class MoveShape : MonoBehaviour
    {
        [SerializeField] private Vector3 _movePoint;
        [SerializeField] private int _id;
        [SerializeField] private float _speed;

        private Vector3 _firstPos;
        private Vector3 _lastPos;
        private bool _isOpen = false;

        [Header("Sound")]
        [SerializeField] private bool _move;
        [SerializeField] private FMODUnity.EventReference _moveSound;
        [SerializeField] private bool _retrun;
        [SerializeField] private FMODUnity.EventReference _returnSound;

        private void Start()
        {
            EventManager.Instance.ShapeMoveAction += ShapeMove;
            EventManager.Instance.ShapeReturnAction += ShapeReturn;

            _firstPos = transform.position;
            _lastPos = transform.position + _movePoint;
        }

        private void OnDisable()
        {
            EventManager.Instance.ShapeMoveAction -= ShapeMove;
            EventManager.Instance.ShapeReturnAction -= ShapeReturn;
        }

        private void Update()
        {
            if (_isOpen == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, _lastPos, _speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _firstPos, _speed * Time.deltaTime);
            }
        }

        private void ShapeMove(int id)
        {
            if (id == this._id)
            {
                _isOpen = true;

                if (_move)
                {
                    AudioManager.Instance.SoundOneShot3D(_moveSound, gameObject.transform);
                }
            }
        }

        private void ShapeReturn(int id)
        {
            if (id == this._id)
            {
                _isOpen = false;

                if (_retrun)
                {
                    AudioManager.Instance.SoundOneShot3D(_returnSound, gameObject.transform);
                }
            }
        }
    }
}
