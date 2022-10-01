using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public interface ISkill
    {
        abstract void OnMove(Vector2 input);
        abstract void OnSkill();
        abstract void Animation();
    }
}
