using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public abstract class Stat<T>
    {
        public Stat(T value)
        {
            BaseValue = value;
            Value = value;
        }

        public T BaseValue { get; protected set; }
        public T Value { get; protected set; }

        public abstract void AddValue(T value);
        public abstract void MulValue(T value);

        public void ResetValue()
        {
            Value = BaseValue;
        }
    }

    public class IntStat : Stat<int>
    {
        public IntStat(int value) : base(value) { }

        public override void AddValue(int value) => Value += value;
        public override void MulValue(int value) => Value *= value;
    }

    public class FloatStat : Stat<float>
    {
        public FloatStat(float value) : base(value) { }

        public override void AddValue(float value) => Value += value;
        public override void MulValue(float value)
        {
            Debug.LogError(Value);
            Value *= value;

            Debug.LogError(Value);
        }
    }
}
