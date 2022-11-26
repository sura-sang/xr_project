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

        [Header("Config")]
        [SerializeField] private bool _oneShot;
        [SerializeField] private bool _overrideDistance;
        [SerializeField] private float min;
        [SerializeField] private float max;

        private int _count = 0;

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
                    if (_oneShot && _count == 0)
                    {
                        if (_overrideDistance)
                        {
                            AudioManager.Instance.SoundOneShot3DOverrideDistance(_moveSound, gameObject.transform, min, max);
                        }
                        else
                        {
                            AudioManager.Instance.SoundOneShot3D(_moveSound, gameObject.transform);
                        }

                        _count++;
                    }

                    if (!_oneShot)
                    {
                        if (transform.position != _lastPos)
                        {
                            if (_overrideDistance)
                            {
                                AudioManager.Instance.SoundOneShot3DOverrideDistance(_moveSound, gameObject.transform, min, max);
                            }
                            else
                            {
                                AudioManager.Instance.SoundOneShot3D(_moveSound, gameObject.transform);
                            }
                        }
                    }
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
                    if (_oneShot && _count == 0)
                    {
                        if (_overrideDistance)
                        {
                            AudioManager.Instance.SoundOneShot3DOverrideDistance(_moveSound, gameObject.transform, min, max);
                        }
                        else
                        {
                            AudioManager.Instance.SoundOneShot3D(_moveSound, gameObject.transform);
                        }

                        _count++;
                    }

                    if (!_oneShot)
                    {
                        if (transform.position != _firstPos)
                        {
                            if (_overrideDistance)
                            {
                                AudioManager.Instance.SoundOneShot3DOverrideDistance(_moveSound, gameObject.transform, min, max);
                            }
                            else
                            {
                                AudioManager.Instance.SoundOneShot3D(_moveSound, gameObject.transform);
                            }
                        }
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_overrideDistance)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(transform.position, min);
                Gizmos.DrawWireSphere(transform.position, max);
            }
        }
    }
}
