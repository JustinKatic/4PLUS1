using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollow : MonoBehaviour
{
    public Transform objToFollow;
    Transform thisTransform;

    private void Start()
    {
        thisTransform = gameObject.transform;
    }
    private void FixedUpdate()
    {
        thisTransform.position = objToFollow.position;
    }
}
