using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NpcSlap : MonoBehaviour
{
    public UnityEvent NPCKilled;

    public List<Rigidbody> BodyParts;
    public Animator Animator;

    private void Start()
    {
        for (int i = 0; i < BodyParts.Count; i++)
        {
            BodyParts[i].isKinematic = true;
            BodyParts[i].useGravity = false;
        }
    }

    public void WasSlapped(SlapDetection slap)
    {
        Animator.enabled = false;
        for (int i = 0; i < BodyParts.Count; i++)
        {
            BodyParts[i].isKinematic = false;
            BodyParts[i].useGravity = true;
            BodyParts[i].AddForce(Vector3.up * 10);
        }
        NPCKilled.Invoke();
    }
}
