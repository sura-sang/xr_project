using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Lumin;

namespace SuraSang
{
    public class DoorTrigger : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField] private LayerMask _interactionLayer;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == _interactionLayer)
            {
                EventManager.Instance.OpenDoor(_id);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == _interactionLayer)
            {
                EventManager.Instance.CloseDoor(_id);
            }
        }
    }
}
