using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Lumin;

namespace SuraSang
{
    public class DoorTrigger : MonoBehaviour
    {
        [SerializeField] private int _id;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventManager.Instance.OpenDoor(_id);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventManager.Instance.CloseDoor(_id);
            }
        }
    }
}
