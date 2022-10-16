using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class CheckPoint : MonoBehaviour
    {
        public float CheckPointRadius = 5f;

        private SphereCollider _cpRaius;
        private bool _isCurCheckPoint;

        public delegate void UpdateCheckpointDel(string cpName);
        public static event UpdateCheckpointDel OnCheckPointHit;

        private void OnEnable()
        {
            OnCheckPointHit += UpdateCheckpoint;
        }

        private void OnDisable()
        {
            OnCheckPointHit -= UpdateCheckpoint;
        }

        private void Awake()
        {
            _cpRaius = GetComponent<SphereCollider>();
            _cpRaius.radius = CheckPointRadius;
        }

        public void UpdateCheckpoint(string cpName)
        {
            if (gameObject.name == cpName)
            {
                SceneMaster.SceneInstance.CurrentCheckPoint = this;
                GameManager.Instance.Player.ReturnEmotion();
                GameManager.Instance.Player.ChangeCharacter();
                gameObject.GetComponent<Renderer>().material.color = Color.green;
                _isCurCheckPoint = true;
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                _isCurCheckPoint = false;
            }
        }

        public Vector3 SpawnPosition()
        {
            var spawnPos = new Vector3(transform.position.x - 2f, transform.position.y + 1f, transform.position.z);
            return spawnPos;
        }

        private void OnTriggerStay(Collider other)
        {
            if(!_isCurCheckPoint)
            {
                if (other.CompareTag("Player") && GameManager.Instance.Player.IsCheckPointClick)
                {
                    OnCheckPointHit(gameObject.name);
                    Debug.Log("체크포인트 접촉");
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, CheckPointRadius);
        }
    }
}
