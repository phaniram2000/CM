using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class AiParent : MonoBehaviour
{
    public static AiParent instance;
    public GameObject[] aiPlayers;
    public Transform[] spawnPos;
    public Transform[] aiMovingTransforms;
    public int maxSpawnAis;
    public Transform spawnedAiParent;
    public TableManager sellingTransform;
    public List<GameObject> spawnedAiList = new();
    public Transform aiBuyingTransform;
    public float thresholdDistance;
	private NavMeshAgent ai;
	private float aa;
	private bool once;
    private void Awake()
    {
        instance = this;
    }

	private void Start()
    {
        for (int i = 0; i < maxSpawnAis; i++)
        {
            GameObject SpawnedAis = Instantiate(aiPlayers[Random.Range(0, aiPlayers.Length)].gameObject);
            if (!spawnedAiList.Contains(SpawnedAis))
            {
                spawnedAiList.Add(SpawnedAis);
            }
            SpawnedAis.name = "Ai - " + i;
            SpawnedAis.transform.parent = spawnedAiParent;
            SpawnedAis.transform.position = spawnPos[i].position;
            SpawnedAis.transform.rotation = spawnPos[i].rotation;
        }
    }

	private void Update()
    {
        CheckAvailableItems();
    }
    public void CheckAvailableItems()
    {
        if (sellingTransform.childObjs.Count > 0)
        {
            aa += Time.deltaTime;
            if (spawnedAiList.Count > 0 && aa > 0.5f)
            {
                ai = spawnedAiList[0].GetComponent<NavMeshAgent>();
                ai.SetDestination(aiBuyingTransform.position);
                ai.GetComponent<Animator>().SetTrigger("slowCollect");
                if (ai.remainingDistance < 0.25)
                {
                    BalancePlaces();
                }
                if (!once)
                {
                    Kill();
                    once = true;
                }
                aa = 0;
            }
        }
    }
    public void BalancePlaces()
    {
        for (int i = 1; i < spawnedAiList.Count; i++)
        {
            Animator animator = spawnedAiList[i].GetComponent<Animator>();
            animator.SetTrigger("slowWalk");
            spawnedAiList[i].transform.DOMove(spawnPos[i].position, 1, false);
            spawnedAiList[i].transform.rotation = spawnPos[i].transform.rotation;
        }
    }
    public void Kill()
    {
        for (int i = 0; i < spawnedAiList.Count; i++)
        {
            spawnedAiList[i].transform.DOKill();
        }
    }
}
