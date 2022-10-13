using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class StealNRun_PoliceMan : MonoBehaviour
{
    public float speed;
    public bool followNow;
    public GameObject baton;
    public Transform target;
    public Collider myCollider;
    public Animator myAnimator;
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Attack = Animator.StringToHash("Attack");


    private void Awake()
    {
        myCollider = GetComponent<Collider>();
        myAnimator = GetComponent<Animator>();
    }

    public void DeactiveMe()
    {
        followNow = false;
        gameObject.SetActive(false);
    }


    private void LateUpdate()
    {
        if(!followNow) return;

        if ( GetDistance(transform.position, target.position) > 0.2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position - new Vector3(0.25f,0,0), speed * Time.deltaTime);
        }
        
        if ( GetDistance(transform.position, target.position) <= 0.7f &&
                  GetDistance(transform.position, target.position) > 0.1f)
        {
            followNow = false;
            myCollider.enabled = false;
            myAnimator.SetTrigger(Attack);
            StealNRun_PlayerController.instance.PlayerCaught(transform.position.z - 0.5f);
            StealNRun_Manager.instance.PoliceMenAttackPlayer();
        }

        RotateToWardsPlayer();
    }

    public void AttackPlayer()
    {
        if(!followNow) return;
        followNow = false;
        myAnimator.SetTrigger(Attack);
        transform.DOMoveZ(StealNRun_PlayerController.instance.transform.position.z - 1f, 0.2f).SetEase(Ease.Flash);
        var transform1 = transform;
        Vector3 dir = target.position - transform1.position;
        transform.rotation = Quaternion.LookRotation(-dir.normalized);
    }

    private void ChangeState()
    {
        followNow = true;
        myCollider.enabled = false;
        myAnimator.SetTrigger(Run);
    }

    float GetDistance(Vector3 pos1, Vector3 pos2) => Vector3.Distance(pos1, pos2);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeState();
        }
    }

    private void RotateToWardsPlayer()
    {
        var transform1 = transform;
        Vector3 dir = target.position - transform1.position;
        transform.rotation = Quaternion.RotateTowards(transform1.rotation, Quaternion.LookRotation(dir.normalized),
            250 * Time.deltaTime);
    }
}
