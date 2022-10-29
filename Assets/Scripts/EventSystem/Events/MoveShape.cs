using Cinemachine.Utility;
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
        [SerializeField] private int _speed;

        private Vector3 _firstPos;
        private Vector3 _lastPos;
        private bool _isOpen = false;

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
            }
        }

        private void ShapeReturn(int id)
        {
            if (id == this._id)
            {
                _isOpen = false;
            }
        }
    }
}
