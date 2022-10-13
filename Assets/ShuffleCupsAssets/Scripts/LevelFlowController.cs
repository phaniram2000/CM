using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShuffleCups
{
	public class LevelFlowController : MonoBehaviour
{
	public static LevelFlowController only;

	public Texture2D[] emojis;
	public bool isHatLevel;

	[SerializeField] private SquidSign levelSquidSign;
	[SerializeField] private Transform[] balls;
	[SerializeField] private int noOfShuffles;

	[SerializeField] public GameObject[] winParticles;
	
	public Transform winSuitcasePos;
	private int _ballCount, _attemptsMade, _correctGuesses;
	private bool _canChooseCup;

	private Camera _mainCam;
	private Shuffler _shuffler;

	private void OnEnable()
	{
		GameEvents.Singleton.ShuffleStart += OnShuffleStart;
		GameEvents.Singleton.ShuffleEnd += OnShuffleEnd;
		
		GameEvents.Singleton.GameResult += OnGameResult;
	}

	private void OnDisable()
	{
		GameEvents.Singleton.ShuffleStart -= OnShuffleStart;
		GameEvents.Singleton.ShuffleEnd -= OnShuffleEnd;
		
		GameEvents.Singleton.GameResult -= OnGameResult;
	}

	private void Awake()
	{
		if (!only) only = this;
		else Destroy(gameObject);
	}

	private void Start()
	{
		Vibration.Init();
		_shuffler = GetComponent<Shuffler>();
		_mainCam = Camera.main;
		
		_ballCount = balls.Length;
		_attemptsMade = 0;
		
		GameEvents.Singleton.InvokeSetSquidSign(levelSquidSign);
		//AudioManager.instance.Play("kokoa");
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

		//print($"can choose cup {_canChooseCup}");
		if (!_canChooseCup) return;
		
		if (!InputExtensions.GetFingerDown()) return;
		
		var ray = _mainCam.ScreenPointToRay(InputExtensions.GetInputPosition());
		
		if (!Physics.Raycast(ray.origin, ray.direction, out var hitInfo)) return;
		
		if (!hitInfo.collider.CompareTag("Cup")) return;
		
		CheckIfWin(hitInfo.transform);
	}

	private void OnValidate()
	{
		//GameObject.FindGameObjectWithTag("Enemy").GetComponent<MaskSetter>()?.SetSquidSign(levelSquidSign);
	}

	private void CheckIfWin(Transform hit)
	{
		Vibration.Vibrate(15);
		
		var cup = hit.GetComponent<CupController>();
		GameEvents.Singleton.InvokePlayerTapCup(cup);
	}

	private void OnShuffleStart()
	{
		_shuffler.AssignBallToSlot(balls);
		_shuffler.DoShuffle(noOfShuffles);
	}

	private void OnShuffleEnd()
	{
		StartCoroutine(ConcludeShuffle());
	}
	
	private IEnumerator ConcludeShuffle()
	{
		//yield return new WaitUntil(() => DOTween.TotalActiveTweens() == 0);
		yield return MyHelpers.GetWaiter(0.5f);
		_canChooseCup = true;
	}

	public void ProcessCupSelectionResult(bool result)
	{
		if (!result)
		{
			GameEvents.Singleton.InvokeGameResult(false);
			return;
		}

		_correctGuesses++;
		if (--_ballCount > 0) return;
		
		GameEvents.Singleton.InvokeGameResult(true);
	}
	
	private void OnGameResult(bool didWin)
	{
		_canChooseCup = false;
		if (didWin)
			DOTween.Sequence().AppendInterval(5f).AppendCallback(() =>
			{
				AudioManager.instance.Play("confetti");
				AudioManager.instance.Play("fatake");
				foreach (var particle in winParticles)
					particle.SetActive(true);
			});
	}
	
	public bool CanMakeNextAttempt()
	{
		print($"attempts {_attemptsMade + 1} <= {balls.Length}");
		return ++_attemptsMade <= balls.Length;
	}

	public int GetScore()
	{
		const int score = 50;

		if (_correctGuesses == 0)
			return score;

		return score + (int) (_correctGuesses / (float)_attemptsMade * 50);
	}
}

public enum SquidSign { Square, Circle, Triangle };
}



