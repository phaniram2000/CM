using UnityEngine;


namespace ShuffleCups
{
	[ExecuteInEditMode]
	public class UVScaler : MonoBehaviour
	{
		int tiltingvalue;
		float A1, B1;
		float A2, B2;
	
		private void OnValidate()
		{
			A1 = 30;
			B1 = 12;
			A2 = transform.localScale.z;
			B2 = (A2 * B1) / A1;
			GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale=(new Vector2(1, -B2));
		}
	}

}



