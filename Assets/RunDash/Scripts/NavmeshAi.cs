using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshAi : MonoBehaviour
{
    public Transform goal;
    public NavMeshAgent navMesh;
    public static NavmeshAi instance;

    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        //NavMeshAgent agent = GetComponent<NavMeshAgent>();
        //agent.destination = goal.position;
        navMesh.SetDestination(goal.position);
    }
}
