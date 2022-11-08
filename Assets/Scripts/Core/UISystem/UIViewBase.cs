using UnityEngine;

namespace SuraSang
{
    public class UIViewBase : MonoBehaviour
    {
        [SerializeField] private bool usePooling = false;
        public bool UsePooling => usePooling;
    }
}