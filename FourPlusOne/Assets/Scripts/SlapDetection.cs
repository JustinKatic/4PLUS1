﻿using System.Collections;
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


    private Camera cam;
    public GameObject comicTxtToSpawn;
    private bool hasSpawned = false;

    public Transform objToLookAt; 



    private void Start()
    {
        cam = Camera.main;
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

                if (hasSpawned == false)
                {
                    SpawnComicText(collision.transform);
                    hasSpawned = true;
                }
            }
        }
        else if (collision.relativeVelocity.magnitude >= SlapStrengthThreshold)
        {
            SpawnComicText(transform);
            OnSlap.Invoke();
        }
    }

    public void SpawnComicText(Transform pos)
    {
        comicTxtToSpawn = Instantiate(comicTxtToSpawn, pos.position, Quaternion.identity);
        comicTxtToSpawn.transform.LookAt(objToLookAt);

    }
    //  if (collision.relativeVelocity.magnitude != 0) Debug.Log("Relative Force: " + collision.relativeVelocity.magnitude.ToString() + ". " + gameObject.name);
}

