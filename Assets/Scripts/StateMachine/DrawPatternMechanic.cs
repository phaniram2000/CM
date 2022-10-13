using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

namespace StateMachine
{
	[RequireComponent(typeof(LineRenderer))]
	public class DrawPatternMechanic : MonoBehaviour
	{
		[SerializeField] private Gradient rightPatternGradient, wrongPatternGradient;
		
		[Header("Fake Grid"), SerializeField] private List<Image> fakeCanvasGridItems;
		[SerializeField] private Sprite traversedSprite, untraversedSprite;

		private LineRenderer _lineRenderer;
		private List<Vector3> _pointCache;
		private List<Collider> _collidersChanged;
		private bool _isUsingTempPos;
		private Gradient _initGradient;

		private bool _isDoneHere;

		public List<Collider> GetCurrentPattern() => _collidersChanged;
		public int GetNodesTraversed => _collidersChanged.Count;

		private void OnEnable()
		{
			PatternMoneyEvents.CompletePatternStage += OnCompletePatternStage;
		}
	
		private void OnDisable()
		{
			PatternMoneyEvents.CompletePatternStage -= OnCompletePatternStage;
		}
		
		private void Start()
		{
			_lineRenderer = GetComponent<LineRenderer>();
			_collidersChanged = new();
			_pointCache = new();
			_initGradient = _lineRenderer.colorGradient;

			if (fakeCanvasGridItems.Count == 0 || fakeCanvasGridItems[0] == null) print("Uninitialised fake grid item list.");
			
			
			Vibration.Init();
		}

		public void TraverseGridItem(Collider newItem, Vector3 position, bool shouldCheck = true)
		{
			if(_isDoneHere) return;
			if(shouldCheck && _pointCache.Count > 1)
				CheckIfContainsItemBetween(position, newItem);

			PopTempPos();
			_pointCache.Add(position);
			_collidersChanged.Add(newItem);
			TraverseOnFakeGrid(newItem.transform.GetSiblingIndex());

			UpdateLine();
			
			if(AudioManager.instance)
				AudioManager.instance.Play("pop");
			Vibration.Vibrate(30);
		}

		public void ClearPattern(bool untraverseFakeGrid = true)
		{
			for (var i = 0; i < _collidersChanged.Count; i++)
			{
				_collidersChanged[i].tag = "PatternGridItem";
				if(untraverseFakeGrid)
					UnTraverseOnFakeGrid(i);
			}
			
			_lineRenderer.positionCount = 0;
			_lineRenderer.colorGradient = _initGradient;
			_pointCache.Clear();
			_collidersChanged.Clear();
		}

		public void AddTempPos(Vector3 pos)
		{
			if(_isDoneHere) return;
			if(_pointCache.Count == 0) return;

			if(_isUsingTempPos)
			{
				_pointCache[^1] = pos;
				UpdateLine();
				return;
			}

			_isUsingTempPos = true;
			_pointCache.Add(pos);
			UpdateLine();
		}

		public void PopTempPos()
		{
			if(!_isUsingTempPos) return;
			if(_pointCache.Count == 0) return;
			
			_isUsingTempPos = false;
			_pointCache.RemoveAt(_pointCache.Count - 1);
			UpdateLine();
		}

		public void FindRightPattern()
		{
			TweenLineColor(rightPatternGradient.Evaluate(0f));
			DOVirtual.DelayedCall(0.5f, () => TweenLineColor(_initGradient.Evaluate(0f)));
			ColorFakeGrid(rightPatternGradient.Evaluate(0f));
		}

		public void FindWrongPattern()
		{
			TweenLineColor(wrongPatternGradient.Evaluate(0f));
			DOVirtual.DelayedCall(0.5f, () => TweenLineColor(_initGradient.Evaluate(0f)));
			ColorFakeGrid(wrongPatternGradient.Evaluate(0f));
		}

		public bool IsAlreadyTraversed(Collider hitCollider)
		{
			if (_isDoneHere) return true;
			return _collidersChanged.Contains(hitCollider);
		}

		public Vector3 PeekPos() => _pointCache.Count > 0 ? _pointCache[^1] : Vector3.zero;

		private void CheckIfContainsItemBetween(Vector3 position, Collider newItem)
		{
			if (_pointCache.Count == 0) return;
			
			var dir = position - _collidersChanged[^1].transform.position;
			var ray = new Ray(_collidersChanged[^1].transform.position, dir);
			var hits = Physics.RaycastAll(ray, dir.magnitude);

			foreach (var hit in hits)
			{
				if(!hit.collider.CompareTag("PatternGridItem")) continue;
				if(hit.collider == newItem) continue;
				if(IsAlreadyTraversed(hit.collider)) continue;

				TraverseGridItem(hit.collider, hit.transform.position, false);
				return;
			}
		}

		private void UpdateLine()
		{
			_lineRenderer.positionCount = _pointCache.Count;
			_lineRenderer.SetPositions(_pointCache.ToArray());
		}

		private void TweenLineColor(Color to)
		{
			var color = _lineRenderer.colorGradient.Evaluate(0f);
			var current = new Color2(color, color);

			_lineRenderer.DOColor(current, new Color2(to, to), 0.25f);
		}

		private void ColorFakeGrid(Color color)
		{
			foreach (var gridItem in fakeCanvasGridItems) gridItem.color = color;

			TweenFakeGridColor(Color.white, 0.5f);
		}

		private void TweenFakeGridColor(Color to, float time)
		{
			DOTween.Kill(this);
			
			var temp = 0f;
			DOTween.To(() => temp, value => temp = value, 1f, time)
				.OnUpdate(() =>
				{
					foreach (var gridItem in fakeCanvasGridItems)
						gridItem.color =
							Color.Lerp(gridItem.color, to, temp);
				})
				.SetDelay(0.25f)
				.SetTarget(this);
		}

		private void TraverseOnFakeGrid(int siblingIndex) => fakeCanvasGridItems[siblingIndex].sprite = traversedSprite;

		private void UnTraverseOnFakeGrid(int siblingIndex)
		{
			fakeCanvasGridItems[siblingIndex].sprite = untraversedSprite;
			fakeCanvasGridItems[siblingIndex].color = Color.white;
		}
		
		private void OnCompletePatternStage()
		{
			if(GameRules.GetGameMode != GameMode.PhoneUnlockPattern) return;
			
			TweenFakeGridColor(Color.clear, .5f);
			DOVirtual.DelayedCall(.5f, () => ClearPattern(false));
			_isDoneHere = true;
		}
	}
}