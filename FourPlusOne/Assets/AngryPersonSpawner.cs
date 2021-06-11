using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryPersonSpawner : MonoBehaviour
{
    public Transform Target;
    public GameObject AngryPersonPrefab;

    private void Start()
    {
        AngryPerson angry = AngryPersonPrefab.GetComponent<AngryPerson>();
        if (angry != null) angry.TargetTransform = Target;
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0f,1f) > 0.99f)
        {
            //SpawnPrefab
            Instantiate(AngryPersonPrefab,transform.position + new Vector3(Random.Range(-2,2),0, Random.Range(-2, 2)),Quaternion.identity);
        }
    }
}
