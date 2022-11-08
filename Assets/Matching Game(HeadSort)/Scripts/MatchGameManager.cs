using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class MatchGameManager : MonoBehaviour
{
    public Animator karenAnim;
    public NavMeshAgent karenNavmesh;
    public Transform startPoint;
    public Transform door;
    public Transform suitcase;
    public Transform openSuitcase;
    public Transform cam;
    public List<GameObject> match;
    public bool karenMove;
    private static readonly int Walk = Animator.StringToHash("Walk");

    private void OnEnable()
    {
        GameEvents.TapToPlay += StartTime;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= StartTime;
    }

    private void StartTime()
    {
        karenMove = true;
        karenAnim.SetBool(Walk,true);
        DOVirtual.DelayedCall(0.5f, () =>
        {
            door.DOLocalRotate(new Vector3(0, 0, -275), 1f);
        });
    }
    void Update()
    {
        if (karenMove)
        {
            karenNavmesh.SetDestination(startPoint.position);
            if (!karenNavmesh.pathPending)
            {
                if (karenNavmesh.remainingDistance <= karenNavmesh.stoppingDistance)
                {
                    if (!karenNavmesh.hasPath || karenNavmesh.velocity.sqrMagnitude == 0f)
                    {
                        karenAnim.SetBool(Walk,false);
                        suitcase.parent = null;
                        DOVirtual.DelayedCall(0.5f, () =>
                        {
                            suitcase.DOMove(new Vector3(-54f, 7.2f, -87f), 0.5f);
                            suitcase.DORotate(new Vector3(0, 90, 0), 0.5f).OnComplete(()=>
                                openSuitcase.DOLocalRotate(Vector3.zero, 0.5f).OnComplete(() =>
                                {
                                    cam.DOLocalMove(new Vector3(0.17f, 1.13f, 0.75f), 0.5f);
                                    cam.DOLocalRotate(new Vector3(90, 0, -9.5f), 0.5f);
                                    DOVirtual.DelayedCall(0.4f, () =>
                                    {
                                        cam.gameObject.SetActive(false);
                                        for (int i = 0; i < match.Count; i++)
                                        {
                                            match[i].SetActive(true);
                                        }
                                        Spawner.instance.isGameStart = true;
                                        Spawner.instance.isTimer = true;
                                    });
                                }));
                        });
                        karenMove = false;
                    }
                }
            }
        }
    }
}
