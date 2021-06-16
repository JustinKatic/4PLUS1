using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class AngryPerson : MonoBehaviour
{
    public Transform TargetTransform;
    private NavMeshAgent selfAgent;

    public UnityEvent WithTransform;

    // Start is called before the first frame update
    void Start()
    {
        selfAgent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        if (TargetTransform != null && selfAgent != null && selfAgent.isOnNavMesh)
        {
            selfAgent.destination = TargetTransform.position;

            if(Vector3.Distance(TargetTransform.position,transform.position) < 0.4f)
            {
                WithTransform.Invoke();
            }
        }
    }
}
