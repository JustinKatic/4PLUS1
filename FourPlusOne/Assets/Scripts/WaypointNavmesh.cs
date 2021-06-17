using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WaypointNavmesh : MonoBehaviour
{
    public List<Vector3> WayPoints = new List<Vector3>();

    private NavMeshAgent navAgent;
    private int currentPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();

        GoToNextPoint();
    }

    private void GoToNextPoint()
    {
        if (WayPoints.Count == 0) return;

        navAgent.SetDestination(WayPoints[currentPoint]);

        currentPoint = ++currentPoint % WayPoints.Count;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!navAgent.pathPending && navAgent.remainingDistance < 0.25f)
        {
            GoToNextPoint();
        }
    }
}
