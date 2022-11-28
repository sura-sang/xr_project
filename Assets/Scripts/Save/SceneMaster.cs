using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace SuraSang
{
    public class SceneMaster : MonoBehaviour
    {
        public bool isDebug = false;
        public static SceneMaster SceneInstance;
        public CheckPoint CurrentCheckPoint;
        public string[] LevelArr;

        public string LevelID { get { return _levelID; } }
        private string _levelID;

        public Player Player => _player;
        [SerializeField] private Player _player;
        public int _replyCount = 0;

        private void Awake()
        {
            Scene scene = SceneManager.GetActiveScene();
            _levelID = scene.name;
            
            //다른 레벨에 있던 씬 마스터 삭제
            if (SceneInstance != null)
            {
                if (SceneInstance.LevelID != scene.name)
                {
                    Destroy(SceneInstance.gameObject);
                    SceneInstance = null;
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            if (SceneInstance == null)
            {
                SceneInstance = this;
                DontDestroyOnLoad(this);
            }

            Global.Instance.SetCurrentSceneMaster(this);

            if (SceneInstance._replyCount == 0)
            {
                GameObject.Find("IntroObject").transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                GameObject.Find("IntroObject").transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (_player == null)
            {
                _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

                if (_player == null)//타이틀 씬을 위한 예외 ㅎ
                {
                    Destroy(gameObject);
                }
            }
        }
        public void LoadLevel(int num)
        {
            SceneInstance._replyCount++;
            AudioManager.Instance.StopAllSFXEvents();
            SceneManager.LoadScene(LevelArr[num]);
        }
    }
}
