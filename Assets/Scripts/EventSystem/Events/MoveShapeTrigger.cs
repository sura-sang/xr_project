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

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _interactionLayer) != 0)
            {
                EventManager.Instance.ShapeMove(_id);

                if (_enter)
                {
                    AudioManager.Instance.SoundOneShot3D(AudioManager.Instance.SFX_OB_Pad, gameObject.transform);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & _interactionLayer) != 0)
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
