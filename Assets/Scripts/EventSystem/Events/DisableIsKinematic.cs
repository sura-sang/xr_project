using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class DisableIsKinematic : MonoBehaviour
    {
        [SerializeField] private int _id;
        private Rigidbody _rigidbody;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            EventManager.Instance.DisableKinematic += OffKinematic;
        }

        private void OnDisable()
        {
            EventManager.Instance.DisableKinematic -= OffKinematic;
        }


        void OffKinematic(int id)
        {
            if (id == this._id)
                _rigidbody.isKinematic = false;
        }
    }
}
