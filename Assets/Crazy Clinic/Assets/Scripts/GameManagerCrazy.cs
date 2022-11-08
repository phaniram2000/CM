using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerCrazy : MonoBehaviour
{
	public static GameManagerCrazy Instance;

	[SerializeField] private GameObject winRibbonPanel;
	[SerializeField] private GameObject failRibbonPanel;
	[SerializeField] private GameObject retryButtonPanel;
	[SerializeField] private GameObject nextButtonPanel;

	[SerializeField] private GameObject tapToPlay;
	[SerializeField] private Transform cuttingCameraPosTransform;
	[SerializeField] private Transform hammeringCameraPosTransform;
	[SerializeField] private GameObject optionsMenu;
	
	[SerializeField] private Transform bombingTransform;
	[SerializeField] private Transform bombingFinalTransform;
	[SerializeField] private float cameraMoveDuration;
	
	[SerializeField] private PatientControl patient;

	private Hands _hands;

	private Camera _camera;
	
	
	public Transform CuttingCameraTransform => cuttingCameraPosTransform;
	public Transform HammeringCameraTransform => hammeringCameraPosTransform;
	
	public GameObject OptionsMenu => optionsMenu;

	public float hammeringEventAngle = 40f;
	public float cuttingEventAngle = 20f;
	public float bombingEventAngle = 40f;

	private Transform initCamTransform;
	
	public PatientControl Patient
	{
		get => patient;
		set => patient = value;
	}

	public GameObject cutText;
	
	private void OnEnable()
	{
		GameEvents.TapToPlay += OnTapToPlay;
		GameEventsCrazy.DoneCutting += OnDoneCutting;
	}

	private void OnDisable()
	{
		GameEvents.TapToPlay -= OnTapToPlay;
		GameEventsCrazy.DoneCutting -= OnDoneCutting;
	}

	private void Awake()
	{
		if (Instance)
			Destroy(gameObject);
		else
			Instance = this;

		DOTween.KillAll();

		optionsMenu.SetActive(false);
		_camera = Camera.main;
		if (_camera != null) _hands = _camera.transform.GetChild(0).GetComponent<Hands>();
		//_hands.gameObject.SetActive(false);
		Vibration.Init();
		cutText.SetActive(false);
	}

	private void OnTapToPlay()
	{
		tapToPlay.SetActive(false);
		if(GAScript.Instance)
			GAScript.Instance.LevelStart(PlayerPrefs.GetInt("level", 1).ToString());
		Vibration.Vibrate(30);

	}

	public void BombingButton()
	{
		ShowTheHands();
		MoveCameraToEventPosition(bombingTransform, bombingEventAngle);
		
		DOVirtual.DelayedCall(cameraMoveDuration + 0.25f, GameEventsCrazy.InvokeOptionBomb);
		DOVirtual.DelayedCall(cameraMoveDuration + 1.5f, MoveCameraBackToStartPosition);
		HideOptionsMenu();
		if(AudioManager.instance)
			AudioManager.instance.Play("Button");
		Vibration.Vibrate(30);
	}

	public void HammeringButton()
	{		
		ShowTheHands();
		MoveCameraToEventPosition(HammeringCameraTransform,hammeringEventAngle);
		
		DOVirtual.DelayedCall(cameraMoveDuration + 0.25f, GameEventsCrazy.InvokeOptionHammer);
		DOVirtual.DelayedCall(cameraMoveDuration + 1.5f, MoveCameraBackToStartPosition);
		HideOptionsMenu();
		if(AudioManager.instance)
			AudioManager.instance.Play("Button");
		Vibration.Vibrate(30);
	}

	public void CuttingButton()
	{
		ShowTheHands();
		MoveCameraToEventPosition(cuttingCameraPosTransform, cuttingEventAngle);
		
		DOVirtual.DelayedCall(cameraMoveDuration + 0.25f, ()=>
		{
			GameEventsCrazy.InvokeOptionCutter();
			cutText.SetActive(true);
		});
		HideOptionsMenu();
		if(AudioManager.instance)
			AudioManager.instance.Play("Button");
		Vibration.Vibrate(30);
	}

	private void MoveCameraToEventPosition(Transform eventStartPos, float xAngle)
	{
		initCamTransform = Camera.main.transform;
		print(initCamTransform);
		_camera.transform.DOMove(eventStartPos.position, cameraMoveDuration).SetEase(Ease.Linear);
		_camera.transform.DORotate(new Vector3(xAngle, 0f, 0f), 0.25f).SetEase(Ease.Linear);
	}

	private void MoveCameraBackToStartPosition()
	{
		
		//_camera.transform.DOMove(new Vector3(0f,2.02f,-4.85f), cameraMoveDuration).SetEase(Ease.Linear);
		_camera.transform.DOMove(initCamTransform.position, cameraMoveDuration).SetEase(Ease.Linear);
		// _camera.transform.DORotate(new Vector3(15f, 0f, 0f), 0.25f).SetEase(Ease.Linear);
		_camera.transform.DORotate(initCamTransform.transform.rotation.eulerAngles, 0.25f).SetEase(Ease.Linear);
	}

	private void HideOptionsMenu()
	{
		OptionsMenu.SetActive(false);
	}

	private void ShowTheHands()
	{
		_hands.gameObject.SetActive(true);
	}

	private void OnDoneCutting()
	{
		_hands.gameObject.SetActive(false);
		MoveCameraBackToStartPosition();
		cutText.SetActive(false);
	}
	
	public void ReloadButton()
	{
		if(GAScript.Instance)
			GAScript.Instance.LevelFail(PlayerPrefs.GetInt("level", 1).ToString());
		// if(ISManager.instance)
		// 	ISManager.instance.ShowInterstitialAds();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		Vibration.Vibrate(30);

	}
	
	public void NextButton()
	{
		if(GAScript.Instance)
			GAScript.Instance.LevelCompleted(PlayerPrefs.GetInt("level", 1).ToString());
		// if(ISManager.instance)
		// 	ISManager.instance.ShowInterstitialAds();
		Debug.Log(PlayerPrefs.GetInt("level", 1).ToString());
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		if (PlayerPrefs.GetInt("level") >= (SceneManager.sceneCountInBuildSettings) - 1)
		{
			PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("Level", 1) + 1);
			var i = Random.Range(1, SceneManager.sceneCountInBuildSettings);
			PlayerPrefs.SetInt("ThisLevel", i);
			SceneManager.LoadScene(i);
		}
		else
		{
			PlayerPrefs.SetInt("level", SceneManager.GetActiveScene().buildIndex + 1);
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
		Vibration.Vibrate(30);

	}
	
	public void ShowWinUi()
	{
		winRibbonPanel.SetActive(true);
		nextButtonPanel.SetActive(true);
		if (AudioManager_Crazy.instance)
		{
			AudioManager_Crazy.instance.Play("Won");
			AudioManager_Crazy.instance.Pause("Train");
		}
	}

	public void ShowLostUi()
	{
		failRibbonPanel.SetActive(true);
		retryButtonPanel.SetActive(true);
		if (AudioManager_Crazy.instance)
		{
			AudioManager_Crazy.instance.Play("Lost");
			AudioManager_Crazy.instance.Pause("Train");
		}
	}
}
