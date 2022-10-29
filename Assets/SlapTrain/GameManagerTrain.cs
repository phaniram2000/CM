using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerTrain : MonoBehaviour
{
	public static GameManagerTrain Instance;

	[SerializeField] private GameObject winRibbonPanel;
	[SerializeField] private GameObject failRibbonPanel;
	[SerializeField] private GameObject retryButtonPanel;
	[SerializeField] private GameObject nextButtonPanel;
	[SerializeField] private GameObject fillPanel;
	[SerializeField] private Image fillImage;
	[SerializeField] private GameObject tapToPlay;

	[SerializeField] private List<GameObject> heartImages;
	public int totalHearts;

	private int _totalPedestrians;
	public int totalSlappedPedestrians;

	private float _fillMultiplier;

	private void OnEnable()
	{
		GameEventsTrain.TapToPlay += OnTapToPlay;
	}

	private void OnDisable()
	{
		GameEventsTrain.TapToPlay -= OnTapToPlay;
	}

	private void Awake()
	{
		if (Instance)
			Destroy(gameObject);
		else
			Instance = this;

		DOTween.KillAll();
	}

	private void Start()
	{
		if(GAScript.Instance)
			GAScript.Instance.LevelStart(PlayerPrefs.GetInt("level", 1).ToString());
		Debug.Log(PlayerPrefs.GetInt("level", 1).ToString());
			_totalPedestrians = GameObject.FindGameObjectsWithTag("Pedestrian").Length;
		_totalPedestrians += GameObject.FindGameObjectsWithTag("PedestrianOnPhone").Length;
		_fillMultiplier = (float)1 / _totalPedestrians;
		fillImage.fillAmount = 0f;
		// totalHearts = heartImages.Count;
		totalHearts = 1;
	}

	public void FillTheSlapMeter()
	{
		fillImage.fillAmount += _fillMultiplier;
		if (totalSlappedPedestrians == _totalPedestrians) fillImage.fillAmount = 1f;
	}

	public void ReloadButton()
	{
		if(GAScript.Instance)
			GAScript.Instance.LevelFail(PlayerPrefs.GetInt("level", 1).ToString());
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	
	public void NextButton()
	{
		if(GAScript.Instance)
			GAScript.Instance.LevelCompleted(PlayerPrefs.GetInt("level", 1).ToString());
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

	}

	private void OnTapToPlay()
	{
		tapToPlay.SetActive(false);
	}

	public void RemoveTheHeart()
	{
		if (totalHearts == 0)
		{
			DOVirtual.DelayedCall(2f, ShowLostUi);
			return;
		}
		
		heartImages[^1].SetActive(false);
		heartImages.RemoveAt(heartImages.Count - 1);
		totalHearts--;
		print("Here");
	}

	public void ShowWinUi()
	{
		winRibbonPanel.SetActive(true);
		nextButtonPanel.SetActive(true);
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("Won");
			AudioManager.instance.Pause("Train");
		}
	}

	public void ShowLostUi()
	{
		failRibbonPanel.SetActive(true);
		retryButtonPanel.SetActive(true);
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("Lost");
			AudioManager.instance.Pause("Train");
		}
	}
}
