using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace SuraSang
{
    public class SceneMaster : MonoBehaviour
    {
        public static SceneMaster SceneInstance;
        public CheckPoint CurrentCheckPoint;
        public string[] LevelArr;

        private string _levelID;
        public string LevelID { get { return _levelID; } }

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
        }

        public void LoadLevel(int num)
        {
            SceneManager.LoadScene(LevelArr[num]);
        }
    }
}
