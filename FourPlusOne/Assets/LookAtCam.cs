using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{

    public Transform cam; 
    private void OnEnable()
    {
        transform.LookAt(Camera.main.transform);
        Debug.Log(cam.position);
    }
}
