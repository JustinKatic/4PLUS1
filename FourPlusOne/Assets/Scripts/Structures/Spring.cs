using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Springs a float value between two values
/// </summary>
public struct Spring
{
    public float Value;
    private float velocity;

    public Spring(float initalValue)
    {
        Value = initalValue;
        velocity = 0;
    }

    public float UpdateSpring(float targetValue, float stiffness, float damping, float valueThresh, float velocityThresh)
    {
        float dampfactor = Mathf.Max(0, 1 - damping * Time.fixedDeltaTime);
        float accel = (targetValue - Value) * stiffness * Time.fixedDeltaTime;
        velocity = velocity * dampfactor + accel;
        Value += velocity * Time.deltaTime;

        if(Mathf.Abs(Value - targetValue) < valueThresh &&
            Mathf.Abs(velocity) < velocityThresh)
        {
            Value = targetValue;
            velocity = 0f;
        }

        return Value;
    }

}
