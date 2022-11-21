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
        public event Action<int> PlatformMoveAction;
        public event Action<int> ShapeReturnAction;
        public event Action ObjectActivateAction;
        public event Action ObjectDeActivateAction;
        public event Action TimelineStartAction;
        public event Action TimelineStopAction;

        public Emotion PEmotion { get; set; }

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

        public void PlatformMove(int id)
        {
            PlatformMoveAction?.Invoke(id);
        }

        public void ShapeMove(int id)
        {
            ShapeMoveAction?.Invoke(id);
        }

        public void ShapeReturn(int id)
        {
            ShapeReturnAction?.Invoke(id);
        }

        public void ObjectActivate()
        {
            ObjectActivateAction?.Invoke();
        }

        public void ObjectDeActivate()
        {
            ObjectDeActivateAction?.Invoke();
        }

        public void TimelineStart()
        {
            TimelineStartAction?.Invoke();
        }

        public void TimelineStop()
        {
            TimelineStopAction?.Invoke();
        }
    }
}
