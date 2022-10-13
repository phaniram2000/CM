using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ShuffleCups
{
	public class MainCanvasController : MonoBehaviour
{
	[SerializeField] private GameObject retryText, retryButton, nextText, nextButton, loseText, winText, titleText;
	[SerializeField] private Text levelText;

	private float _timeSpent;
	private bool _addTime = true;
	
	private void OnEnable()
	{
		GameEvents.Singleton.ShuffleStart += OnShuffleStart;

		GameEvents.Singleton.GameResult += OnGameResult;

		global::GameEvents.TapToPlay += GO;
	}

	private void OnDisable()
	{
		GameEvents.Singleton.ShuffleStart -= OnShuffleStart;

		GameEvents.Singleton.GameResult -= OnGameResult;
		
		global::GameEvents.TapToPlay -= GO;
	}

    private void Start()
    {
		levelText.text = "Level " + PlayerPrefs.GetInt("levelNo");
		_timeSpent = 0f;
		
		if(titleText)
			titleText.SetActive(true);
		
		if(GAScript.Instance)
			GAScript.Instance.LevelStart(PlayerPrefs.GetInt("levelNo").ToString());
	}

	private void Update()
	{
		if (_addTime)
			_timeSpent += Time.deltaTime; 
	}

	private void OnShuffleStart()
    {
		if(titleText)
			titleText.SetActive(false);
    }

	private void OnGameResult(bool didWin)
	{
		DOTween.Sequence().AppendInterval(5f).AppendCallback(() => Decide(didWin));
	}

	private void Decide(bool didWin)
	{
		_addTime = false;
		if (didWin) 
		{ 
			winText.gameObject.SetActive(true);
			nextButton.SetActive(true);
			nextText.SetActive(true);
			
			global::GameEvents.InvokeGameWin();

			
		}
		else
		{
			loseText.gameObject.SetActive(true);
			retryText.SetActive(true);
			retryButton.SetActive(true);
			
			global::GameEvents.InvokeGameLose(-1);
			
			
		}
	}

	public void GO()
	{
		AudioManager.instance.Play("Button");
		print("Shuffle start");
		GameEvents.Singleton.InvokeShuffleStart();
		
		if(PaperGameEvents.Singleton)
			PaperGameEvents.Singleton.InvokeTapToPlay();
	}

	public void Retry()
	{
		AudioManager.instance.Play("button");
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void NextLevel()
	{
		AudioManager.instance.Play("button");
		if (PlayerPrefs.GetInt("levelNo") < SceneManager.sceneCountInBuildSettings - 1)
		{
			var x = PlayerPrefs.GetInt("levelNo") + 1;
			SceneManager.LoadScene(x);
			PlayerPrefs.SetInt("lastBuildIndex", x);
		}
		else
		{
			var x = Random.Range(5, SceneManager.sceneCountInBuildSettings - 1);
			SceneManager.LoadScene(x);
			PlayerPrefs.SetInt("lastBuildIndex", x);
		}
		PlayerPrefs.SetInt("levelNo", PlayerPrefs.GetInt("levelNo") + 1);
	}
}
}

