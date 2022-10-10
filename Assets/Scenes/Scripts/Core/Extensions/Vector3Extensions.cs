using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public static class Vector3Extentions
    {
        public static Vector3 InputToTransformSpace(Vector2 input, Transform transform)
        {
            var foward = transform.forward;
            var right = transform.right;
            foward.y = 0;
            right.y = 0;
            foward.Normalize();
            right.Normalize();

            var fowardReVerticalInput = input.y * foward;
            var rightRelHorizontalInput = input.x * right;

            return fowardReVerticalInput + rightRelHorizontalInput;
        }
    }
}
