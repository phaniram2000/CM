using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using EPOOutline;
using TMPro;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;
    public MatchingObject[] objectPrefabs;
    [Header("Level Variables")]
    public int objectCount;
    public int pairCount = 5;
    public float minX, maxX, minY, maxY, minZ, maxZ;
    private Transform objectHolder;
    public List<MatchingObject> activeObjects = new List<MatchingObject>();
    private List<int> selectedIndex = new List<int>();
    public MatchingManager matchingManager;

    [Space]
    [Header("Matching Object Attributes")]
    public float moveSpeed = 200f;
    public float throwForce = 1.5f;
    public float rotateForce = 1f;
    public float expelForce = 50f;
    public float height = 1f;

    public MatchingObject highlightObject_01;
    public MatchingObject highlightObject_02;
    public float hintTimer;
    public float hintTime = 5f;
    public float levelTime;
    public TextMeshProUGUI levelTimeText;
    public bool isLevelTime;
    public UnityEvent onCompleteEvent;

    [Space]
    [Header("Outline Effect Attributes")]
    public Color outlineColor;
    [Range(0,10)]
    public float outlineWidth;

    public bool isOutline;
    public bool isGameStart;
    public bool isTimer;
    public GameObject timer;
   private void Start()
   {
      // SpawnObjects();
      isLevelTime = true;
      AudioManager.instance.Play("BGM");
   }

   private void Awake()
   {
       instance = this;
   }
   

   private void StartTime()
   {
       isGameStart = true;
       isTimer = true;
   }

   private void Update()
    {
        if (isGameStart)
        {
            if (hintTimer > 0)
            {
                hintTimer -= Time.deltaTime;
                if (hintTimer <= 0)
                {
                    HideHint();
                }
            }

            if (MatchingState.Full == matchingManager.state)
            {
                isHint = true;
            }

            if (levelTime > 0 && isLevelTime)
            {
                levelTime -= Time.deltaTime;
                levelTimeText.text = Mathf.RoundToInt(levelTime).ToString();
            }

            if (levelTime < 10)
            {
                if (isTimer)
                {
                    timer.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f).SetEase(Ease.OutQuad).SetLoops(-1,LoopType.Yoyo);
                    isTimer = false;
                }
            }

            if (levelTime < 0)
            {
                // levelTimeText.gameObject.GetComponent<DOTweenAnimation>().DOPause();
                GameEvents.InvokeGameLose(-1);
                for (int i = 0; i < activeObjects.Count; i++)
                {
                    activeObjects[i].GetComponent<MatchingObject>().gameObject.SetActive(false);
                }

                isLevelTime = false;
            }
        }
    }
    private void Init()
    {
        string holderName = "Generated Objects";
        if (transform.Find(holderName))
            DestroyImmediate(transform.Find(holderName).gameObject);
        selectedIndex.Clear();
        activeObjects = new List<MatchingObject>();
        objectHolder = new GameObject(holderName).transform;
        objectHolder.parent = transform;
    }
    

    // public void SpawnObjects()
    // {
    //     Init();
    //     hintTimer = 0f;
    //     matchingManager.ResetState();
    //     highlightObject_01 = null;
    //     highlightObject_02 = null;
    //     for (int i = 0; i < objectPrefabs.Length; i++)
    //     {
    //         if (i > objectCount)
    //             return;
    //         for (int j = 0; j < Random.Range(1, pairCount + 1); j++) 
    //         {
    //             int randomIndex;
    //
    //             do
    //             {
    //                 randomIndex = Random.Range(0, objectPrefabs.Length - 1);
    //             }
    //             while (selectedIndex.Contains(randomIndex));
    //             selectedIndex.Add(randomIndex);
    //
    //             var matchingObject_01 = Instantiate(objectPrefabs[randomIndex], GetRandomPos(), Random.rotation, objectHolder);
    //             var matchingObject_02 = Instantiate(objectPrefabs[randomIndex], GetRandomPos(), Random.rotation, objectHolder);
    //             matchingObject_01.SetData(moveSpeed, throwForce, rotateForce, expelForce, height, outlineColor, outlineWidth);
    //             matchingObject_02.SetData(moveSpeed, throwForce, rotateForce, expelForce, height,outlineColor, outlineWidth);
    //             matchingObject_01.pairedObject = matchingObject_02;
    //             matchingObject_02.pairedObject = matchingObject_01;
    //             activeObjects.Add(matchingObject_01);
    //             activeObjects.Add(matchingObject_02);
    //
    //         }           
    //     }
    //
    //
    // }

    private Vector3 GetRandomPos()
    {
        return new Vector3((int)Random.Range(minX, maxX), Random.Range(minY, maxY), (int)Random.Range(minZ, maxZ));
    }

    public void CheckCompleteLevel()
    {
        if (activeObjects.Count > 0)
            return;
        Debug.Log("Complete");
        AudioManagerMatch.instance.PlaySFX("Complete", 0.8f);
        GameEvents.InvokeGameWin();
        isLevelTime = false;
        AudioManager.instance.Play("Clap");
        onCompleteEvent?.Invoke();
    }

    public bool isHint;
    public void ShowHint()
    {
        if (MatchingState.Half == matchingManager.state)
        {
            isHint = false;
            matchingManager.leftObject.pairedObject.outlineEffect.enabled = true;
            if (isOutline)
            {
                matchingManager.leftObject.pairedObject.GetComponent<Outlinable>().enabled = true;
            }
        }
        
        if (isHint)
        {
            if (hintTimer > 0)
                return;
            GetRandomActivePair();
            hintTimer = hintTime;
        }
    }

    public void HideHint()
    {
        if (highlightObject_01 == null || highlightObject_02 == null)
            return;
        highlightObject_01.SetHint(false);
        highlightObject_02.SetHint(false);
        if (isOutline)
        {
            highlightObject_01.GetComponent<Outlinable>().enabled = false;
            highlightObject_02.GetComponent<Outlinable>().enabled = false;
        }
        highlightObject_01 = null;
        highlightObject_02 = null;
    }

    private void GetRandomActivePair()
    {
        
        highlightObject_01 = null;
        highlightObject_02 = null;
        int randIndex = Random.Range(0, activeObjects.Count - 1);
        highlightObject_01 = activeObjects[randIndex];
        highlightObject_02 = highlightObject_01.pairedObject;
        highlightObject_01.SetHint(true);
        highlightObject_02.SetHint(true);
        if (isOutline)
        {
            highlightObject_01.GetComponent<Outlinable>().enabled = true;
            highlightObject_02.GetComponent<Outlinable>().enabled = true;
        }
    }
}
