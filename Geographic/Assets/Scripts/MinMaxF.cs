using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMaxF
{
    public float Min { get; private set; }
    public float Max { get; private set; }

    public MinMaxF()
    {
        Max = float.MinValue;
        Min = float.MaxValue;
    }

    public void AddValue(float value)
    {
        if (value > Max)
            Max = value;

        if (value < Min)
            Min = value;
    }
}
