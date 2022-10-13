using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEscapeCollider : MonoBehaviour
{
	private Collider _collider;
	[SerializeField] private bool isTriggered;

	
	private void OnTriggerEnter(Collider other)
	{
		if (isTriggered) return;
		
		if (!other.CompareTag("IKCubes")) return;
		LaserEscapeEvents.InvokeCrossedOneLaserGroup();
		LaserEscapeEvents.InvokeResetTargetPositions();
		isTriggered = true;
		gameObject.SetActive(false);
		print("hitWithCube");
	}
}
