using UnityEngine;


//[CreateAssetMenu(fileName = "STFSettings", menuName = "SnapToFloor/Create STFSettings", order = Int32.MaxValue)]
namespace SnapToFloor
{
    public class STFSettings : ScriptableObject
    {
        [HideInInspector]
        public int StartAtShow = 0;

        [HideInInspector]
        public int SnapMode = -1;
    }
}