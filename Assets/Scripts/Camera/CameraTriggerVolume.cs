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
        private BoxCollider _box;


        private void Awake()
        {
            _box = GetComponent<BoxCollider>();
            _box.isTrigger = true;
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
