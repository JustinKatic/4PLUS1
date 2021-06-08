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
    public GameObject[] comicTxtToSpawn;

    public GameEvent LhandSlapPerson;
    public GameEvent RhandSlapPerson;

    public GameEvent LhandSlapObject;
    public GameEvent RhandSlapObject;

    private bool hasSlappedObj = false;

    [HideInInspector]
    public Vector3 PreviousHitVelocity = new Vector3(0, 0, 0);

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

                if (hasSlappedObj == false)
                {
                    SpawnComicText(collision.contacts[0].point);
                }
                PreviousHitVelocity = collision.rigidbody.velocity.normalized;
                Debug.Log("HHJUSHJHSDGJH");
                OnSlap.Invoke();
                StartCoroutine(HasSlapped());
            }
        }
        else if (collision.relativeVelocity.magnitude >= SlapStrengthThreshold)
        {
            PreviousHitVelocity = collision.relativeVelocity.normalized;
            SpawnComicText(collision.contacts[0].point);
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
        GameObject spawnedObj = Instantiate(comicTxtToSpawn[Random.Range(0, comicTxtToSpawn.Length)], pos, Quaternion.identity);
        spawnedObj.transform.LookAt(cam.transform);
    }
    //  if (collision.relativeVelocity.magnitude != 0) Debug.Log("Relative Force: " + collision.relativeVelocity.magnitude.ToString() + ". " + gameObject.name);
}

