using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnTrigger : MonoBehaviour
{
    public bool OnlyOnce = true;
    public UnityEvent OnEnterTrigger;
    public UnityEvent OnExitTrigger;

    public GameObject dialogueBox;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            OnEnterTrigger.Invoke();
            dialogueBox.transform.LookAt(other.transform);
            if (OnlyOnce) enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            OnExitTrigger.Invoke();
            //dialogueBox.transform.LookAt(other.transform);
            if (OnlyOnce) enabled = true;
        }
    }
}
