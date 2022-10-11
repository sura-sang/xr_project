using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SuraSang
{
    public class TitleScene : MonoBehaviour
    {
        [SerializeField] private GameObject _titleObject;
        [SerializeField] private GameObject _storyObject;

        private int _sequence = 0;

        private void Awake()
        {
            _sequence = 0;
            _titleObject.SetActive(true);
            _storyObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                switch (_sequence++)
                {
                    case 0:
                        _titleObject.SetActive(false);
                        _storyObject.SetActive(true);
                        break;
                    case 1:
                        //씬 이동
                        SceneManager.LoadScene(1);
                        break;
                }
            }
        }
    }
}
