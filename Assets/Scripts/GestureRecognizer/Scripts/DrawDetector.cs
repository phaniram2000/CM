using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.Events;

namespace GestureRecognizer
{
	public enum RemoveStrategy
	{
		RemoveOld,
		ClearAll
	}
	/// <summary>
	/// Captures player drawing and call the Recognizer to discover which gesture player id.
	/// Calls 'OnRecognize' event when something is recognized.
	/// </summary>
	public class DrawDetector : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
	{
		public Recognizer recognizer;

		public UILineRenderer line;
		private List<UILineRenderer> linesRenderers;

		[Range(0f, 1f)]
		public float scoreToAccept = 0.8f;

		[Range(1, 10)]
		public int minLines = 1;

		public int MinLines
		{
			set => minLines = Mathf.Clamp(value, 1, 10);
		}

		[Range(1, 10)]
		public int maxLines = 2;

		public int MaxLines
		{
			set { maxLines = Mathf.Clamp(value, 1, 10); }
		}
		
		public RemoveStrategy removeStrategy;

		public bool clearNotRecognizedLines;

		public bool fixedArea = false;

		GestureData data = new GestureData();

		[System.Serializable]
		public class ResultEvent : UnityEvent<RecognitionResult>
		{
		}

		public ResultEvent OnRecognize;

		RectTransform rectTransform;


		void Start()
		{
			line.relativeSize = true;
			line.LineList = false;
			linesRenderers = new List<UILineRenderer>() { line };
			rectTransform = transform as RectTransform;
			UpdateLines();
		}

		void OnValidate()
		{
			maxLines = Mathf.Max(minLines, maxLines);
		}

		public void UpdateLines()
		{
			while (linesRenderers.Count < data.lines.Count)
			{
				var newLine = Instantiate(line, line.transform.parent);
				linesRenderers.Add(newLine);
			}

			for (int i = 0; i < linesRenderers.Count; i++)
			{
				linesRenderers[i].Points = new Vector2[] { };
				linesRenderers[i].SetAllDirty();
			}

			int n = Mathf.Min(linesRenderers.Count, data.lines.Count);
			for (int i = 0; i < n; i++)
			{
				linesRenderers[i].Points = data.lines[i].points.Select(RealToLine).ToArray();
				linesRenderers[i].SetAllDirty();
			}
		}

		Vector2 RealToLine(Vector2 position)
		{
			var local = rectTransform.InverseTransformPoint(position);
			var normalized = Rect.PointToNormalized(rectTransform.rect, local);
			return normalized;
		}

		Vector2 FixedPosition(Vector2 position)
		{
			return position;
			//var local = rectTransform.InverseTransformPoint (position);
			//var normalized = Rect.PointToNormalized (rectTransform.rect, local);
			//return normalized;
		}

		public void ClearLines()
		{
			data.lines.Clear();
			UpdateLines();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (data.lines.Count >= maxLines)
			{
				switch (removeStrategy)
				{
					case RemoveStrategy.RemoveOld:
						data.lines.RemoveAt(0);
						break;
					case RemoveStrategy.ClearAll:
						data.lines.Clear();
						break;
				}
			}

			data.lines.Add(new GestureLine());
			
			var fixedPos = FixedPosition(eventData.position);
			if (data.LastLine.points.Count != 0 && data.LastLine.points.Last() == fixedPos) return;
			
			data.LastLine.points.Add(fixedPos);
			UpdateLines();
		}

		public void OnDrag(PointerEventData eventData)
		{
			var fixedPos = FixedPosition(eventData.position);
			if (data.LastLine.points.Count != 0 && data.LastLine.points.Last() == fixedPos) return;
			data.LastLine.points.Add(fixedPos);
			UpdateLines();
		}

		public void OnEndDrag(PointerEventData eventData) => StartCoroutine(OnEndDragCoroutine(eventData));

		IEnumerator OnEndDragCoroutine(PointerEventData eventData)
		{
			data.LastLine.points.Add(FixedPosition(eventData.position));
			UpdateLines();

			for (int size = data.lines.Count; size >= 1 && size >= minLines; size--)
			{
				//last [size] lines
				var sizedData = new GestureData()
				{
					lines = data.lines.GetRange(data.lines.Count - size, size)
				};

				var sizedNormalizedData = sizedData;

				if (fixedArea)
				{
					var rect = this.rectTransform.rect;
					sizedNormalizedData = new GestureData()
					{
						lines = sizedData.lines.Select(gestureLine => new GestureLine()
						{
							closedLine = gestureLine.closedLine,
							points = gestureLine.points.Select(p => Rect.PointToNormalized(rect, this.rectTransform.InverseTransformPoint(p))).ToList()
						}).ToList()
					};
				}

				RecognitionResult result = null;

				//run in another thread

				var thread = new System.Threading.Thread(() => { result = recognizer.Recognize(sizedNormalizedData, normalizeScale: !fixedArea); });
				thread.Start();
				while (thread.IsAlive)
					yield return null;

				if (result.gesture && result.score.score >= scoreToAccept)
				{
					OnRecognize.Invoke(result);
					if (clearNotRecognizedLines)
					{
						data = sizedData;
						UpdateLines();
					}

					break;
				}
				else
					OnRecognize.Invoke(RecognitionResult.Empty);
			}

			yield return null;
		}
	}
}