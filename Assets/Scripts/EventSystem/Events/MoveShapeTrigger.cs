using UnityEngine;

namespace SuraSang
{
    public class MoveShapeTrigger : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField] private LayerMask _interactionLayer;

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _interactionLayer) != 0)
            {
                EventManager.Instance.ShapeMove(_id);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & _interactionLayer) != 0)
            {
                EventManager.Instance.SharpReturn(_id);
            }
        }
    }
}
