using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class movePlatform : MonoBehaviour
    {
        [SerializeField] private Vector3 _movePoint;
        [SerializeField] private int _id;
        [SerializeField] private int _speed;
        [SerializeField] private GameObject[] _jumpMap;

        private Vector3 _firstPos;
        private Vector3 _lastPos;

        private bool _isHappy = false;
        private bool _isSad = false;
        private bool _isAnger = false;

        private void Start()
        {
            EventManager.Instance.PlatformMoveAction += PlatformMove;

            _firstPos = transform.position;
            _lastPos = transform.position + _movePoint;
        }

        private void OnDisable()
        {
            EventManager.Instance.PlatformMoveAction -= PlatformMove;
        }

        private void Update()
        {
            if (_isHappy == true)
            {
                //0,1
                transform.position = Vector3.MoveTowards(transform.position, _lastPos, _speed * Time.deltaTime);
            }
            else if(_isSad == true)
            {
                //2,3
                transform.position = Vector3.MoveTowards(transform.position, _lastPos, _speed * Time.deltaTime);
            }
            else if(_isAnger == true)
            {
                //4,5,6
                transform.position = Vector3.MoveTowards(transform.position, _lastPos, _speed * Time.deltaTime);
            }
        }

        private void PlatformMove(int id)
        {
            if (id == this._id && EventManager.Instance.PEmotion == Emotion.Happiness)
            {
                _isHappy = true;
            }
            else if (id == this._id && EventManager.Instance.PEmotion == Emotion.Sadness)
            {
                _isSad = true;
            }
            else if (id == this._id && EventManager.Instance.PEmotion == Emotion.Anger)
            {
                _isAnger = true;
            }
        }
    }
}
