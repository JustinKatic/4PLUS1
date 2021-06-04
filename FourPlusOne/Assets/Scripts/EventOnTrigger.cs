using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnTrigger : MonoBehaviour
{
    public bool OnlyOnce = true;
    public UnityEvent OnEnterTrigger;

    private void OnTriggerEnter(Collider other)
    {
        OnEnterTrigger.Invoke();
        if (OnlyOnce) enabled = false;
    }
}
