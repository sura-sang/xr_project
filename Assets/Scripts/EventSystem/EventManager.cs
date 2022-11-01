using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; } = null;

        public event Action<int> ShapeMoveAction;
        public event Action<int> ShapeReturnAction;

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
            ShapeMoveAction?.Invoke(id);
        }

        public void ShapeReturn(int id)
        {
            ShapeReturnAction?.Invoke(id);
        }
    }
}
