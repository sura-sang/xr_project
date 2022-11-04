using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace SuraSang
{
    //public abstract class SOData : ScriptableObject
    //{
    //    public string DataName
    //}

    public class SODataManager
    {
        private const string SODataPath = "SO/";

        private Dictionary<Type, ScriptableObject> _datas = new Dictionary<Type, ScriptableObject>();

        public SODataManager()
        {
            LoadDatas();
        }

        public void LoadDatas()
        {
            _datas.Clear();

            var datas = Resources.LoadAll<ScriptableObject>(SODataPath);
            foreach (var data in datas)
            {
                _datas.TryAdd(data.GetType(), data);
            }
        }

        public T GetData<T>() where T : ScriptableObject
        {
            if(!_datas.TryGetValue(typeof(T), out var data))
            {
                Debug.LogError($"{typeof(T)} 데이터를 찾을 수 없습니다");
                return null;
            }
            return data as T;
        }
    }
}
