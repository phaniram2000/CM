using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
	public static GameManager instance { get; private set; }

	public CollectibleMoney collectiblePrefab;
	
	[HideInInspector] public string myIndex = "index";

	[SerializeField] private List<MetaShop> shopList;

	[HideInInspector] public bool once;
	[Header("BuildingCount")] public int buildingNumber;

	[Header("Money Multipler")] public int moneyMultiplyer;

	public int[] moneyArray;

	[Header("             ")] public int playerMaxCapacity = 7;
	public int playerSpeed = 5, playerCollectingSpeed = 5, playerRobotCapacity = 3;

	[Header("    ")] public float playerRobotWaitingTime = 0;

	[HideInInspector] public string playerCapacityData = "capacity",
		playerSpeedData = "playerSpeed",
		playerCollectingSpeedData = "collectingSpeed",
		playerRobotCapacityData = "robotCapacity",
		assistantData = "Assistant",
		aiPlayerSpeedData = "AiplayerSpeed";

	[Header("Ai Players")] public float aiSpeed;

	[HideInInspector] public string aiSpeedData = "aiSpeed";

	[Header("Assistant Prefab")] public GameObject playerRobotPrefab;

	[Header("Cloud Particle")] public ParticleSystem cloudParticle;
	
	public CinemachineVirtualCamera shoppingCam;

	private void Awake()
	{
		Application.targetFrameRate = 120;
		if (instance != null && instance != this)
			Destroy(gameObject);
		else
			instance = this;

		SetIndex(myIndex);
	}

	private void Start()
	{
		DeActivatingBuildings();
		UnlockedBuildings();
		PlayerCapacityData(playerCapacityData);
		PlayerSpeedData(playerSpeedData);
		AiSpeedData(aiSpeedData);
		SetPlayerCollectingSpeedData(playerCollectingSpeedData);
		StartingSpawn();
		PlayerPrefs.SetInt("LevelStart", PlayerPrefs.GetInt("LevelStart", 0));
		PlayerPrefs.SetInt("LevelCompleted", PlayerPrefs.GetInt("LevelCompleted", 1));
		StartCoroutine(ActivateBuyingArea());
		var cameraData = Camera.main.GetUniversalAdditionalCameraData();
		cameraData.renderType = CameraRenderType.Base;
	}

	private void Update()
	{
		buildingNumber = PlayerPrefs.GetInt(myIndex, 0);
		playerMaxCapacity = PlayerPrefs.GetInt(playerCapacityData, playerMaxCapacity);
		playerSpeed = PlayerPrefs.GetInt(playerSpeedData, playerSpeed);
		playerCollectingSpeed = PlayerPrefs.GetInt(playerCollectingSpeedData, playerCollectingSpeed);
		aiSpeed = PlayerPrefs.GetFloat(aiSpeedData, aiSpeed);
		if (MyPlayerPrefsSave.GetTotalMoney() > 150 && !once)
		{
			StartCoroutine(ActivateBuyingArea());
			once = true;
		}

		ScoreShow();
	}

	private static void ScoreShow()
	{
		var score = MyPlayerPrefsSave.GetTotalMoney();
		MetaUiManager.instance.playerMoneyText.text = score switch
		{
			> 1000 and < 1000000 => "$" + score / 1000f + "K",
			>= 1000000 => "$" + score / 1000000f + "M",
			_ => score + "$"
		};
	}

	private void UnlockedBuildings()
	{
		for (var i = 0; i < GetIndex(); i++) shopList[i].gameObject.SetActive(true);
	}

	private void DeActivatingBuildings()
	{
		foreach (var shop in shopList) shop.gameObject.SetActive(false);
	}

	private IEnumerator ActivateBuyingArea()
	{
		yield return new WaitForSeconds(1.5f);
		if (GetIndex().Equals(buildingNumber)) 
			Condition(buildingNumber);
	}

	private void Condition(int num)
	{
		print($"requesting shop number {num + 1} but only have shops {shopList.Count}");
		var b = shopList[num];
		b.gameObject.SetActive(true);
		b.cam.gameObject.SetActive(true);
		StartCoroutine(TurnOffCamera(num));
	}

	private IEnumerator TurnOffCamera(int num)
	{
		yield return new WaitForSeconds(3f);
		var b = shopList[num];
		b.cam.SetActive(false);
	}

	//playerprefs Area
	private static void SetIndex(string name) => PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name, 0));

	private int GetIndex() => PlayerPrefs.GetInt(myIndex, 0);

	private void PlayerCapacityData(string name) => PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name, playerMaxCapacity));

	public int GetPlayerCapacityData() => PlayerPrefs.GetInt(name, playerMaxCapacity);

	private void PlayerSpeedData(string name) => PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name, playerSpeed));

	public int GetPlayerSpeedData() => PlayerPrefs.GetInt(name, playerSpeed);

	private void AiSpeedData(string name) => PlayerPrefs.SetFloat(name, PlayerPrefs.GetFloat(name, aiSpeed));

	public float GetAiSpeedData() => PlayerPrefs.GetFloat(name, aiSpeed);

	private void SetPlayerCollectingSpeedData(string name) => PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name, playerCollectingSpeed));

	public int GetPlayerCollectingSpeedData() => PlayerPrefs.GetInt(name, playerCollectingSpeed);
	
	public int GetAssistantData() => PlayerPrefs.GetInt(assistantData, 0);

	public void PlayerRobotSpawner()
	{
		var prefab = Instantiate(playerRobotPrefab);
		prefab.transform.position = Player.instance.transform.position + new Vector3(0, 0, 3f);
	}

	private void StartingSpawn()
	{
		for (var i = 0; i < GetAssistantData(); i++)
		{
			var prefab = Instantiate(playerRobotPrefab);
			prefab.transform.position = Player.instance.transform.position + new Vector3(Random.Range(0, 3), 0, Random.Range(0, 3));
		}
	}
}