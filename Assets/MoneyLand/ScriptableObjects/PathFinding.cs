using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PathFinding : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    public List<Transform> points = new List<Transform>();
    private int destPoint;
    public List<Transform> collectingAreas = new List<Transform>();
    bool set;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        points.Add(collectingAreas[Random.Range(0, collectingAreas.Count - 1)]);
        points.Add(AiBuying.instance.transform);
    }
    private void Update()
    {
        Animations();
    }
    private void FixedUpdate()
    {
        SetTargetToCollect();
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            // SettingPoints();
            GoToNextPoint();
        }
    }
    public void GoToNextPoint()
    {
        navMeshAgent.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Count;
    }
    public void Animations()
    {
        if (navMeshAgent.remainingDistance < 0.15f)
        {
            anim.SetBool("Walk", false);
        }
        else
        {
            anim.SetBool("Walk", true);
        }
    }
    public void SettingPoints()
    {
        if (!set)
        {
            points[0] = collectingAreas[Random.Range(0, collectingAreas.Count - 1)];
            points[1] = AiBuying.instance.transform;
            set = true;
        }
    }
    public void SetTargetToCollect()
    {
        CollectableArea[] collectItems = FindObjectsOfType<CollectableArea>();
        foreach (CollectableArea obj in collectItems)
        {
            if (obj.gameObject.activeInHierarchy)
            {
                if (!collectingAreas.Contains(obj.transform))
                    collectingAreas.Add(obj.transform);
            }
        }
    }
}
