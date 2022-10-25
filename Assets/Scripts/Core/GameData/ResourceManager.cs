using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace SuraSang
{
    public class ResourceManager
    {
        // 그냥 로드

        private Dictionary<string, Object> _loadedObjects = new Dictionary<string, Object>();

        public T Load<T>(string path) where T : Object
        {
            if (_loadedObjects.TryGetValue(path, out var result))
            {
                return (T)result;
            }

            result = Resources.Load(path);

            if (result != null)
            {
                _loadedObjects.Add(path, result);
            }
            else
            {
                Debug.LogError($"{path} 를 로드할 수 없습니다.");
            }

            return (T)result;
        }


        // 오브젝트 풀

        private Dictionary<string, Queue<GameObject>> _objectPools = new Dictionary<string, Queue<GameObject>>();
        private Transform _poolTransform = null;

        private Transform GetPoolTransform()
        {
            if (_poolTransform == null)
            {
                _poolTransform = new GameObject("_pool").transform;
                _poolTransform.gameObject.SetActive(false);

                GameObject.DontDestroyOnLoad(_poolTransform);
            }
            return _poolTransform;
        }

        public GameObject GetObject(string path, Transform parent)
        {
            var obj = Load<GameObject>(path);

            if (obj == null)
            {
                return null;
            }

            if (!_objectPools.TryGetValue(path, out var queue))
            {
                queue = new Queue<GameObject>();
                _objectPools.Add(path, queue);
            }

            if (queue.Count <= 0)
            {
                queue.Enqueue(GameObject.Instantiate(obj, GetPoolTransform()));
            }

            var result = queue.Dequeue();

            result.transform.SetParent(parent);
            result.transform.localPosition = Vector3.zero;
            result.SetActive(true);

            return result;
        }

        public void ReturnObject(string path, GameObject obj)
        {
            if (!_objectPools.TryGetValue(path, out var queue))
            {
                Debug.LogError($"{obj.name}을 {path} 에 반환할 수 없습니다.");
                return;
            }

            obj.SetActive(false);
            obj.transform.SetParent(GetPoolTransform());
            queue.Enqueue(obj);
        }

        public async void ReturnObjectWithDelay(string path, GameObject obj, float second)
        {
            await Task.Delay((int)(second * 1000));

            ReturnObject(path, obj);
        }

        public void ReturnParticleSystem(string path, GameObject obj)
        {
            if (!obj.TryGetComponent<ParticleSystem>(out var particle))
            {
                ReturnObject(path, obj);
                return;
            }
            ReturnObjectWithDelay(path, obj, particle.main.duration);
        }
    }
}
