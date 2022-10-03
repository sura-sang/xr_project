using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public abstract class PuzzleObserver : MonoBehaviour
    {
        //상태 update 메서드
        public abstract void OnNotify();
    }
}
