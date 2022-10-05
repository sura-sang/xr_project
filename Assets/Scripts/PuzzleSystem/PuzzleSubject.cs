using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class PuzzleSubject : MonoBehaviour, IPuzzleSubject
    {
        private List<PuzzleObserver> _puzzleObservers = new List<PuzzleObserver>();

        public void AddObserver(PuzzleObserver po)
        {
            if (!_puzzleObservers.Contains(po))
            {
                _puzzleObservers.Add(po);
            }
        }

        public void RemoveObserver(PuzzleObserver po)
        {
            if (_puzzleObservers.Contains(po))
            {
                _puzzleObservers.Remove(po);
            }
        }

        public void Notify(PuzzleObserver po)
        {
            foreach (PuzzleObserver pObs in _puzzleObservers)
            {
                if (pObs == po)
                {
                    pObs.OnNotify();
                }
            }
        }
    }
}
