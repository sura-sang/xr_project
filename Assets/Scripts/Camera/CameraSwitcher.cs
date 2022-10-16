using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SuraSang
{
    public class CameraSwitcher : MonoBehaviour
    {
        static List<CinemachineVirtualCamera> _cameras = new List<CinemachineVirtualCamera>();
        public static CinemachineVirtualCamera ActiveCamera = null;

        public static void RegisterCamera(CinemachineVirtualCamera camera)
        {
            _cameras.Add(camera);
        }

        public static void UnregisterCamera(CinemachineVirtualCamera camera)
        {
            _cameras.Remove(camera);
        }

        public static void SwitchCamera(CinemachineVirtualCamera camera)
        {
            camera.Priority = 10;
            ActiveCamera = camera;

            foreach (CinemachineVirtualCamera i in _cameras)
            {
                if (i != camera && i.Priority != 0) i.Priority = 0;
            }
        }

        public static bool IsActiveCamera(CinemachineVirtualCamera camera)
        {
            return camera == ActiveCamera;
        }
    }
}
