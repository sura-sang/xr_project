using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Lumin;
using UnityEngine.UI;

namespace SuraSang
{
    public class DoorTrigger : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField] private LayerMask _interactionLayer;

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _interactionLayer) != 0)
            {
                EventManager.Instance.OpenDoor(_id);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & _interactionLayer) != 0)
            {
                EventManager.Instance.CloseDoor(_id);
            }
        }
    }
}
