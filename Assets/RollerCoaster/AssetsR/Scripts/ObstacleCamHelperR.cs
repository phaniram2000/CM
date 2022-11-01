using Kart;
using UnityEngine;

public class ObstacleCamHelperR : MonoBehaviour
{
	[SerializeField] private bool calledOnExit, isObstacleOnRight;
	private bool _doneOnce;

	private void Start() => GetComponent<MeshRenderer>().enabled = false;

	private void OnTriggerEnter(Collider other)
	{
		if(_doneOnce) return;
		if(!other.CompareTag("Player")) return;

		_doneOnce = true;
		if(calledOnExit)
			ReturnFromObstacleCam();
		else
		{
			SendToObstacleCam();
		}
	}

	private void SendToObstacleCam() => DampCameraR.only.SendToObstacleCam(isObstacleOnRight);

	private static void ReturnFromObstacleCam() => DampCameraR.only.CameraResetPosition();
}