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


    private Camera cam;
    public GameObject comicTxtToSpawn;
    private bool hasSpawned = false;

    public GameEvent LhandSlapPerson;
    public GameEvent RhandSlapPerson;

    public GameEvent LhandSlapObject;
    public GameEvent RhandSlapObject;

    private bool hasSlappedObj = false;


    private void Start()
    {
        cam = Camera.main;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasSlappedObj)
            return;

        if (collision.rigidbody != null)
        {
            if (collision.rigidbody.velocity.magnitude >= SlapStrengthThreshold)
            {
                Debug.Log(collision.gameObject.name);
                if (collision.gameObject.name == "RightHand")
                {
                    controllerR.SendHapticImpulse(hapticImpulseOnSlap, hapticDuration);
                    if (gameObject.tag == "Person")
                    {
                        RhandSlapPerson.Raise();
                    }
                    if (gameObject.tag == "Object")
                    {
                        RhandSlapObject.Raise();
                    }
                }
                else if (collision.gameObject.name == "LeftHand")
                {
                    controllerL.SendHapticImpulse(hapticImpulseOnSlap, hapticDuration);
                    if (gameObject.tag == "Person")
                    {
                        LhandSlapPerson.Raise();
                    }
                    if (gameObject.tag == "Object")
                    {
                        LhandSlapObject.Raise();
                    }
                }
                OnSlap.Invoke();
                StartCoroutine(HasSlapped());

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

    IEnumerator HasSlapped()
    {
        hasSlappedObj = true;
        yield return new WaitForSeconds(0.5f);
        hasSlappedObj = false;

    }

    public void SpawnComicText(Transform pos)
    {
        comicTxtToSpawn = Instantiate(comicTxtToSpawn, pos.position, Quaternion.identity);
        comicTxtToSpawn.transform.LookAt(cam.transform);

    }
    //  if (collision.relativeVelocity.magnitude != 0) Debug.Log("Relative Force: " + collision.relativeVelocity.magnitude.ToString() + ". " + gameObject.name);
}

