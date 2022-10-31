using UnityEngine;

public class Beam : MonoBehaviour
{
	private bool _isTriggered;
	
    private void OnTriggerEnter(Collider other)
	{
		if (_isTriggered) return;
		
    	if (!other.CompareTag("Player")) return;
    	
    	GameManagerTrain.Instance.RemoveTheHeart();
		_isTriggered = true;
	}
}
