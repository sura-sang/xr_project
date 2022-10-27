using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; } = null;

        public event Action<int> SharpMoveAction;
        public event Action<int> SharpReturnAction;

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

        public void ShapeMove(int id)
        {
            SharpMoveAction?.Invoke(id);
        }

        public void SharpReturn(int id)
        {
            SharpReturnAction?.Invoke(id);
        }
    }
}
