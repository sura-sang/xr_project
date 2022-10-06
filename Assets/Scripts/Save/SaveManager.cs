using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SuraSang
{
    public class SaveManager : MonoBehaviour
    {
        private static SaveManager _instance;

        private List<LevelMemento> _levelList;

        public static SaveManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(SaveManager)) as SaveManager;

                    if (_instance == null)
                        Debug.Log("no singleton obj : " + _instance.GetType());
                }

                return _instance;
            }
        }

        private void Awake()
        {
            _levelList = new();

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

        private void Update()
        {
            foreach (LevelMemento test in _levelList)
            {
                Debug.Log($"레벨 : {test.LevelData}, 위치 : {test.TranformData} 출력");
            }
        }

        public void PushLevelData(LevelMemento level)
        {
            //if(_levelList == null)
            //{
            //    _levelList = new();
            //}

            _levelList.Add(level);
        }

        public LevelMemento PopLastLevelMemento()
        {
            LevelMemento data = GetLastLevelMemento();

            if (data != null)
            {
                _levelList.Remove(data);
            }

            return data;
        }

        public LevelMemento GetLastLevelMemento()
        {
            //최소한의 예외처리
            if (!CheckListUsable())
            {
                Debug.Log("데이터 불러오기 실패 : GetLastLevelMemento");
                return null;
            }

            return _levelList[_levelList.Count - 1];
        }

        public LevelMemento GetLevelMementoAt(int index)
        {
            if (!CheckListUsable())
            {
                Debug.Log("데이터 불러오기 실패 : GetLevelMementoAt.");
                return null;
            }
            else if (_levelList.Count <= index)
            {
                Debug.Log("요청한 인덱스가 리스트의 크기를 넘어섬.");
                return null;
            }

            return _levelList[index];
        }

        public bool CheckListUsable()
        {
            if (_levelList == null)
            {
                Debug.Log("레벨 데이터 리스트가 정의되지 않음.");
            }
            else if (_levelList.Count == 0)
            {
                Debug.Log("레벨 데이터 리스트에 저장된 데이터가 없음.");
            }
            else
            {
                return true;
            }

            return false;
        }

        public void ClearLevelMementoList()
        {
            _levelList.Clear();
        }
    }
}
