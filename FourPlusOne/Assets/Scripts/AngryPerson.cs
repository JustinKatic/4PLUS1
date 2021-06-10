using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AngryPerson : MonoBehaviour
{
    public Transform TargetTransform;

    private NavMeshAgent selfAgent;

    // Start is called before the first frame update
    void Start()
    {
        selfAgent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        if(TargetTransform != null && selfAgent != null && selfAgent.isOnNavMesh)
        selfAgent.destination = TargetTransform.position;
    }
}
