using UnityEngine;
using UnityEngine.AI;

namespace SuraSang
{
    public class MoveShapeTrigger_BJY : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField] private LayerMask _interactionLayer;
        public NavMeshAgent DisableAgent;

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _interactionLayer) != 0)
            {
                EventManager.Instance.ShapeMove(_id);
                DisableAgent.enabled = false;
            }
        }

        /*private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & _interactionLayer) != 0)
            {
                EventManager.Instance.ShapeReturn(_id);
            }
        }
        */
    }
}
