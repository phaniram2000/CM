using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class RobotMovement : MonoBehaviour
{
    public enum State { collecting, selling };
    public State myState;
    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    [HideInInspector]
    public Animator anim;
    public Transform target;
    public List<Transform> collectingAreas = new List<Transform>();
    public Transform sellingTransform;
    public float speed;
    //public bool canCollect, canSell,collected,sold;
    public bool once;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        speed = navMeshAgent.speed;
        sellingTransform = AiBuying.instance.transform;
    }
    void Update()
    {
        switch (myState)
        {
            case State.collecting:
                Collection();
                //CheckCollected();
                break;
            case State.selling:
                Selling();
                myState = State.collecting;
                break;
        }
        if (target != null)
            navMeshAgent.destination = target.position;
        Animations();
    }
    public void Animations()
    {
        if (target != null)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }
    }
    public void Collection()
    {
        SetTargetToCollect();
        if (!once)
        {
            int num = Random.Range(0, collectingAreas.Count);
            target = collectingAreas[num];
            once = true;
        }
    }
    public void CheckCollected()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.01f)
        {
            myState = State.selling;
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
    public void Selling()
    {
        target = sellingTransform;
    }
    public void CheckReachedTarget(State my)
    {
        if (target != null)
        {
            if (my == State.collecting)
            {
                if (navMeshAgent.destination == target.position)
                {
                    navMeshAgent.ResetPath();
                    anim.SetBool("Walk", false);
                    target = null;
                    my = State.selling;
                }
            }
            if (my == State.selling)
            {
                if (navMeshAgent.destination == target.position)
                {
                    navMeshAgent.ResetPath();
                    anim.SetBool("Walk", false);
                    target = null;
                    my = State.collecting;
                }
            }
        }

    }

}
//if (myState == State.collecting)
//{
//    if (target == null && !canCollect)
//    {
//        SetTargetToCollect();
//        target = collectingAreas[Random.Range(0, collectingAreas.Count)];
//        canCollect = true;
//    }
//    if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f && canCollect && !collected)
//    {

//        Debug.Log("c");
//        target = null;
//        collected = true;
//        canSell = false;
//        sold = false;
//        myState = State.selling;
//    }
//}
//if (myState == State.selling)
//{
//    if (target == null && !canSell)
//    {
//        Debug.Log("TNull");
//        target = sellingTransform;
//        canSell = true;
//    }
//    if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f && canSell && !sold)
//    {
//        Debug.Log("s");
//        canCollect = false;
//        collected = false;
//        target = null;
//        sold = true;
//        myState = State.collecting;

//    }
//}