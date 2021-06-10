using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BalloonGun : MonoBehaviour
{
    public Transform GunFront;
    public GameObject BalloonPrefab;
    public GameObject HandPrefab;

    public void ShootGun()
    {
        if(Random.Range(0,5) >= 4)
        {
            //Spawn Balloon
            if (BalloonPrefab != null)
            {
                Instantiate(BalloonPrefab, GunFront.position + (GunFront.forward * 0.1f), GunFront.rotation);
            }
        }
        else
        {
            //Spawn Hand

            if(HandPrefab != null)
            {
                Instantiate(HandPrefab, GunFront.position + (GunFront.forward * 0.1f), GunFront.rotation);
            }
        }
    }
}
