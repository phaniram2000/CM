using System;
using Cinemachine;
using UnityEngine;

[Serializable] public class ContinuousImpulse
{
	[HideInInspector] public Transform host;
	public bool active;

	[CinemachineImpulseDefinitionProperty]
	public CinemachineImpulseDefinition impulseDefinition = new();

	private float _lastEventTime = 0;

	public void Rumble()
	{
		if(!active) return;
		
		var now = Time.time;
		var eventLength = impulseDefinition.m_TimeEnvelope.m_AttackTime + impulseDefinition.m_TimeEnvelope.m_SustainTime;
		if (now - _lastEventTime < eventLength) return;

		impulseDefinition.CreateEvent(host.transform.position, Vector3.down);
		_lastEventTime = now;
		Vibration.Vibrate(20);
	}
}