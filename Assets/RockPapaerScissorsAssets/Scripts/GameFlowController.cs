using DG.Tweening;

using UnityEngine;

namespace RPS
{
	public enum GameMoves
	{
		Rock,
		Paper,
		Scissor
	}

	public class GameFlowController : MonoBehaviour
	{
		public static GameFlowController only;

		private int _currentRoundNumber;

		[HideInInspector] public bool isGameWin, isGameLose;

		public int CurrentRoundNumber
		{
			get => _currentRoundNumber;
			set => _currentRoundNumber = value;
		}

		private void Awake()
		{
			if (!only) only = this;
			else Destroy(gameObject);

			DOTween.KillAll();
		}

		private void OnEnable()
		{
			RPSGameEvents.GameTie += OnGameTie;
			RPSGameEvents.NewRound += OnNewRound;
			RPSGameEvents.GameWin += OnGameWin;
			GameEvents.TapToPlay += OnTapToPlay;
		}

		private void OnDisable()
		{
			RPSGameEvents.GameTie -= OnGameTie;
			RPSGameEvents.NewRound -= OnNewRound;
			RPSGameEvents.GameWin -= OnGameWin;
			GameEvents.TapToPlay -= OnTapToPlay;
		}

		


		private void Start()
		{
			_currentRoundNumber = 0;

			Vibration.Init();
		}

		private void OnGameTie()
		{
			DOVirtual.DelayedCall(3f, () => RPSGameEvents.InvokeOnNewRound());
		}

		private void OnNewRound()
		{
			_currentRoundNumber++;
		}

		private void OnGameWin()
		{

		}
		
		private void OnTapToPlay()
		{
			RPSGameEvents.InvokeOnTapToPlay();
		}

	}

}
