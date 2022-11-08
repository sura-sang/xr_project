using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class ObjectActivateArea : MonoBehaviour
    {
        [SerializeField] private GameObject[] objects;
        [SerializeField] private LayerMask _interactionLayer;
        private void Start()
        {
            EventManager.Instance.ObjectActivateAction += ObjectActivate;
            EventManager.Instance.ObjectDeActivateAction += ObjectDeActivate;
        }

        private void OnDisable()
        {
            EventManager.Instance.ObjectActivateAction -= ObjectActivate;
            EventManager.Instance.ObjectDeActivateAction -= ObjectDeActivate;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _interactionLayer) != 0)
            {
                ObjectActivate();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & _interactionLayer) != 0)
            {
                ObjectDeActivate();
            }
        }

        private void ObjectActivate()
        {
            foreach (GameObject obj in objects)
            {
                obj.SetActive(true);
            }
        }

        private void ObjectDeActivate()
        {
            foreach (GameObject obj in objects)
            {
                obj.SetActive(false);
            }
        }
    }
}
