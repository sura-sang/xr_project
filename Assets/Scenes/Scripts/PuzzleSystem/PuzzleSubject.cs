using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class PuzzleSubject : MonoBehaviour, IPuzzleSubject
    {
        private List<PuzzleElements> _puzzleObservers = new List<PuzzleElements>();

        public void AddObserver(PuzzleElements po)
        {
            if (!_puzzleObservers.Contains(po))
            {
                _puzzleObservers.Add(po);
            }
        }

        public void RemoveObserver(PuzzleElements po)
        {
            if (_puzzleObservers.Contains(po))
            {
                _puzzleObservers.Remove(po);
            }
        }

        public void Notify(PuzzleElements pe, PuzzleContext context)
        {
            foreach (PuzzleElements pObs in _puzzleObservers)
            {
                if (pObs == pe)
                {
                    pObs.OnNotify(context);
                }
            }
        }
    }
}
