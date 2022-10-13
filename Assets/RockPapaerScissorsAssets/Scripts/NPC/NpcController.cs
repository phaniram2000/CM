using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace RPS
{

	public class NpcController : MonoBehaviour
	{
		private NpcRefBank _my;

		private GameMoves _myCurrentMove;
		[SerializeField] private List<int> levelGameRigList;
		[SerializeField] private bool useRandomizeNpcBehaviour;

		[SerializeField] private GameObject tortillaHolder,
			indicationGameObject,
			slapParticleEffect,
			spitParticleEffect,
			drinkingGlass;

		[SerializeField] private HealthCanvas healthCanvas;
		[SerializeField] private GameObject happyEmoji, sadEmoji;
		[SerializeField] private List<GameObject> movesTextList;
		[SerializeField] private bool isGirl, isBoy, shouldFallAfterFail;
		[SerializeField] private float npcSlapMin, npcSlapMax;
		private float _health = 1f;
		private float _damageToNpc;
		public GameMoves MyCurrentMove => _myCurrentMove;



		public float DamageToNpc
		{
			get => _damageToNpc;
			set => _damageToNpc = value;
		}

		public bool IsGirl => isGirl;

		public bool IsBoy => isBoy;

		public GameObject HappyEmoji => happyEmoji;

		public GameObject SadEmoji => sadEmoji;

		private void OnEnable()
		{
			RPSGameEvents.MoveSelectedByPlayer += OnMoveSelectedByPlayer;
			RPSGameEvents.NewRound += OnNewRound;
			RPSGameEvents.PlayerGaveSlap += OnPlayerGaveSlap;
			RPSGameEvents.NpcWin += OnNpcWin;
			RPSGameEvents.GameLose += OnGameLose;
			RPSGameEvents.OnTapToPlay += OnTapToPlay;
			RPSGameEvents.GameMovesTextEnable += OnGameMovesTextEnable;
			RPSGameEvents.GameMovesTextDisable += OnGameMovesTextDisable;
		}

		private void OnDisable()
		{
			RPSGameEvents.MoveSelectedByPlayer -= OnMoveSelectedByPlayer;
			RPSGameEvents.NewRound -= OnNewRound;
			RPSGameEvents.PlayerGaveSlap -= OnPlayerGaveSlap;
			RPSGameEvents.NpcWin -= OnNpcWin;
			RPSGameEvents.GameLose -= OnGameLose;
			RPSGameEvents.OnTapToPlay -= OnTapToPlay;
			RPSGameEvents.GameMovesTextEnable -= OnGameMovesTextEnable;
			RPSGameEvents.GameMovesTextDisable -= OnGameMovesTextDisable;
		}

		private void Start()
		{
			_my = GetComponent<NpcRefBank>();
			tortillaHolder.SetActive(true);
			DisableMovesTextList();
			if (slapParticleEffect)
				slapParticleEffect.SetActive(false);

			if (spitParticleEffect)
				spitParticleEffect.SetActive(false);
		}


		private void DisableMovesTextList()
		{
			foreach (var t in movesTextList)
			{
				t.SetActive(false);
			}
		}

		private void SelectNpcMove()
		{
			int random = Random.Range(0, 3);
			switch (random)
			{
				case 0:
					_myCurrentMove = GameMoves.Rock;
					break;
				case 1:
					_myCurrentMove = GameMoves.Paper;
					break;
				case 2:
					_myCurrentMove = GameMoves.Scissor;
					break;
			}

			RPSGameEvents.InvokeOnCameraZoom();


		}

		private void GiveMeWinMove(GameMoves playerMove)
		{
			switch (playerMove)
			{
				case GameMoves.Rock:
					_myCurrentMove = GameMoves.Paper;
					break;
				case GameMoves.Paper:
					_myCurrentMove = GameMoves.Scissor;
					break;
				case GameMoves.Scissor:
					_myCurrentMove = GameMoves.Rock;
					break;
			}

			RPSGameEvents.InvokeOnCameraZoom();
		}

		private void GiveMeLoseMove(GameMoves playerMove)
		{
			switch (playerMove)
			{
				case GameMoves.Rock:
					_myCurrentMove = GameMoves.Scissor;
					break;
				case GameMoves.Paper:
					_myCurrentMove = GameMoves.Rock;
					break;
				case GameMoves.Scissor:
					_myCurrentMove = GameMoves.Paper;
					break;
			}

			RPSGameEvents.InvokeOnCameraZoom();
		}

		private void GiveMeDrawMove(GameMoves playerMove)
		{
			_myCurrentMove = playerMove;
			RPSGameEvents.InvokeOnCameraZoom();
		}

		private void NpcRiggedMove(int val, GameMoves playerMove)
		{
			//0 means player lose,1 wins player wins.
			switch (val)
			{
				case 0:
					GiveMeWinMove(playerMove);
					break;
				case 1:
					GiveMeLoseMove(playerMove);
					break;
				case -1:
					GiveMeDrawMove(playerMove);
					break;
			}
		}

		private void OnMoveSelectedByPlayer(GameMoves playerMove)
		{
			if (useRandomizeNpcBehaviour)
			{
				SelectNpcMove();
				return;
			}

			print("current round number: " + GameFlowController.only.CurrentRoundNumber);

			if (GameFlowController.only.CurrentRoundNumber >= levelGameRigList.Count)
			{
				SelectNpcMove();
				return;
			}

			int indexVal = levelGameRigList[GameFlowController.only.CurrentRoundNumber];
			print("indexval: " + indexVal);
			NpcRiggedMove(indexVal, playerMove);

		}



		private void OnNewRound()
		{
			DOVirtual.DelayedCall(0.8f, () =>
			{
				if (spitParticleEffect)
					spitParticleEffect.SetActive(false);
			});

			if (slapParticleEffect)
				slapParticleEffect.SetActive(false);

			DisableEmoji();
		}

		public void EnableTortilla()
		{
			tortillaHolder.SetActive(true);
		}

		private void GiveDamage()
		{
			if (_my.isdead) return;

			_health -= _damageToNpc;
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
			RPSGameEvents.InvokeOnGameWin();
			GameEvents.InvokeGameWin();
		}

		public void ResetHealthAfterDie()
		{
			_health = 0f;
			healthCanvas.SetHealth(1f);
			healthCanvas.DisableCanvas();
		}

		private void OnPlayerGaveSlap()
		{
			GiveDamage();
			if (slapParticleEffect)
				slapParticleEffect.SetActive(true);
		}

		private void OnNpcWin()
		{
			//vrooooooo, kya hai ye bc.
			var random = Random.Range(npcSlapMin, npcSlapMax);
			var damage = random;

			print("Damget to player: " + damage);
			if (!_my.CharacterRefBank) return;

			if (!_my.CharacterRefBank.Controller) return;

			_my.CharacterRefBank.Controller.DamageToPlayer = damage;

		}

		private void OnGameLose()
		{
			//this means npc won
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


		public void OnNpcSpit()
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


	}

}
