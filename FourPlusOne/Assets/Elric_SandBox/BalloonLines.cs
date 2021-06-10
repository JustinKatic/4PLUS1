using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonLines : MonoBehaviour
{
    LineRenderer line;
    public Transform knot;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.parent != null)
        {
            line.SetPosition(0, knot.position);
            line.SetPosition(1, transform.parent.position);
        }
    }
    private void OnJointBreak(float breakForce)
    {
        line.enabled = false;
        Destroy(gameObject, 20);
        transform.parent = null;
    }
}
