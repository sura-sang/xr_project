using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class PuzzleContext
    {
        
    }
    
    public abstract class PuzzleElements : MonoBehaviour
    {
        //상태 update 메서드
        public abstract void OnNotify(PuzzleContext context);
    }
}
