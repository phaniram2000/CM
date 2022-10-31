using DG.Tweening;
using UnityEngine;

public class EndOfLevelTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player")) return;

		GameEventsTrain.InvokeEndLevel();
	GameEvents.InvokeGameWin();
	//	DOVirtual.DelayedCall(2f, () => { GameManagerTrain.Instance.ShowWinUi(); });
	}
}
