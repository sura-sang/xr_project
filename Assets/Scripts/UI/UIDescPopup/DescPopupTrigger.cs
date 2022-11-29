using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class DescPopupTrigger : MonoBehaviour
    {
        [ReadOnly] private bool _isUsed = false;

        [SerializeField] private float _releaseTime;
        [SerializeField] private DescType _descType;

        [SerializeField] private DescType _descType2 = DescType.None;

        private void OnTriggerEnter(Collider other)
        {
            if (!_isUsed && other.CompareTag("Player"))
            {
                Global.Instance.UIManager.ReleaseAllPopups();
                Global.Instance.UIManager.Get<UIDescPopupModel>().Init(_descType, _releaseTime);

                if(_descType2 != DescType.None)
                {
                    StartCoroutine(DescSeq());
                }

                _isUsed = true;
            }
        }

        private IEnumerator DescSeq()
        {
            yield return new WaitForSeconds(_releaseTime);

            Global.Instance.UIManager.ReleaseAllPopups();
            Global.Instance.UIManager.Get<UIDescPopupModel>().Init(_descType2, _releaseTime);
        }
    }
}
