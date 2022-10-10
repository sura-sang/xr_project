using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public interface ISkill
    {
        abstract bool IsStopAble { get; }
        abstract void InitializeSkill();
        abstract void OnMove(Vector2 input);
        abstract void UpdateSkill();
        abstract void ClearSkill();
    }
}