using System;
using DG.Tweening;
using Meta;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainCanvasController : MonoBehaviour
{
	private enum GameState { Playing, Won, Lost }
	[Serializable] private struct IntRange { public int min, max; }
	
	[SerializeField] private int moneyEarned = 100;
	[SerializeField] private bool dontUsePlayerPrefsMoneyEarned;
	
	[SerializeField] private IntRange richRankChangeRange;

	[SerializeField] private RankPanel ranker;
	
	[Header("UI Elements"), SerializeField] private Image blackBg;

	[SerializeField] private GameObject constantRetryButton,
		endPanel,
		nextButton,
		retryButton,
		passedHeader,
		failedHeader,
		skipLevelButton,
		metaCanvasPanel;

	[SerializeField] private bool showSkipLevelButton;
	
	[SerializeField] private TextMeshProUGUI levelText, moneyEarnedText, bankBalanceText, playerRichRankText;

	[Header("Cash Particles"), SerializeField] private RectTransform cashDest;
	[SerializeField] private ParticleControlScript cashParticleController;

	private static Tweener _bankBalanceTextTween, _richRankTextTween;

	private Loader _loader;
	private Color _initBlackBgColor;
	private GameState _currentState = GameState.Playing;

	private void OnEnable()
	{
		GameEvents.TapToPlay += OnTapToPlay;
		GameEvents.GameLose += OnGameLose;
		GameEvents.GameWin += OnGameEnd;
		
		MetaEvents.AlterBankBalance += OnAlterBankBalance;
		MetaEvents.AlterRichRank += OnAlterRichRank;
	}

	private void OnDisable()
	{
		GameEvents.TapToPlay -= OnTapToPlay;
		GameEvents.GameLose -= OnGameLose;
		GameEvents.GameWin -= OnGameEnd;
		
		MetaEvents.AlterBankBalance -= OnAlterBankBalance;
		MetaEvents.AlterRichRank -= OnAlterRichRank;
	}

	private void Awake() => DOTween.KillAll();

	private void Start()
	{
		_loader = transform.parent.GetComponentInChildren<Loader>();
		var levelNo = PlayerPrefs.GetInt("levelNo", 1);
		levelText.text = "Day " + levelNo;

		if(!dontUsePlayerPrefsMoneyEarned)
			moneyEarned = PlayerPrefs.GetInt("CurrentLevelEarnings", 100);
		
		_initBlackBgColor = blackBg.color;
		blackBg.color = Color.clear;
		blackBg.raycastTarget = false;
		
		_bankBalanceTextTween = _richRankTextTween = null;
		if (AudioManager.instance)
			AudioManager.instance.Play("BGM");
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.N)) NextLevel();
		
		if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			
		if (Input.GetKeyDown(KeyCode.J)) ShowWinPanel();
		if (Input.GetKeyDown(KeyCode.L)) ShowLosePanel();
	}

	public void Add500()
	{
		ShopStateController.AlterBankBalance(500, true);
	}

	private void EnableVictoryObjects()
	{
		if (_currentState != GameState.Won) return;

		_loader.ShowLoader();
		
		if(AudioManager.instance)
			AudioManager.instance.Play("Win");
	}
	
	public void ShowWinPanelAfterLoader()
	{
		ShowWinPanel();
		var startPos = cashParticleController.transform.parent.GetComponent<RectTransform>().anchoredPosition;
		CoinEffects.instance.PlayCoinEffects(cashDest,startPos );
		if(AudioManager.instance)
			AudioManager.instance.Play("");
		//cashParticleController.PlayControlledParticles(cashDest);
		// var x = Instantiate(cashParticleController);
		// DOVirtual.DelayedCall(.5f, () =>
		// {
		// 	
		// 	x.PlayControlledParticles(cashDest);
		// });
	}

	private void EnableLossObjects()
	{
		if (_currentState != GameState.Lost) return;
		
		ShowLosePanel();
		
		if(AudioManager.instance)
			AudioManager.instance.Play("Lose");
	}

	private void ShowWinPanel()
	{
		endPanel.transform.DOScale(0f, 0.5f).From();
		endPanel.SetActive(true);
		passedHeader.SetActive(true);
		DOVirtual.DelayedCall(1.5f, () => nextButton.SetActive(true));

		if(!dontUsePlayerPrefsMoneyEarned)
			moneyEarnedText.text = "YOU EARNED : $" + moneyEarned;
		else
			moneyEarnedText.text = "YOU EARNED : $" + 100;
		
		
		bankBalanceText.text = "$" + ShopStateController.CurrentState.GetBankBalance;
		playerRichRankText.text = "#" + ShopStateController.CurrentState.GetRichRank;

		print("money :" + moneyEarned);
		
		ShopStateController.AlterBankBalance(moneyEarned);
		ShopStateController.AlterRichRank(MakeRichRankChange(true));

		ranker.WinRanking(ShopStateController.CurrentState.GetRichRank);
		playerRichRankText.DOColor(Color.green, 0.5f).SetLoops(-1, LoopType.Yoyo);
		
		//if there are errors here, its because both these tweens need to be init, they are null on start()
		_bankBalanceTextTween.Play();
		_bankBalanceTextTween.OnComplete(() => _richRankTextTween.Play());
	}

	private void ShowLosePanel()
	{
		endPanel.transform.DOScale(0f, 0.5f).From();
		endPanel.SetActive(true);
		failedHeader.SetActive(true);
		DOVirtual.DelayedCall(1.5f, () => retryButton.SetActive(true));
		
		if(showSkipLevelButton)
			DOVirtual.DelayedCall(2.5f, () => skipLevelButton.SetActive(true));

		moneyEarnedText.text = "YOU EARNED : $" + 0;
		bankBalanceText.text = "$" + ShopStateController.CurrentState.GetBankBalance;
		playerRichRankText.text = "#" + ShopStateController.CurrentState.GetRichRank;

		ShopStateController.AlterRichRank(MakeRichRankChange(false));
		
		ranker.LoseRanking(ShopStateController.CurrentState.GetRichRank);
		playerRichRankText.DOColor(Color.red, 0.5f).SetLoops(-1, LoopType.Yoyo);
		
		bankBalanceText.transform.DOScale(1.15f, 0.5f).SetLoops(-1, LoopType.Yoyo);
		bankBalanceText.DOColor(Color.red, 0.5f).SetLoops(-1, LoopType.Yoyo);
	}

	private void OnGameEnd(bool didWin)
	{
		constantRetryButton.SetActive(false);

		DOVirtual.DelayedCall(2.5f, () =>
		{
			blackBg.enabled = true;
			blackBg.DOColor(_initBlackBgColor, 1f)
				.OnComplete(() => blackBg.raycastTarget = true);
		});
		
		DOVirtual.DelayedCall(3.5f, () =>
		{
			if (didWin)
				EnableVictoryObjects();
			else
				EnableLossObjects();
		});
	}

	private static void UpdateBankBalanceText(int oldAmount, int newAmount, TMP_Text text)
	{
		_bankBalanceTextTween = DOTween.To(() => oldAmount, value => oldAmount = value, newAmount, 1.5f)
			.SetEase(Ease.OutCubic)
			.OnUpdate(() => text.text = "$ " + oldAmount);
		_bankBalanceTextTween.Pause();
	}

	private static void UpdateRichRankText(int oldRank, int newRank, TMP_Text text)
	{
		_richRankTextTween = DOTween.To(() => oldRank, value => oldRank = value, newRank, 1.5f)
			.SetEase(Ease.OutCubic)
			.OnUpdate(() => text.text = "#" + oldRank);
		_bankBalanceTextTween.Pause();
	}

	private int MakeRichRankChange(bool positiveEffect) => Random.Range(richRankChangeRange.min, richRankChangeRange.max) * (positiveEffect ? -1 : 1);

	public void Retry()
	{
		/*
		if(GAScript.Instance)
			GAScript.Instance.LevelRestart( PlayerPrefs.GetInt("levelNo", 1), AttemptsPerLevel.CurrentAttempts);
		*/
		
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		if(AudioManager.instance)
			AudioManager.instance.Play("Button");
		
		if(GAScript.Instance)
			GAScript.Instance.LevelFail( LevelData().ToString());
	}

	public void NextLevel()
	{
		if (PlayerPrefs.GetInt("levelNo", 1) < SceneManager.sceneCountInBuildSettings - 1)
		{
			if (PlayerPrefs.GetInt("lastBuildIndex", 2) < SceneManager.sceneCountInBuildSettings - 1)
				PlayerPrefs.SetInt("lastBuildIndex", PlayerPrefs.GetInt("lastBuildIndex", 2) + 1);
			else
				PlayerPrefs.SetInt("lastBuildIndex", Random.Range(2, SceneManager.sceneCountInBuildSettings));
			SceneManager.LoadScene(1);
		}
		else
		{
			var x = Random.Range(2, SceneManager.sceneCountInBuildSettings);
			PlayerPrefs.SetInt("lastBuildIndex", x);
			SceneManager.LoadScene(1);
		}
		
		
		PlayerPrefs.SetInt("levelNo", PlayerPrefs.GetInt("levelNo", 1) + 1);
		ShopStateController.ShopStateSerializer.SaveCurrentState();
		
		if(AudioManager.instance)
			AudioManager.instance.Play("Button");
		Vibration.Vibrate(15);
		
		if(GAScript.Instance)
		{
			GAScript.Instance.LevelCompleted((LevelData()-1).ToString());
		}
	}

	private void OnTapToPlay()
	{
		metaCanvasPanel.SetActive(false);
		int val = PlayerPrefs.GetInt("levelNo", 1);

		if (PlayerPrefs.GetInt("interviewSceneShowed") == 0)
		{
			val = 1;
		}
		else val++;
		
		if(GAScript.Instance)
			GAScript.Instance.LevelStart( val.ToString());
	}

	int LevelData()
	{
		int val = PlayerPrefs.GetInt("levelNo", 1);

		if (PlayerPrefs.GetInt("interviewSceneShowed") == 0)
		{
			val = 1;
		}
		else val++;

		return val;
	}

	private void OnGameLose(int _)
	{
		if(_currentState != GameState.Playing) return;
		
		_currentState = GameState.Lost;
		OnGameEnd(false);
		
		// if(SceneManager.GetActiveScene().name.Contains("Story Scene")) return;
		int val = PlayerPrefs.GetInt("levelNo", 1);
		
		if (PlayerPrefs.GetInt("interviewSceneShowed") == 0)
		{
			val = 1;
		}
		else val++;
		
		 /*if(GAScript.Instance)
			GAScript.Instance.LevelFail( val.ToString());*/
	}

	private void OnGameEnd()
	{
		if(_currentState != GameState.Playing) return;
		
		_currentState = GameState.Won;
		OnGameEnd(true);
		
		int val = PlayerPrefs.GetInt("levelNo", 1);
		
		if (PlayerPrefs.GetInt("interviewSceneShowed") == 0)
		{
			val = 1;
		}
		else val++;
		
		 /*if(GAScript.Instance)
			GAScript.Instance.LevelCompleted( val.ToString());*/
	}

	private void OnAlterBankBalance(int oldBalance, int newBalance) =>
		UpdateBankBalanceText(oldBalance, newBalance, bankBalanceText);

	private void OnAlterRichRank(int oldRank, int newRank) =>
		UpdateRichRankText(oldRank, newRank, playerRichRankText);
	
}