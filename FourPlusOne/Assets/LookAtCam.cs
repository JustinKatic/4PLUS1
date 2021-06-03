using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{

    private void OnEnable()
    {
        transform.LookAt(Camera.main.transform);
    }
}
