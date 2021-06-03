using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonLines : MonoBehaviour
{
    public LineRenderer balloon1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        balloon1.SetPosition(0, gameObject.transform.position);
        balloon1.SetPosition(0, balloon1.transform.position);

    }
}
