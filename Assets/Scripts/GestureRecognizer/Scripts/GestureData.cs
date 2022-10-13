using System.Collections.Generic;
using UnityEngine;

namespace GestureRecognizer 
{
	/// <summary>
	/// Classes to store gesture lines.
	/// </summary>

	[System.Serializable]
	public class GestureLine 
	{
		public List<Vector2> points = new();
		public bool closedLine;
	}

	[System.Serializable]
	public class GestureData 
	{
		//Copy Constructor
		public GestureData() {}
		public GestureData(GestureData data) => lines = data.lines;
		
		public List<GestureLine> lines = new();

		public GestureLine LastLine => lines [^1];
	}
}