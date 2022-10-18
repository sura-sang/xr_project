using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace SuraSang
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Vector3 _movePoint;
        [SerializeField] private int _id;
        [SerializeField] private int _speed;

        private Vector3 _firstPos;
        private Vector3 _lastPos;
        private bool IsOpen = false;

        private void Start()
        {
            EventManager.Instance.OpenDoorAction += OnOpenDoor;
            EventManager.Instance.CloseDoorAction += OnCloseDoor;

            _firstPos = transform.position;
            _lastPos = transform.position + _movePoint;
        }

        private void OnDisable()
        {
            EventManager.Instance.OpenDoorAction -= OnOpenDoor;
            EventManager.Instance.CloseDoorAction -= OnCloseDoor;
        }

        private void Update()
        {
            if (IsOpen == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, _lastPos, _speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _firstPos, _speed * Time.deltaTime);
            }
        }

        private void OnOpenDoor(int id)
        {
            if (id == this._id)
            {
                IsOpen = true;
            }
        }

        private void OnCloseDoor(int id)
        {
            if (id == this._id)
            {
                IsOpen = false;
            }
        }
    }
}
