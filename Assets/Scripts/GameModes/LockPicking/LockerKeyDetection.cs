using UnityEngine;

public class LockerKeyDetection : MonoBehaviour
{
	private void Update()
	{
		if (!InputExtensions.GetFingerDown()) return;
		CheckForKeyCode();
	}

	private void CheckForKeyCode()
	{
		var ray = new Ray(transform.position, Vector3.left * 2f);
		if (Physics.Raycast(ray, out var hit, 2f))
		{
			print(hit.collider.gameObject);
			
		}
		Debug.DrawLine(transform.position, Vector3.left * 2f, Color.black, 3f);
	}
}
