using System;
using UnityEngine;

namespace ShuffleCups
{
	public class GameEvents : MonoBehaviour
	{
		public static GameEvents Singleton;

		public Action<SquidSign> SetSquidSign;
	
		public Action ShuffleStart, ShuffleEnd;

		public Action<CupController> PlayerTapCup;
		public Action<bool, CupController> MakeSelection;

		public Action<bool> GameResult;

		public Action ShowInstruction;
	
		private void Awake()
		{
			if (!Singleton) Singleton = this;
			else Destroy(gameObject);
		}

		public void InvokeSetSquidSign(SquidSign levelSquidSign)
		{
			SetSquidSign?.Invoke(levelSquidSign);
		}
	
		public void InvokeShuffleStart()
		{
			ShuffleStart?.Invoke();
		}

		public void InvokeShuffleEnd()
		{
			ShuffleEnd?.Invoke();
		}

		//this is used for player IK to go to cup and return
		public void InvokePlayerTapCup(CupController cup) => PlayerTapCup?.Invoke(cup);

		//this is used for running the dotween animation on the cup, chained/called by PlayerTapCup
		public void InvokeMakeSelection(bool isLeftHand, CupController cup)
		{
			MakeSelection?.Invoke(isLeftHand, cup);
		}

		public void InvokeGameResult(bool didWin)
		{
			GameResult?.Invoke(didWin);
		}

		public void InvokeShowInstruction()
		{
			ShowInstruction?.Invoke();
		}
	}
}


