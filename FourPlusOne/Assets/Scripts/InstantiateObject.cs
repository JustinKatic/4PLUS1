using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    public GameObject objToInstantiate;
    //public Transform transformToSpawnAt;

    public void SpawnObject()
    {
        Instantiate(objToInstantiate, transform.position, Quaternion.identity);
    }
}
