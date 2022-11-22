using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class CheckPoint : MonoBehaviour
    {
        public float CheckPointRadius = 5f;
        public Transform SpawnPos;

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
                Global.Instance.SceneMaster.Player.ReturnEmotion();
                Global.Instance.SceneMaster.Player.ChangeCharacter();
                gameObject.GetComponentInChildren<Renderer>().material.color = Color.green;
                _isCurCheckPoint = true;
            }
            else
            {
                gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
                _isCurCheckPoint = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if(!_isCurCheckPoint)
            {
                if (other.CompareTag("Player") && Global.Instance.SceneMaster.Player.IsCheckPointClick)
                {
                    OnCheckPointHit(gameObject.name);
                    Debug.Log("체크포인트 접촉");

                    Global.Instance.UIManager.Get<UISavingPopupModel>().Init();
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
