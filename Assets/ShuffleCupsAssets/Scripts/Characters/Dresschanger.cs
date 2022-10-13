using System.Collections.Generic;
using UnityEngine;


namespace ShuffleCups
{
	[ExecuteInEditMode]
	public class Dresschanger : MonoBehaviour
	{
		public List<GameObject> dress;
    
		[Range(0, 70)]
		public int dressIndex;
	
		private void OnValidate()
		{
			for (int i = 0; i < dress.Count; i++)
			{
				dress[i].SetActive(i == dressIndex);
			}
		}
	}
}


