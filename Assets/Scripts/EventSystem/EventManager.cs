using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; } = null;

        public event Action<int> OpenDoorAction;
        public event Action<int> CloseDoorAction;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != null)
            {
                Destroy(this.gameObject);
            }
        }

        public void OpenDoor(int id)
        {
            OpenDoorAction?.Invoke(id);
        }

        public void CloseDoor(int id)
        {
            CloseDoorAction?.Invoke(id);
        }
    }
}
