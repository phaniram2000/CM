using UnityEngine;

namespace ShuffleCups
{
	public class FinishLineTrigger : MonoBehaviour
	{
		private bool _hasFinished;

		private void OnTriggerEnter(Collider other)
		{
			if(_hasFinished) return;
			if(!other.CompareTag("PaperCup")) return;
		
			if(other.transform.root.GetComponent<PaperPullerPlayer>().myData.isPlayer)
				PaperGameEvents.Singleton.InvokeCrossFinishLine();
			else
				PaperGameEvents.Singleton.InvokeAiCrossFinishLine();

			_hasFinished = true;
		}
	}
}



