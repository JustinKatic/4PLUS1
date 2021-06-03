using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlapDetection : MonoBehaviour
{
    [Min(0)]
    [Tooltip("The velocity the gameobject needs to be hit by to Invoke OnSlap.")]
    public float SlapStrengthThreshold = 5;
    public UnityEvent OnSlap;



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            Debug.Log(collision.rigidbody.velocity.magnitude);
            if (collision.rigidbody.velocity.magnitude >= SlapStrengthThreshold)
            {
                OnSlap.Invoke();
            }
        }

        //  if (collision.relativeVelocity.magnitude != 0) Debug.Log("Relative Force: " + collision.relativeVelocity.magnitude.ToString() + ". " + gameObject.name);
    }
}
