using UnityEngine;


namespace ShuffleCups
{
	public class MaskSetter : MonoBehaviour
	{
		[SerializeField] private GameObject squareSignMesh, triangleSignMesh, circleSignMesh;

		private void OnEnable()
		{
			GameEvents.Singleton.SetSquidSign += SetSquidSign;
		}

		private void OnDisable()
		{
			GameEvents.Singleton.SetSquidSign -= SetSquidSign;
		}
	
		public void SetSquidSign(SquidSign levelSquidSign)
		{
			if(!squareSignMesh) return;
		
			squareSignMesh.SetActive(false);
			triangleSignMesh.SetActive(false);
			circleSignMesh.SetActive(false);
		
			switch (levelSquidSign)
			{
				case SquidSign.Square:
					squareSignMesh.SetActive(true);
					break;
				case SquidSign.Triangle:
					triangleSignMesh.SetActive(true);
					break;
				case SquidSign.Circle:
					circleSignMesh.SetActive(true);
					break;
			}
		}
	}
}


