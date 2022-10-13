using DG.Tweening;
using UnityEngine;

namespace StateMachine
{
	public class DragMechanic : MonoBehaviour
	{
		public bool isAllowedToDrag = true;
		[SerializeField] private float lerpSpeed, snapRadius = 0.5f, swapDuration = 0.5f;

		private bool _hasHidTutorial;
		[SerializeField] private SpriteRenderer tutorialHand;
		[SerializeField] private Transform tutorialDest;
		private Sequence _tutorialTween;

		private Transform _draggedObject, _swappableTarget;
		private Transform _draggedObjectParent, _swappableTargetParent;
		private float _draggedObjectDistance;

		private Camera _cam;

		private void OnEnable() => GameEvents.PressDoneButton += OnDoneButtonPress;

		private void OnDisable() => GameEvents.PressDoneButton -= OnDoneButtonPress;

		private void Start()
		{
			_cam = Camera.main;
			DOVirtual.DelayedCall(1f, PlayTutorialSequence);
		}

		private void OnDrawGizmosSelected()
		{
			if(!_swappableTarget) return;
			
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(_swappableTarget.position, snapRadius);
		}

		public void StartDragging(Transform hitTransform, Vector3 rayOrigin)
		{
			DOTween.Kill(_draggedObject);
			_draggedObject = hitTransform;
			_draggedObjectDistance = Vector3.Distance(_draggedObject.position, rayOrigin);
			_draggedObjectParent = hitTransform.parent;
			hitTransform.parent = null;
			
			if(AudioManager.instance)
				AudioManager.instance.Play("StartDrag");
			Vibration.Vibrate(20);
		}

		
		public void Drag(Vector3 direction)
		{
			var camPosition = _cam.transform.position;
			var point = camPosition + direction.normalized * (_draggedObjectDistance * 0.75f);
			var candidate = camPosition + direction.normalized * _draggedObjectDistance;

			if (_swappableTarget && Vector3.Distance(candidate, _swappableTarget.position) < snapRadius)
				point = _cam.transform.position + (_swappableTarget.position - camPosition).normalized * (_draggedObjectDistance * 0.75f);

			_draggedObject.position = Vector3.Lerp(_draggedObject.position, point, lerpSpeed * Time.deltaTime);
			
			
		}

		/// <summary>
		/// Returns if successfully has a swappable target after calling
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public bool TrySetSwappable(Transform target)
		{
			// if the candidate is the object being dragged itself
			if (target == _draggedObject) return false;

			// if this is already is the swappable i have, raycast did the right thing,
			// don't calculate again
			if (target == _swappableTarget) return true;

			// if you didn't have a swappable target yet, set it now
			// OR
			// in case that this target is not the old one
			_swappableTarget = target;
			_swappableTargetParent = target.parent;
			return true;
		}

		public void StopDragging()
		{
			if(!isAllowedToDrag) return;
			
			if (_swappableTarget)
			{
				//Swap Targets
				SendToSwapPos(_swappableTarget, _draggedObjectParent);
				SendToSwapPos(_draggedObject, _swappableTargetParent);

				ToiletHelper.GetAreSignsSwapped = !ToiletHelper.GetAreSignsSwapped;
				TryHideTutorial();

				if(AudioManager.instance)
					AudioManager.instance.Play("EndDrag");
				Vibration.Vibrate(20);
			}
			else
			{
				SendToInitPos(_draggedObject);

				if(AudioManager.instance)
					AudioManager.instance.Play("CancelDrag");
				Vibration.Vibrate(20);
			}
			
			_draggedObject = _swappableTarget = null;
			_draggedObjectParent = _swappableTargetParent = null;
			_draggedObjectDistance = float.NegativeInfinity;
			
			
		}

		private void SendToSwapPos(Transform swapTarget, Transform otherParent)
		{
			var swapTargetPosition = swapTarget.position;
			var swapDir = otherParent.position - swapTargetPosition;
			var camDir = (_cam.transform.position - swapTargetPosition).normalized;

			swapTarget.DOMove(swapTargetPosition + swapDir * 0.5f + camDir, swapDuration / 2)
				.SetEase(Ease.InQuart)
				.OnStart(() => SetUndraggable(swapTarget))
				.OnComplete(() =>
				{
					swapTarget.parent = otherParent;
					swapTarget.DOLocalRotate(Vector3.zero, swapDuration / 2);
					swapTarget.DOScale(Vector3.one, swapDuration / 2);
					swapTarget.DOLocalMove(Vector3.zero, swapDuration / 2)
						.SetEase(Ease.OutQuart)
						.OnComplete(() => SetDraggable(swapTarget));
				});
			
			if(AudioManager.instance)
				AudioManager.instance.Play("Swipe");
			
			GameEvents.InvokeOnShowDoneButton();
			isAllowedToDrag = false;
			Vibration.Vibrate(20);
		}

		private void SendToInitPos(Transform t)
		{
			t.parent = _draggedObjectParent;
			t.DOLocalMove(Vector3.zero, swapDuration)
				.OnStart(() => SetUndraggable(t))
				.OnComplete(() => SetDraggable(t));
		}

		private void TryHideTutorial()
		{
			if(_hasHidTutorial) return;
			
			_hasHidTutorial = true;
			_tutorialTween.Kill(true);
			tutorialHand.DOColor(Color.clear, .5f);
		}
		
		private void PlayTutorialSequence()
		{
			_tutorialTween = DOTween.Sequence();

			var localScale = tutorialHand.transform.localScale;
			_tutorialTween.Append(tutorialHand.transform.DOScale(localScale * 0.85f, 0.5f));
			_tutorialTween.Append(tutorialHand.transform.DOMove(tutorialDest.position, 2f));
			_tutorialTween.Append(tutorialHand.transform.DOScale(localScale, 0.5f));
			_tutorialTween.Join(tutorialHand.DOColor(Color.clear, 1f));

			_tutorialTween.SetLoops(-1, LoopType.Restart);
		}

		private static void SetDraggable(Component t) => t.tag = "Draggable";

		private static void SetUndraggable(Component t) => t.tag = "Untagged";

		private void OnDoneButtonPress()
		{
			isAllowedToDrag = false;
			if (AudioManager.instance)
			{
				AudioManager.instance.Play("CrowdTalking");
				DOVirtual.DelayedCall(3f, () => AudioManager.instance.Play("Door"));
				DOVirtual.DelayedCall(5f, () => AudioManager.instance.Pause("CrowdTalking"));
			}
		}
	}
}