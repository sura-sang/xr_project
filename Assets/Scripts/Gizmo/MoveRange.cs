using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class MoveRange : MonoBehaviour
    {
        public float MoveRadius = 5f;

        private void Start()
        {
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(transform.position, MoveRadius * 2);
        }
    }
}
