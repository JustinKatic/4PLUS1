using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthTest : MonoBehaviour
{
    [Tooltip("The thing that goes up when hit, i don't know the propper name!")]
    public Rigidbody Indecator;
    public float StrengthScale = 1;

    public void OnPressurePlateSlapped(SlapDetection slap)
    {
        if(Indecator.IsSleeping())
        {
            Indecator.AddForce(Vector3.up * slap.PreviousHitVelocity.magnitude * StrengthScale, ForceMode.Impulse);
            Debug.Log(slap.PreviousHitVelocity.magnitude.ToString());
        }
    }
}
