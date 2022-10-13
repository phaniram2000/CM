using DG.Tweening;
using UnityEngine;

public class Father_DetectionChild : MonoBehaviour
{
	private bool _foundKissing;

	private KissingScene_Father _father;

	private void Start()
	{
		_father = transform.root.GetComponent<KissingScene_Father>();
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
			CheckOnKids();	
	}

	private void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("Player"))
			CheckOnKids();
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player"))
			CheckOnKids();	
	}
	
	private void CheckOnKids()
	{
		if (_foundKissing) return;

		if (!KissingChildren.StartedKissing) return;

		KissingEvents.InvokeGotFoundKissing();
		DOVirtual.DelayedCall(1f, () =>
			GameCanvas.game.MakeGameResult(1));
		_foundKissing = true;
	}

	
}
