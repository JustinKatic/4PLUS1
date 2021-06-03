using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class SlapDetection : MonoBehaviour
{
    public ActionBasedController controllerL;
    public ActionBasedController controllerR;






    [Range(0, 1)]
    [SerializeField] private float hapticImpulseOnSlap;
    [SerializeField] private float hapticDuration;


    [Min(0)]
    [Tooltip("The velocity the gameobject needs to be hit by to Invoke OnSlap.")]
    public float SlapStrengthThreshold = 5;
    public UnityEvent OnSlap;

    private void Start()
    {
        //controllerL = GameObject.FindGameObjectWithTag("LHand").GetComponent<Hand>().controller;
        //controllerR = GameObject.FindGameObjectWithTag("RHand").GetComponent<Hand>().controller;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            if (collision.rigidbody.velocity.magnitude >= SlapStrengthThreshold)
            {
                Debug.Log(collision.gameObject.name);
                if (collision.gameObject.name == "RightHand")
                {
                    controllerR.SendHapticImpulse(hapticImpulseOnSlap, hapticDuration);
                }
                else if (collision.gameObject.name == "LeftHand")
                {
                    controllerL.SendHapticImpulse(hapticImpulseOnSlap, hapticDuration);
                }
                OnSlap.Invoke();
            }
        }
    }

    //  if (collision.relativeVelocity.magnitude != 0) Debug.Log("Relative Force: " + collision.relativeVelocity.magnitude.ToString() + ". " + gameObject.name);
}

