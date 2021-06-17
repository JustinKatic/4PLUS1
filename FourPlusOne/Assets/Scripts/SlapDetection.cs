using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SlapDetection : MonoBehaviour
{
    private ActionBasedController controllerL;
    private ActionBasedController controllerR;

    [Range(0, 1)]
    [SerializeField] private float hapticImpulseOnSlap;
    [SerializeField] private float hapticDuration;

    [Min(0)]
    [Tooltip("The velocity the gameobject needs to be hit by to Invoke OnSlap.")]
    public float SlapStrengthThreshold = 5;
    public bool ShouldSpawnComicText = true;
    public UnityEvent OnSlap;

    public bool OnlyHands = false; 

    private Camera cam;
    public SlapDetectionData SlapData;

    private bool hasSlappedObj = false;

    [HideInInspector]
    public Vector3 PreviousHitVelocity = new Vector3(0, 0, 0);

    private void Start()
    {
        cam = Camera.main;

        // Find L and R controllers in scene
        controllerL = GameObject.FindGameObjectWithTag("LHand").GetComponent<ActionBasedController>();
        controllerR = GameObject.FindGameObjectWithTag("RHand").GetComponent<ActionBasedController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasSlappedObj || collision.gameObject.tag == "3DLetter")
            return;

        if (collision.rigidbody != null)
        {
            if (collision.rigidbody.velocity.magnitude >= SlapStrengthThreshold)
            {
                bool isHand = false;
                //HIT RIGHT HAND
                if (collision.gameObject.name == "RightHand")
                {
                    isHand = true;
                    //Vibrate
                    if (controllerR != null) controllerR.SendHapticImpulse(hapticImpulseOnSlap, hapticDuration);

                    //Raise sound events
                    if (gameObject.tag == "Person")
                    {
                        SlapData.RhandSlapPerson.Raise();
                    }
                    else if (gameObject.tag == "Object")
                    {
                        SlapData.RhandSlapObject.Raise();
                    }
                    else if (gameObject.tag == "Balloon")
                    {
                        SlapData.RhandSlapBalloon.Raise();
                    }

                    //Spawn effect object
                    if (hasSlappedObj == false)
                    {
                        SpawnComicText(collision.contacts[0].point);
                    }
                }

                //HIT LEFT HAND
                else if (collision.gameObject.name == "LeftHand")
                {
                    isHand = true;
                    //Vibrate
                    if (controllerL != null) controllerL.SendHapticImpulse(hapticImpulseOnSlap, hapticDuration);

                    //Raise sound events
                    if (gameObject.tag == "Person")
                    {
                        SlapData.LhandSlapPerson.Raise();
                    }
                    else if (gameObject.tag == "Object")
                    {
                        SlapData.LhandSlapObject.Raise();
                    }
                    else if (gameObject.tag == "Balloon")
                    {
                        SlapData.LhandSlapBalloon.Raise();
                    }

                    //Spawn effect object
                    if (hasSlappedObj == false)
                    {
                        SpawnComicText(collision.contacts[0].point);
                    }
                }

                if (OnlyHands && isHand)
                {
                    PreviousHitVelocity = collision.rigidbody.velocity;
                    OnSlap.Invoke();
                    StartCoroutine(HasSlapped());
                }
                else if(!OnlyHands)
                {
                    PreviousHitVelocity = collision.rigidbody.velocity;
                    OnSlap.Invoke();
                    StartCoroutine(HasSlapped());
                }
            }
            else if (collision.relativeVelocity.magnitude <= SlapStrengthThreshold)
            {

                //HIT RIGHT HAND
                if (collision.gameObject.name == "RightHand")
                {
                    //Vibrate
                    if (controllerR != null) controllerR.SendHapticImpulse(hapticImpulseOnSlap / 2, hapticDuration / 2);

                    //Raise sound events
                    if (gameObject.tag == "Balloon")
                    {
                        SlapData.RhandPokeBalloon.Raise();
                    }
                }
                //HIT LEFT HAND
                else if (collision.gameObject.name == "LeftHand")
                {
                    //Vibrate
                    if (controllerL != null) controllerL.SendHapticImpulse(hapticImpulseOnSlap / 2, hapticDuration / 2);

                    //Raise sound events
                    if (gameObject.tag == "Balloon")
                    {
                        SlapData.LhandPokeBalloon.Raise();
                    }
                }
            }
        }
        else if (collision.relativeVelocity.magnitude >= SlapStrengthThreshold && !OnlyHands)
        {
            PreviousHitVelocity = collision.relativeVelocity;
            OnSlap.Invoke();
        }     
    }

    IEnumerator HasSlapped()
    {
        hasSlappedObj = true;
        yield return new WaitForSeconds(0.5f);
        hasSlappedObj = false;

    }

    public void SpawnComicText(Vector3 pos)
    {
        if (!ShouldSpawnComicText) return;
        GameObject spawnedObj = Instantiate(SlapData.comicTxtToSpawn[Random.Range(0, SlapData.comicTxtToSpawn.Length)], pos, Quaternion.identity);
        spawnedObj.transform.LookAt(cam.transform);
    }
    //  if (collision.relativeVelocity.magnitude != 0) Debug.Log("Relative Force: " + collision.relativeVelocity.magnitude.ToString() + ". " + gameObject.name);
}

