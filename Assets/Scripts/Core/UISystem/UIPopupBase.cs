using UnityEngine;

namespace SuraSang
{
    public class UIPopupBase : UIViewBase
    {
        [SerializeField] private bool useBlackMargin = false;
        public bool UseBlackMargin => useBlackMargin;
    }
}