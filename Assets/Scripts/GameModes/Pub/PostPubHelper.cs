using DG.Tweening;
using StateMachine;
using UnityEngine;
using UnityEngine.UI;

public class PostPubHelper : MonoBehaviour
{
	[SerializeField] private Image blackOverlay;
	[SerializeField] private Transform bouncerDest, james, jamesDest;
	private bool _hasSetPositions;
	private Bouncer _bouncer;
	private PubJames _james;

	private void OnEnable()
	{
		GameEvents.TapToPlay += OnTapToPlay;
		PubEvents.StartPostPub += OnStartPostPub;
	}
	
	private void OnDisable()
	{
		GameEvents.TapToPlay -= OnTapToPlay;
		PubEvents.StartPostPub -= OnStartPostPub;
	}

	private void Start()
	{
		_bouncer = PubHelper.GetBouncer;
	}

	private void StartPostPub() => _bouncer.PushJames();

	private void GoBlackAndBack() => blackOverlay.DOColor(Color.black, 0.5f)
		.SetLoops(2, LoopType.Yoyo)
		.OnStepComplete(() =>
		{
			if (_hasSetPositions)
			{
				StartPostPub();
				return;
			}

			SetPositions();
		});

	private void SetPositions()
	{
		_bouncer.transform.position = bouncerDest.position;
		_bouncer.transform.rotation = bouncerDest.rotation;
		
		james.position = jamesDest.position;
		james.rotation = jamesDest.rotation;
		_hasSetPositions = true;
	}

	private static void OnTapToPlay() => AInputHandler.AssignNewState(InputState.Disabled);

	private void OnStartPostPub() => GoBlackAndBack();
}