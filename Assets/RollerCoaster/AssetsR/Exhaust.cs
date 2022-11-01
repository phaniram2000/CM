using UnityEngine;

public class Exhaust : MonoBehaviour
{
	[SerializeField] private GameObject exhaustParticleSystem;
	private void OnEnable()
	{
		GameEventsR.PlayerOnFever += ExhaustOn;
		GameEventsR.PlayerOffFever += ExhaustOff;
	}

	private void OnDisable()
	{
		GameEventsR.PlayerOnFever -= ExhaustOn;
		GameEventsR.PlayerOffFever -= ExhaustOff;		
	}
	
	private void ExhaustOn()
	{
		exhaustParticleSystem.SetActive(true);
	}

	private void ExhaustOff() => exhaustParticleSystem.SetActive(false);
}
