using UnityEngine;

public class Baton : MonoBehaviour
{
	private bool _foundThief;
	
	private void OnTriggerEnter(Collider other)
	{
		if (_foundThief) return; 
		
		if (!other.CompareTag("Player")) return;
		JewelleryHeistEvents.InvokeGotHitByTheBaton();
		_foundThief = true;
	}

	private void OnTriggerStay(Collider other)
	{
		if (_foundThief) return; 
		
		if (!other.CompareTag("Player")) return;
		JewelleryHeistEvents.InvokeGotHitByTheBaton();
		_foundThief = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if (_foundThief) return; 

		if (!other.CompareTag("Player")) return;
		JewelleryHeistEvents.InvokeGotHitByTheBaton();
		_foundThief = true;
	}
}
