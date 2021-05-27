﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class SlapDetection : MonoBehaviour
{
    [Min(0)]
    [Tooltip("The velocity the gameobject needs to be hit by to Invoke OnSlap.")]
    public float SlapStrengthThreshold = 5;
    public UnityEvent OnSlap;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude >= SlapStrengthThreshold)
        {
            OnSlap.Invoke();
        }

        Debug.Log("Relative Force: " + collision.relativeVelocity.magnitude.ToString() + ". " + gameObject.name);
    }


}
