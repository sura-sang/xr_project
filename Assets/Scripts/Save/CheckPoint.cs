using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class CheckPoint : MonoBehaviour
    {
        public int Level;
        public Vector3 Location;

        private void Start()
        {
            Location = transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                LevelMemento levelMemento = new(Level, Location);
                SaveManager.Instance.PushLevelData(levelMemento);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position, gameObject.GetComponent<BoxCollider>().size);
        }
    }
}
