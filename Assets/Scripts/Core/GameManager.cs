using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace SuraSang
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; } = null;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance);
            }
            Instance = this;
        }

        [SerializeField] private Player _player;
        public Player Player => _player;
    }
}