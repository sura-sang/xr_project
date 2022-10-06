using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class LevelMemento
    {
        public int LevelData { get; set; }
        public Vector3 TranformData { get; set; }

        public LevelMemento(int level, Vector3 transform)
        {
            this.LevelData = level;
            this.TranformData = transform;
        }
    }
}
