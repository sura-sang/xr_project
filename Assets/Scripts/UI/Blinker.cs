using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class Blinker : MonoBehaviour
    {
        [SerializeField] private GameObject _target;
        [SerializeField] private float _interval = 1;

        private void Awake()
        {
            StartCoroutine(Blink());
        }

        private IEnumerator Blink()
        {
            while (true)
            {
                _target?.SetActive(true);
                yield return new WaitForSeconds(_interval);

                _target?.SetActive(false);
                yield return new WaitForSeconds(_interval);
            }
        }
    }
}
