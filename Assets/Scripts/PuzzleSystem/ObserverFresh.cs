using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class ObserverFresh : PuzzleObserver
    {
        private void Start()
        {
            PuzzleManager.Instance.AddObserver(this);
        }

        public override void OnNotify()
        {
            Debug.Log("갓버섯 퍼즐 실행");
        }
    }
}
