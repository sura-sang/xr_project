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

        private GameObject _Effectobj = null;

        public static bool IsOnCheckPoint = false;

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
                _Effectobj = Global.Instance.ResourceManager.GetObject(Constant.CheckPointEffect, transform);
                _Effectobj.transform.localRotation = Quaternion.Euler(-90, 0, 0);

                gameObject.GetComponentInChildren<Renderer>().material.color = Color.green;
                _isCurCheckPoint = true;
            }
            else
            {
                if (_Effectobj != null)
                {
                    Global.Instance.ResourceManager.ReturnObject(Constant.CheckPointEffect, _Effectobj);
                    _Effectobj = null;
                }
                gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
                _isCurCheckPoint = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if(!_isCurCheckPoint)
            {
                IsOnCheckPoint = true;
                Debug.Log("IsOnCheckPoint : " + IsOnCheckPoint);

                if (other.CompareTag("Player") && Global.Instance.SceneMaster.Player.IsCheckPointClick)
                {
                    OnCheckPointHit(gameObject.name);
                    Debug.Log("체크포인트 접촉");

                    Global.Instance.UIManager.Get<UISavingPopupModel>().Init();

                    AudioManager.Instance.SoundOneShot3D(AudioManager.Instance.SFX_OB_SavePoint, other.transform);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            IsOnCheckPoint = false;
            Debug.Log("IsOnCheckPoint : " + IsOnCheckPoint);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, CheckPointRadius);
        }
    }
}
