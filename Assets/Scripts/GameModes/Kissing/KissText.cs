using DG.Tweening;
using UnityEngine;

public class KissText : MonoBehaviour
{
	[SerializeField] private GameObject kissText;
	[SerializeField] private GameObject tapToPlayToText;

	private void OnEnable()
	{
		GameEvents.TapToPlay += OnTapToPlay;
	}

	private void OnDisable()
	{
		GameEvents.TapToPlay -= OnTapToPlay;
	}

	private void OnTapToPlay()
	{
		DOVirtual.DelayedCall(5f, ()=>kissText.SetActive(false));
		DOVirtual.DelayedCall(5f, ()=>tapToPlayToText.SetActive(false));
	}
}
