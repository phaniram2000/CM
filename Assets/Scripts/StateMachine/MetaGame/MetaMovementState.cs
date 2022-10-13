using StateMachine;
using UnityEngine;

public sealed class MetaMovementState : InputStateBase
{
	private static MetaPlayer _player;
	
	private Vector2 _startInputPosition;
	private static float _normalLength;
	private float _inputPosSetTime;
	private const float InputPosSetTimer = 1f;

	public MetaMovementState(MetaPlayer player, float normalisingLength)
	{
		_player = player;
		_normalLength = normalisingLength;
	}

	public MetaMovementState(Vector2 startInputPosition) => _startInputPosition = startInputPosition;

	public override void OnEnter()
	{
		base.OnEnter();
		
		_inputPosSetTime = Time.time;
	}

	public override void FixedExecute()
	{
		if(IsExitingCurrentState) return;
		if (InputExtensions.GetFingerUp())
		{
			ExitState();
			return;
		}

		HandleNewStartPosition();

		var direction = InputExtensions.GetInputPosition() - _startInputPosition;
		var distance = direction.magnitude;
		direction = direction.normalized;
		var direction3 = new Vector3(direction.x, 0f, direction.y);
		var desiredRot = Quaternion.LookRotation(direction3);
		
		_player.UpdatePlayer(Mathf.Clamp01(distance / _normalLength), desiredRot);
	}

	public override void OnExit()
	{
		base.OnExit();
		_player.StopMoving();
	}

	private void HandleNewStartPosition()
	{
		if(Time.time - _inputPosSetTime < InputPosSetTimer) return;

		_inputPosSetTime = Time.time;
		//_startInputPosition = Vector3.Lerp(_startInputPosition, InputExtensions.GetInputPosition(), 0.2f);
	}
}