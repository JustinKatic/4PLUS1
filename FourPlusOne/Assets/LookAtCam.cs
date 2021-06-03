using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{

    public Transform cam; 
    private void OnEnable()
    {
        transform.LookAt(cam.position);
        Debug.Log(cam.position);
    }
}
