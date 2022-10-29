using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace RPS
{

	public class CharacterController : MonoBehaviour
	{
		private CharacterRefBank _my;

		[SerializeField] private GameObject tortillaHolder,
			indicationGameObject,
			slapParticleEffect,
			spitParticleEffect,
			drinkingGlass,
			powerSlapParticleEffect;

		[SerializeField] private HealthCanvas healthCanvas;
		[SerializeField] private GameObject happyEmoji, sadEmoji;
		[SerializeField] private List<GameObject> movesTextList;
		[SerializeField] private bool isGirl, isBoy, shouldFallAfterFail, isHelmetFeatureOn;

		private float _health = 1f;
		private float _damageToPlayer;

		private GameMoves _myCurrentMove;

		private bool _isHelmetOn;

		private bool _iwin, _ilose;


		public bool IsGirl => isGirl;

		public bool IsBoy => isBoy;

		public GameMoves MyCurrentMove => _myCurrentMove;


		public GameObject HappyEmoji => happyEmoji;

		public GameObject SadEmoji => sadEmoji;

		public float DamageToPlayer
		{
			get => _damageToPlayer;
			set => _damageToPlayer = value;
		}

		public float Health => _health;

		private bool _powerSlapGiven;


		private void OnEnable()
		{
			RPSGameEvents.MoveSelectedByPlayer += OnMoveSelectedByPlayer;
			RPSGameEvents.AllowPlayerToSlap += OnAllowPlayerToSlap;
			RPSGameEvents.NewRound += OnNewRound;
			RPSGameEvents.NpcGaveSlap += OnNpcGaveSlap;
			RPSGameEvents.GameWin += OnGameWin;
			RPSGameEvents.OnTapToPlay += OnTapToPlay;
			RPSGameEvents.GameMovesTextEnable += OnGameMovesTextEnable;
			RPSGameEvents.GameMovesTextDisable += OnGameMovesTextDisable;
			RPSGameEvents.PlayerHelmetEnable += OnPlayerHelmetEnable;
			RPSGameEvents.IncreasePlayerHealth += OnInvokeInreasePlayerHealth;
			RPSGameEvents.PowerSlapGiven += OnPowerSlapGiven;
			RPSGameEvents.PlayerGaveSlap += OnPlayerGaveSlap;
		}

		private void OnDisable()
		{
			RPSGameEvents.MoveSelectedByPlayer -= OnMoveSelectedByPlayer;
			RPSGameEvents.AllowPlayerToSlap -= OnAllowPlayerToSlap;
			RPSGameEvents.NewRound -= OnNewRound;
			RPSGameEvents.NpcGaveSlap -= OnNpcGaveSlap;
			RPSGameEvents.GameWin -= OnGameWin;
			RPSGameEvents.OnTapToPlay -= OnTapToPlay;
			RPSGameEvents.GameMovesTextEnable -= OnGameMovesTextEnable;
			RPSGameEvents.GameMovesTextDisable -= OnGameMovesTextDisable;
			RPSGameEvents.PlayerHelmetEnable -= OnPlayerHelmetEnable;
			RPSGameEvents.IncreasePlayerHealth -= OnInvokeInreasePlayerHealth;
			RPSGameEvents.PowerSlapGiven -= OnPowerSlapGiven;
			RPSGameEvents.PlayerGaveSlap -= OnPlayerGaveSlap;
		}


		private void Start()
		{
			_my = GetComponent<CharacterRefBank>();
			happyEmoji.SetActive(false);
			sadEmoji.SetActive(false);
			tortillaHolder.SetActive(true);
			DisableMovesTextList();


			if (slapParticleEffect)
				slapParticleEffect.SetActive(false);

			if (spitParticleEffect)
				spitParticleEffect.SetActive(false);

			if (powerSlapParticleEffect)
				powerSlapParticleEffect.SetActive(false);
		}


		private void DisableMovesTextList()
		{
			foreach (var t in movesTextList)
			{
				t.SetActive(false);
			}
		}

		private void OnMoveSelectedByPlayer(GameMoves move)
		{
			_myCurrentMove = move;
		}

		private void OnAllowPlayerToSlap()
		{
			tortillaHolder.SetActive(true);
		}

		private void OnNewRound()
		{

			if (slapParticleEffect)
				slapParticleEffect.SetActive(false);

			DOVirtual.DelayedCall(0.8f, () =>
			{
				if (spitParticleEffect)
					spitParticleEffect.SetActive(false);
			});

			DisableEmoji();
			DisableMovesTextList();
		}

		private void GiveDamage()
		{
			if (_my.isdead) return;

			_health -= _damageToPlayer;
			healthCanvas.SetHealth(_health);

			if (_health > 0)
				return;

			DOVirtual.DelayedCall(0.25f, DieFromHealth);
			_my.isdead = true;

		}

		private void DieFromHealth()
		{
			//Die
			ResetHealthAfterDie();
			if (shouldFallAfterFail)
				_my.RagdollController.GoRagdoll();
			else
				_my.AnimationController.LoseAnim();

			RPSGameEvents.InvokeOnGameLose();
			GameEvents.InvokeGameLose(-1);

		}

		private void OnNpcGaveSlap()
		{
			if (slapParticleEffect)
				slapParticleEffect.SetActive(true);

			if (isHelmetFeatureOn)
				if (_isHelmetOn)
					return;

			GiveDamage();

		}

		public void ResetHealthAfterDie()
		{
			_health = 0f;
			healthCanvas.SetHealth(1f);
			healthCanvas.DisableCanvas();
		}

		private void OnGameWin()
		{

			ResetHealthAfterDie();
			_my.AnimationController.VictoryAnim();
		}

		private void OnTapToPlay()
		{
			indicationGameObject.SetActive(false);
			drinkingGlass.SetActive(false);
		}

		public void DisableEmoji()
		{
			happyEmoji.SetActive(false);
			sadEmoji.SetActive(false);
		}

		public void OnPlayerSpit()
		{
			if (_health > 0) return;


			if (spitParticleEffect)
				spitParticleEffect.SetActive(true);
		}

		private void OnGameMovesTextEnable()
		{
			if (movesTextList.Count < 3) return;

			switch (_myCurrentMove)
			{
				case GameMoves.Rock:
				{
					movesTextList[0].SetActive(true);
				}
					break;
				case GameMoves.Paper:
				{
					movesTextList[1].SetActive(true);
				}
					break;
				case GameMoves.Scissor:
				{
					movesTextList[2].SetActive(true);
				}
					break;
			}
		}

		private void OnGameMovesTextDisable()
		{
			DisableMovesTextList();
		}

		private void OnPlayerHelmetEnable()
		{
			_isHelmetOn = true;

		}

		public void OnPlayerHelmetFall()
		{
			_isHelmetOn = false;

		}

		private void OnInvokeInreasePlayerHealth(float value)
		{
			_health += value;
			healthCanvas.SetHealth(_health);
		}

		private void OnPowerSlapGiven()
		{
			_powerSlapGiven = true;
		}

		private void OnPlayerGaveSlap()
		{
			if (!powerSlapParticleEffect) return;

			if (!_powerSlapGiven) return;

			_powerSlapGiven = false;

			powerSlapParticleEffect.SetActive(true);

		}


	}

}
