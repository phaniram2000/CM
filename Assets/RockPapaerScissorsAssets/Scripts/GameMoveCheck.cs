using UnityEngine;

namespace RPS
{



	public class GameMoveCheck : MonoBehaviour
	{
		private CharacterRefBank _playerCharacterRefBank;
		private NpcRefBank _npcRefBank;

		private GameMoves _playerMove, _npcMove;

		public static GameMoveCheck only;

		private void Awake()
		{
			if (!only) only = this;
			else Destroy(gameObject);
		}

		private void Start()
		{
			_playerCharacterRefBank =
				GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<CharacterRefBank>();
			_npcRefBank = GameObject.FindGameObjectWithTag("NpcCharacter").GetComponent<NpcRefBank>();
		}


		public void CheckMoves()
		{


			RPSGameEvents.InvokeOnGameMovesTextDisable();
			print("check move");
			//broooo ye kya hai itna if statements kuch soacho iska.

			if (_playerCharacterRefBank.Controller.MyCurrentMove == _npcRefBank.Controller.MyCurrentMove)
			{
				print("Game tie");
				RPSGameEvents.InvokeOnGameTie();
				return;
			}


			if (_playerCharacterRefBank.Controller.MyCurrentMove == GameMoves.Rock)
			{

				if (_npcRefBank.Controller.MyCurrentMove == GameMoves.Scissor)
				{
					RPSGameEvents.InvokeOnPlayerWin();
					RPSGameEvents.InvokeOnNpcLose();
					print("player win");
					return;
				}


				if (_npcRefBank.Controller.MyCurrentMove == GameMoves.Paper)
				{
					RPSGameEvents.InvokeOnPlayerLose();
					RPSGameEvents.InvokeOnNpcWin();
					print("npc win");
					return;
				}

			}

			if (_playerCharacterRefBank.Controller.MyCurrentMove == GameMoves.Paper)
			{
				if (_npcRefBank.Controller.MyCurrentMove == GameMoves.Rock)
				{
					RPSGameEvents.InvokeOnPlayerWin();
					RPSGameEvents.InvokeOnNpcLose();
					print("player win");
					return;
				}

				if (_npcRefBank.Controller.MyCurrentMove == GameMoves.Scissor)
				{
					RPSGameEvents.InvokeOnPlayerLose();
					RPSGameEvents.InvokeOnNpcWin();
					print("npc win");
					return;
				}

			}


			if (_playerCharacterRefBank.Controller.MyCurrentMove == GameMoves.Scissor)
			{
				if (_npcRefBank.Controller.MyCurrentMove == GameMoves.Paper)
				{
					RPSGameEvents.InvokeOnPlayerWin();
					RPSGameEvents.InvokeOnNpcLose();
					print("player win");
					return;
				}

				if (_npcRefBank.Controller.MyCurrentMove == GameMoves.Rock)
				{
					RPSGameEvents.InvokeOnPlayerLose();
					RPSGameEvents.InvokeOnNpcWin();
					print("npc win");
					return;
				}

			}

		}

	}

}
