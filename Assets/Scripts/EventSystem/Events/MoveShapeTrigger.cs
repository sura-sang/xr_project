using UnityEngine;

namespace SuraSang
{
    public class MoveShapeTrigger : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField] private LayerMask _interactionLayer;

        [Header("Sound Play")]
        [SerializeField] private bool _enter;
        [SerializeField] private bool _exit;

        [SerializeField] private int _count = 0;
        [SerializeField] private int _currentCount = 0;

        private void Start()
        {
            for (int i = 0; i < 32; i++)
            {
                if (_interactionLayer == (_interactionLayer | (1 << i)))
                {
                    _count++;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _interactionLayer) != 0)
            {
                _currentCount++;

                if (_currentCount == 1)
                {
                    EventManager.Instance.ShapeMove(_id);

                    if (_enter)
                    {
                        AudioManager.Instance.SoundOneShot3D(AudioManager.Instance.SFX_OB_Pad, gameObject.transform);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & _interactionLayer) != 0)
            {
                _currentCount--;

                if (_currentCount == 0)
                {
                    EventManager.Instance.ShapeReturn(_id);

                    if (_exit)
                    {
                        AudioManager.Instance.SoundOneShot3D(AudioManager.Instance.SFX_OB_Pad2, gameObject.transform);
                    }
                }
            }
        }
    }
}
