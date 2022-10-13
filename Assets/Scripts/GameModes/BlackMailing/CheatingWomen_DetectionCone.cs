using UnityEngine;

public class CheatingWomen_DetectionCone : MonoBehaviour
{
	private bool _foundTheWitness;
	
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
			CheckForWitness();	
	}

	private void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("Player"))
			CheckForWitness();	
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player"))
			CheckForWitness();	
	}

	private void CheckForWitness()
	{
		if (_foundTheWitness) return;

		BlackmailingEvents.InvokeFoundTakingPictures();
		GameCanvas.game.MakeGameResult(1,1);
		_foundTheWitness = true;
	}
}
