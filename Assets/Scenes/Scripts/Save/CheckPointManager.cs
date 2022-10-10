using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace SuraSang
{
    public class CheckPointManager : MonoBehaviour
    {
        public static CheckPointManager CpManager;
        private string _levelID;

        public string LevelID { get { return _levelID; } }

        private void Awake()
        {
            Scene scene = SceneManager.GetActiveScene();
            _levelID = scene.name;

            //다른 레벨에 있던 체크포인트 매니저 삭제
            if (CpManager != null)
            {
                if (CpManager.LevelID != scene.name)
                {
                    Destroy(CpManager.gameObject);
                    CpManager = null;
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            if(CpManager == null)
            {
                CpManager = this;
                DontDestroyOnLoad(this);
            }
        }
    }
}
