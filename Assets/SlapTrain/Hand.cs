using UnityEngine;

public class Hand : MonoBehaviour
{
	[SerializeField] private PlayerControlTrain player;
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Pedestrian"))
		{
			player.toSlap = true;
			if(AudioManager.instance)		
				AudioManager.instance.Play("Slap");
			Vibration.Vibrate(30);
		}
		
		if(other.CompareTag("Obstacle"))
			player.HitWithPole();

		if (other.CompareTag("PedestrianOnPhone"))
		{
			player.toSlap = true;
			var activePhone = other.GetComponent<Pedestrian>().GetActivePhone();
			//activePhone.transform.parent = player.transform;
			player.MoveThePhoneToBag(activePhone);
			Vibration.Vibrate(30);

			if(AudioManager.instance)		
				AudioManager.instance.Play("Slap");
			return;
		}
		
	}
}
