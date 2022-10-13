using StateMachine;
using UnityEngine;

public class BlackmailingInputHandler : AInputHandler
{
	[SerializeField] private Blackmailer blackmailer;
	[SerializeField] private float intervalBetweenShots = 0.5f;
	[HideInInspector] public float tempIntervalBetweenShots;
	[SerializeField] private float intervalMultiplier = 0.02f;
	
	private static PictureClickingState _pictureClickingState;

	protected override void OnEnable()
	{
		base.OnEnable();
		BlackmailingEvents.GotFooled += DisableStateInput;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		BlackmailingEvents.GotFooled -= DisableStateInput;
	}

	protected override void InitialiseDerivedState()
	{
		_pictureClickingState = new PictureClickingState(blackmailer, this);
		ResetInterval();
	}

	protected override InputStateBase HandleInput()
	{
		if (HasNoInput()) return CurrentInputState;

		var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
		if (!Physics.Raycast(ray, out var hit, InputStateBase.RaycastDistance)) return CurrentInputState;
			
		if(hit.collider.CompareTag("BlackmailCollider")) return _pictureClickingState;
			
		return CurrentInputState;
	}

	public void StartInterval()
	{
		tempIntervalBetweenShots -= (intervalMultiplier * Time.deltaTime);
	}

	public void ResetInterval()
	{
		tempIntervalBetweenShots = intervalBetweenShots;
	}

	private void DisableStateInput()
	{
		AssignNewState(InputState.Disabled);
	}
}
