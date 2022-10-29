using UnityEngine;

namespace ShuffleCups
{
	public class NewMechanicTest : MonoBehaviour
	{
		public Transform paper;
		public float zPosMax, zPosMin, zScaleMax, zScaleMin;

		public float distanceFromZero = 1f;
	
		public float max = 50f, decreaseMultiplier = 1f, increaseMultiplier = 1f;
		public float sweetSpotFrom, sweetSpotTo;
		public float current = 0f;
	
		private void Start()
		{
			sweetSpotFrom = max * 0.5f;
			sweetSpotTo = max * 0.7f;
		}

		private void Update()
		{
			//begs for a StateMachine
			if (current > 0.1f)
				current -= Time.deltaTime * decreaseMultiplier;
		
			//goes into a canvas controller
		
			if(!InputExtensions.GetFingerHeld()) return;

			var delta = InputExtensions.GetInputDelta().y;
		
			if(delta > -0.01f) return;
		
			var old = current;
			current += Time.deltaTime * increaseMultiplier * -delta;
		
			distanceFromZero -= (current - old) * 0.015f;
		
			paper.localScale = new Vector3(paper.localScale.x, paper.localScale.y,
				Mathf.Lerp(zScaleMin, zScaleMax, distanceFromZero));
			paper.position = new Vector3(paper.position.x, paper.position.y,
				Mathf.Lerp(zPosMin, zPosMax, distanceFromZero));
		}
	}
}


