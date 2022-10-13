using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace GestureRecognizer
{
	public class DrawMechanic : MonoBehaviour
	{
		[SerializeField] private GameObject linePrefab;
		[SerializeField] private float minPathInterval = 0.2f, lineWidth = 0.09f;
		[SerializeField] private Transform drawArea;

		[SerializeField, Range(0f, 1f)] private float scoreToAccept = 0.8f;
		[SerializeField, Range(1, 10)] private int minLines = 1, maxLines = 2;
		[SerializeField] private RemoveStrategy removeStrategy;

		[SerializeField] private bool clearNotRecognizedLines;
		[SerializeField] private Color lineColor, dimColor;
		[SerializeField] private float dimDuration = 0.25f;

		protected AudioSource _audio;
		
		private Recognizer _recognizer;
		private LineRenderer _currentLine;

		private Color2 _lineGradient, _dimGradient;
		private Vector2 _lastPoint;
		private readonly GestureData _data = new();

		private static readonly Vector2 HalfVector = Vector2.one * 0.5f;
		private readonly List<Vector3> _rendererPoints = new();
		private static List<LineRenderer> _linesRenderers = new();

		private void OnValidate() => maxLines = Mathf.Max(minLines, maxLines);

		private void Start()
		{
			_audio = GetComponent<AudioSource>();
			_recognizer = GetComponent<Recognizer>();

			_lineGradient = new Color2(lineColor, lineColor);
			_dimGradient = new Color2(dimColor, dimColor);

			_currentLine = null;
			_linesRenderers = new List<LineRenderer>();
		}

		public void StartDrawing()
		{
			if (_data.lines.Count >= maxLines)
			{
				switch (removeStrategy)
				{
					case RemoveStrategy.RemoveOld:
						_data.lines.RemoveAt(0);
						_linesRenderers[0].gameObject.SetActive(false);
						_linesRenderers.RemoveAt(0);
						break;
					case RemoveStrategy.ClearAll:
						_data.lines.Clear();
						foreach (var lineRenderer in _linesRenderers)
							lineRenderer.gameObject.SetActive(false);
						_linesRenderers.Clear();
						break;
				}
			}

			DimLines();
			SpawnNewLine();

			_audio.Play();
		}

		public void Draw(RaycastHit hit)
		{
			var localPoint = hit.transform.InverseTransformPoint(hit.point);
			var point2 = new Vector2(localPoint.x, localPoint.y);

			if (DistanceFromLastPoint(point2) < minPathInterval) return;

			_lastPoint = point2;
			_data.LastLine.points.Add(NormalisePoint(point2));
				
			_rendererPoints.Add(localPoint);
			_currentLine.positionCount = _rendererPoints.Count;
			_currentLine.SetPositions(_rendererPoints.ToArray());
		}

		public virtual void StopDrawing()
		{
			BrightenLines();
			StartCoroutine(Recognise());
			
			_audio.Stop();
		}
		
		private void DimLines()
		{
			if(_linesRenderers.Count < 1) return;
			
			foreach (var line in _linesRenderers)
			{
				DOTween.Kill(line);
				line.DOColor(_lineGradient, _dimGradient, dimDuration);
			}
		}

		public void BrightenLines()
		{
			if(_linesRenderers.Count < 2) return;
			
			foreach (var line in _linesRenderers.Where(line => line.gameObject.activeSelf))
			{
				DOTween.Kill(line);
				line.DOColor(_dimGradient, _lineGradient, dimDuration);
			}
		}

		private void SpawnNewLine()
		{
			_rendererPoints.Clear();
			_lastPoint = Vector2.negativeInfinity;
			
			_currentLine = Instantiate(linePrefab, drawArea).GetComponent<LineRenderer>();
			_currentLine.positionCount = 0;
			_currentLine.DOColor(_dimGradient, _lineGradient, 0f);
			_currentLine.widthMultiplier = lineWidth;

			_linesRenderers.Add(_currentLine);
			_data.lines.Add(new GestureLine());
		}

		private IEnumerator Recognise()
		{
			var resultID = -1;
			for (var size = _data.lines.Count; size >= 1 && size >= minLines; size--)
			{
				//last [size] lines
				var sizedData = new GestureData()
				{
					lines = _data.lines.GetRange(_data.lines.Count - size, size)
				};
				
				RecognitionResult result = null;

				//run in another thread

				var thread = new System.Threading.Thread(() => result = _recognizer.Recognize(sizedData, false));
				thread.Start();
				while (thread.IsAlive)
					yield return null;

				if (result.gesture && result.score.score >= scoreToAccept)
				{
					resultID = Convert.ToInt32(result.gesture.id);
					
					var lineCount = result.gesture.gesture.lines.Count;
					
					if (clearNotRecognizedLines && _linesRenderers.Count > lineCount)
						for (var i = _linesRenderers.Count - lineCount - 1; i >= 0; i--)
						{
							_linesRenderers[i].gameObject.SetActive(false);
							_linesRenderers.RemoveAt(i);
							_data.lines.RemoveAt(i);
						}
					break;
				}
				else
				{
					resultID = -1;
				}
			}
			
			GameCanvas.game.SetRecognisedNumber(resultID);
			yield return null;
		}

		public void ClearDrawnLines()
		{
			foreach (var line in _linesRenderers) line.gameObject.SetActive(false);
		}

		private static Vector2 NormalisePoint(Vector2 point) => point + HalfVector;

		private float DistanceFromLastPoint(Vector2 newPoint) =>
			_lastPoint == Vector2.negativeInfinity ? float.PositiveInfinity : Vector2.Distance(new Vector2(_lastPoint.x, _lastPoint.y), newPoint);
	}
}