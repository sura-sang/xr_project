using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class ObserverOyster : PuzzleObserver
    {
        private void Start()
        {
            PuzzleManager.Instance.AddObserver(this);
        }

        public override void OnNotify()
        {
            Debug.Log("새송이 버섯 퍼즐 실행");            
        }
    }
}
