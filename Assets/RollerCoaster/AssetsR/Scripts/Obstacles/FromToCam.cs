using DG.Tweening;
using Kart;
using UnityEngine;

public class FromToCam : MonoBehaviour
{
	[SerializeField] private Collider fromTrigger, toTrigger;
	[SerializeField] private Transform startTransform, endTransform;
	[SerializeField] private float cameraOutTransitionDuration = 3f;
	[SerializeField] private float duration = 1f;

	private Transform _toTransform, _player, _cameraTarget;
	private Vector3 _fromTriggerPosition, _toTriggerPosition, _fromToTriggerVector;
	private Vector3 _fromCameraPosition, _toCameraPosition;

	private float _fromToTriggerDistance, _fromFromTriggerDot;
	private bool _hasEntered, _hasExited;
	
	private void Start()
	{
		_toTransform = toTrigger.transform;

		_fromCameraPosition = startTransform.position;
		_toCameraPosition = endTransform.position;
		
		_fromTriggerPosition = fromTrigger.transform.position;
		_toTriggerPosition = _toTransform.position;

		_fromToTriggerVector = _toTriggerPosition - _fromTriggerPosition;
		_fromToTriggerVector.y = 0f;
		
		_fromFromTriggerDot = Vector3.Dot(_fromToTriggerVector, _fromToTriggerVector);
	}

	private void Update()
	{
		if(_hasExited) return;
		if(!_hasEntered) return;
		
		var fromPlayerVector = _player.position - _fromTriggerPosition;
		fromPlayerVector.y = 0;
		
		var desiredPos = Vector3.Lerp(startTransform.position, endTransform.position, Mathf.InverseLerp(0f, _fromFromTriggerDot,
			Vector3.Dot(fromPlayerVector, _fromToTriggerVector)));

		_cameraTarget.position = Vector3.Lerp(_cameraTarget.position, desiredPos, Time.deltaTime * DampCameraR.only.lerpMul);
		_cameraTarget.rotation = Quaternion.Lerp(_cameraTarget.rotation, startTransform.rotation, Time.deltaTime * DampCameraR.only.lerpMul);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(!other.CompareTag("Player")) return;

		if(!_hasEntered)
		{
			_hasEntered = true;
			_player = other.transform;
			_cameraTarget = DampCameraR.only.TakeControlOfTarget();
			
			var initVal = DampCameraR.only.lerpMul;
			DampCameraR.only.lerpMul = 0f;
			DOTween.To(() => DampCameraR.only.lerpMul, value => DampCameraR.only.lerpMul = value, initVal, duration);
		}
		else
		{
			_hasExited = true;
			DampCameraR.only.ReleaseControlOfTarget(cameraOutTransitionDuration);
		}
	}
}