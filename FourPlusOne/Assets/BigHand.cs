using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigHand : MonoBehaviour
{
    public GameObject rightHand;
    public GameObject leftHand;


    public void MakeRightHandBig()
    {
        rightHand.transform.localScale = new Vector3(-5, 5, 5);
    }

    public void MakeRightHandSmall()
    {
        rightHand.transform.localScale = new Vector3(-1, 1, 1);
    }
}
