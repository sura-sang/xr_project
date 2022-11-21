using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class MovePlatformTriggerAnger : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField] private LayerMask _interactionLayer;

        private Emotion _playerEmotion;

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _interactionLayer) != 0)
            {
                if (other.GetComponent<Player>().CurrentEmotion == Emotion.Anger)
                {
                    EventManager.Instance.PEmotion = other.GetComponent<Player>().CurrentEmotion;
                    EventManager.Instance.PlatformMove(_id);
                }
            }
        }
    }
}
