using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AiBuying : MonoBehaviour
{
    public static AiBuying instance;
    public TableManager _TableParent;
    public Transform aiHandTransform;
    public Transform moneyParentTransform;
    public List<GameObject> moneyList = new();
    public List<Transform> moneyListParentTrans = new();
    public Transform spawnMoneyParent;
    public GameObject moneyPrefab;
    public static MetaUiManager uiManager;
	
    public RectTransform moneyIcon;
	private GameManager gm;
    
    public bool isCallingOnce/*checkBuying*/,isPlayerSelling;
    public int maxCoinsSpawnCount;
    public bool maxCount;
    public int num;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        AddMoneyToList();
        uiManager = MetaUiManager.instance;
		gm = GameManager.instance;
    }
	private void AddMoneyToList()
    {
        for (var i = 0; i < moneyParentTransform.childCount; i++) 
			moneyListParentTrans.Add(moneyParentTransform.GetChild(i));
	}

	private void MoneyEffect2()
    {
		maxCoinsSpawnCount = uiManager.moneyPrefabCount;
		StartCoroutine(Loop());
    }

	private IEnumerator Loop()
    {
        if (num <= maxCoinsSpawnCount)
        {
            uiManager.moneyParent.GetChild(num).gameObject.SetActive(true);
            var rect = uiManager.moneyParent.GetChild(num).transform.GetComponent<RectTransform>();
            rect.DOMove(moneyIcon.position, 0.75f).SetEase(Ease.Linear).OnComplete(() => DisableMoneyy());
            maxCount = false;
        }
        yield return new WaitForSeconds(0.1f);
        if (num <= maxCoinsSpawnCount)
        {
            num++;
            StartCoroutine(Loop());
        }
        else
            StartCoroutine(delay());
    }

	private IEnumerator delay()
    {
        yield return new WaitForSeconds(num);
        num = 0;
	}

	private void DisableMoneyy()
    {
        uiManager.playerMoneyText.text = MyPlayerPrefsSave.GetTotalMoney() + "$";
        for (int i = 0; i < uiManager.moneyParent.childCount; i++)
        {
            uiManager.moneyParent.GetChild(i).gameObject.SetActive(false);
            NormalizeRect(uiManager.moneyParent.GetChild(i).GetComponent<RectTransform>());
        }
        isCallingOnce = false;
        maxCount = false;
    }

	private static void NormalizeRect(RectTransform rect) => rect.anchoredPosition = Vector2.zero;

	private void GenerateMoney()
    {
        MyPlayerPrefsSave.SetTotalMoney(MyPlayerPrefsSave.GetTotalMoney() + gm.moneyMultiplyer);
		var g = Instantiate(moneyPrefab);
		g.transform.position = moneyListParentTrans[moneyList.Count].position;
        g.transform.parent = spawnMoneyParent;
        moneyList.Add(g);
    }

	private void CollectMoney()
	{
		if (moneyList.Count <= 0) return;
		
		print("this code is not worked on.");
		moneyList[^1].transform.parent = PlayerMoneyCollect.player.transform;
		moneyList.Remove(moneyList[^1]);
	}

	private void Buying(Collider other)
    {
        other.GetComponent<AiPlayer>().canMove = true;
        aiHandTransform = other.GetComponent<AiPlayer>().handTransform;
		if (_TableParent.childObjs.Count <= 0) return;
		
		var currentObject = _TableParent.childObjs[^1];
		MoneyMultiplierCheck(currentObject.name);
		currentObject.transform.parent = aiHandTransform;
		currentObject.transform.SetPositionAndRotation(aiHandTransform.position, aiHandTransform.rotation);
		_TableParent.childObjs.Remove(currentObject);
		_TableParent.index--;
	}

	private void MoneyMultiplierCheck(string itemName)
    {
        for (var i = 1; i < gm.moneyArray.Length; i++)
        {
            if (itemName == "Item" + i)
            {
                gm.moneyMultiplyer = 15 + (i * 5);
            }
        }
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 6)
		{
			Buying(other);
			GenerateMoney();
		}
		if (other.gameObject.CompareTag("Player"))
		{
			//if (ItemsHoldingParent.instance.childObjs.Count < 14)
			gm.shoppingCam.gameObject.SetActive(true);
			if (MetaAudioManager.instance) MetaAudioManager.instance.PlayMoneyCollectedSound();
		}
	}
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			CollectMoney();
			//checkBuying = true;
			if (MyPlayerPrefsSave.GetTotalMoney() > 0)
			{
				if (!isCallingOnce) MoneyEffect2();
				
				var total = MyPlayerPrefsSave.GetTotalMoney();
				MyPlayerPrefsSave.SetTotalMoney(total);
			}
			isPlayerSelling = true;
		}
		if (other.gameObject.CompareTag("Assistant"))
		{
			gm.playerRobotWaitingTime += Time.deltaTime;
			if (gm.playerRobotWaitingTime > 1.5f)
			{
				var rm = other.GetComponent<RobotMovement>();
				rm.myState = RobotMovement.State.collecting;
				rm.once = false;
			}
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 6)
		{
			aiHandTransform = null;
			AiParent.instance.spawnedAiList.Remove(other.gameObject);
			other.GetComponent<Animator>().SetBool("Walk", true);
			gm.playerRobotWaitingTime = 0;
		}
		if (other.gameObject.CompareTag("Player"))
		{
			gm.shoppingCam.gameObject.SetActive(false);
			if (MetaAudioManager.instance) 
				MetaAudioManager.instance.TurnOFFAS3();
			isPlayerSelling = false;
		}
	}
}