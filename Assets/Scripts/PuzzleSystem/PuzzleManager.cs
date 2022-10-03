using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class PuzzleManager : PuzzleSubject
    {
        private static PuzzleManager _instance;

        public static PuzzleManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(PuzzleManager)) as PuzzleManager;

                    if (_instance == null)
                        Debug.Log("no singleton obj");
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}
