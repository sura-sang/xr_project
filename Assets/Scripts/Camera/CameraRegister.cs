using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class CameraRegister : MonoBehaviour
    {
        private void OnEnable()
        {
            CameraSwitcher.RegisterCamera(GetComponent<CinemachineVirtualCamera>());
        }
        private void OnDisable()
        {
            CameraSwitcher.UnregisterCamera(GetComponent<CinemachineVirtualCamera>());
        }
    }
}
