using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathMover : MonoBehaviour
{
    private NavMeshAgent navmeshagent;
    private Queue<Vector3> pathPoints = new Queue<Vector3>();

    private void Awake()
    {
        navmeshagent = GetComponent<NavMeshAgent>();
        FindObjectOfType<PathCreatorLine>().OnNewPathCreated += SetPoints;
    }

    private void SetPoints(IEnumerable<Vector3> points)
    {
        pathPoints = new Queue<Vector3>(points);
    }

    private void Update()
    {
        UpdatePathing();
    }
    private void UpdatePathing()
    {
        if (ShouldSetDestination())
            navmeshagent.SetDestination(pathPoints.Dequeue());
    }

    private bool ShouldSetDestination()
    {
        if (pathPoints.Count == 0)
            return false;

        if (navmeshagent.hasPath == false || navmeshagent.remainingDistance < 0.5f)
            return true;

        return false;
    }
}
