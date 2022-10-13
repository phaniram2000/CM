using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ShuffleCups
{
	public class PaperMainCanvasController : MonoBehaviour
{
	[SerializeField] private GameObject retryText, retryButton, nextText, nextButton, loseText, winText, titleText;
	[SerializeField] private Text levelText;
	
	private float _timeSpent;
	private bool _addTime = true;
	
	private void OnEnable()
	{
		PaperGameEvents.Singleton.tearPaper += OnPlayerLose;
		PaperGameEvents.Singleton.playerCrossFinishLine += OnPlayerWin;
		PaperGameEvents.Singleton.aiCrossFinishLine += OnPlayerLose;
	}

	private void OnDisable()
	{	
		PaperGameEvents.Singleton.tearPaper -= OnPlayerLose;
		PaperGameEvents.Singleton.playerCrossFinishLine -= OnPlayerWin;
		PaperGameEvents.Singleton.aiCrossFinishLine -= OnPlayerLose;
	}
	
	private void Start()
	{
		_timeSpent = 0f;
		levelText.text = "Level " + PlayerPrefs.GetInt("levelNo");
		
		if(titleText)
			titleText.SetActive(true);
		
		if(GAScript.Instance)
			GAScript.Instance.LevelStart(PlayerPrefs.GetInt("levelNo").ToString());
	}
	
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.R)) Retry();
		
		if (_addTime)
				_timeSpent += Time.deltaTime;
	}

	public void Go()
	{
		AudioManager.instance.Play("button");
		GameEvents.Singleton.InvokeShuffleStart();
		PaperGameEvents.Singleton.InvokeTapToPlay();
		
		if(titleText)
			titleText.SetActive(false);
	}

	public void Retry()
	{
		AudioManager.instance.Play("Button");
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
	
	
	private void PlayerHasLost()
	{
		_addTime = false;
		loseText.gameObject.SetActive(true);
		retryText.SetActive(true);
		retryButton.SetActive(true);

		var x = 1 - GameObject.FindGameObjectWithTag("Player").GetComponent<PaperPullerPlayer>().myData
			.distanceFromZero;
		
		
	}
	
	private void PlayerHasWon()
	{
		_addTime = false;
		winText.gameObject.SetActive(true);
		nextButton.SetActive(true);
		nextText.SetActive(true);
		
		
	}
	
	private void OnPlayerWin()
	{
		DOTween.Sequence().AppendInterval(5f).AppendCallback(PlayerHasWon);
		
		global::GameEvents.InvokeGameWin();
	}

	private void OnPlayerLose()
	{
		DOTween.Sequence().AppendInterval(5f).AppendCallback(PlayerHasLost);
		
		global::GameEvents.InvokeGameLose(-1);
	}
}
}

