using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SuraSang
{
    public class TitleScene : MonoBehaviour
    {
        [SerializeField] private GameObject _titleObject;
        [SerializeField] private GameObject _menuObject;

        private int _sequence = 0;

        private void Awake()
        {
            _sequence = 0;
            _titleObject.SetActive(true);
            _menuObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                switch (_sequence++)
                {
                    case 0:
                        _titleObject.SetActive(false);
                        _menuObject.SetActive(true);
                        break;
                }
            }
        }

        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void OpenCredit()
        {
            Debug.Log("열려라 크레딧");
        }
    }
}
