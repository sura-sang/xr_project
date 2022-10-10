using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public interface IPuzzleSubject
    {
        void AddObserver(PuzzleElements po);
        void RemoveObserver(PuzzleElements po);
        void Notify(PuzzleElements po, PuzzleContext context);
    }
}
