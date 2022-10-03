using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public interface IPuzzleSubject
    {
        void AddObserver(PuzzleObserver po);
        void RemoveObserver(PuzzleObserver po);
        void Notify(PuzzleObserver po);
    }
}
