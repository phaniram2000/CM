using System;
using DG.Tweening;
using UnityEngine;

public class WireEndCube : MonoBehaviour
{
	public static event Action Oncount;
	public enum WireColor { Red, Green, Blue, Yellow}
	public bool didsnap;
	public Vector3 startpoint, startLocalPoint;
	public bool isAllowedToDrag = true;
	public WireColor myColor;
	public Camera _cam;
	public float _draggedObjectDistance;
	private Transform _draggedObject, _swappableTarget;
	[SerializeField] private float lerpSpeed, snapRadius = 0.5f, swapDuration = 0.5f;
	private bool DoesMatchColor(WireEndCube other) => other.myColor == myColor;
	public Transform movepoint;
	public ParticleSystem sparks;
	public GameObject tut;
	public bool TrySetSwappable(Transform target)
	{
		if (target == _draggedObject) return false;
		if (target == _swappableTarget) return true;
		_swappableTarget = target;
		return true;
	}
	public void Drag(Vector3 direction)
	{
		var camPosition = _cam.transform.position;
		var point = camPosition + (direction.normalized * (_draggedObjectDistance * 1));

		//if (_swappableTarget && Vector3.Distance(point, _swappableTarget.position) < snapRadius)
		_draggedObject.position = Vector3.Lerp(_draggedObject.position, point, lerpSpeed * Time.deltaTime);
		_draggedObject.localPosition = new Vector3(_draggedObject.localPosition.x, _draggedObject.localPosition.y,
			startLocalPoint.z);
	}
	public void StartDragging(Transform hitTransform, Vector3 rayOrigin)
	{
		startpoint = hitTransform.position;
		startLocalPoint = hitTransform.localPosition;
		DOTween.Kill(_draggedObject);
		_draggedObject = hitTransform;
		_draggedObjectDistance = Vector3.Distance(_draggedObject.position, rayOrigin);
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Draggable")&& other.GetComponent<WireEndCube>().myColor == myColor)
		{
			sparks.Play();
			Oncount?.Invoke();
			this.GetComponent<Collider>().enabled = false;
			other.GetComponent<Collider>().enabled = false;
			this.transform.SetParent(other.transform);
			other.transform.DOMove(movepoint.transform.position, 0.07f).SetEase(Ease.Linear);
			if(AudioManager.instance) 
				AudioManager.instance.Play("Shock");
			 tut.gameObject.SetActive(false);
			Vibration.Vibrate(30);

		}
	}
}