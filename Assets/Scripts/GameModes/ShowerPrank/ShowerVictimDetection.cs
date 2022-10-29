using DG.Tweening;
using UnityEngine;

public class ShowerVictimDetection : MonoBehaviour
{
	private bool _foundThePrankster;
	private ShoweringVictim _showeringVictim;
	
	private Transform _rootTransform;
	private void Start()
	{
		_showeringVictim = transform.root.GetComponent<ShoweringVictim>();
		
		_rootTransform = _showeringVictim.transform;
	}
	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player")) return;
		CheckForPrankster();
	}

	private void OnTriggerStay(Collider other)
	{
		if (!other.CompareTag("Player")) return;
		CheckForPrankster();
	}

	private void OnTriggerExit(Collider other)
	{
		if (!other.CompareTag("Player")) return;
		CheckForPrankster();
	}

	private void CheckForPrankster()
	{
		if (_foundThePrankster) return;

		if (!Prankster.IsPranking) return;
		
		ShowerPrankEvents.InvokeGotFoundPranking();
		DOVirtual.DelayedCall(3f,()=> GameCanvas.game.MakeGameResult(1,1));
		_foundThePrankster = true;
	}
}
