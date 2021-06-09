using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMiss : MonoBehaviour
{
    Rigidbody rigidbody;
    public float handVelocityThreshHold;

    float timer;
    public float timeBeforeMiss;
    public AudioSource audioSource;
    public AudioClip slapMiss;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rigidbody.velocity.magnitude >= handVelocityThreshHold)
        {
            timer += Time.deltaTime;
            if (timer >= timeBeforeMiss)
            {
                audioSource.PlayOneShot(slapMiss);
                timer = 0;
            }
        }
        else
            timer = 0;

    }

}
