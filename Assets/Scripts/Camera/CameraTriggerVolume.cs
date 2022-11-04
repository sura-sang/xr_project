using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;
using UnityEngine.Rendering;

namespace SuraSang
{
    public class CameraTriggerVolume : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private Vector3 _boxSize;

        private BoxCollider _box;
        private Rigidbody _rb;


        private void Awake()
        {
            _box = GetComponent<BoxCollider>();
            _rb = GetComponent<Rigidbody>();

            _box.isTrigger = true;
            // _box.size = _boxSize;

            _rb.isKinematic = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, _boxSize);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (!CameraSwitcher.IsActiveCamera(_camera)) CameraSwitcher.SwitchCamera(_camera);
            }
        }
    }
}
