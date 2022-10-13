using Dreamteck.Splines;
using UnityEngine;

public class SplineTriggerHelper : MonoBehaviour
{
	// TOILET spline triggers
	public void GetKicked(SplineUser npc)
	{
		if (!npc.TryGetComponent(out ToiletMale male)) return;
		if (!male.getsKickedOut) return;

		male.GetKickedOut();
	}

	public void ReachDoor(SplineUser npc)
	{
		if (!npc.TryGetComponent(out ToiletMale male)) return;

		male.ReachDoor();
	}

	// PUB spline triggers
	public void ReachQueueSlot(SplineUser npc)
	{
		if (!npc.TryGetComponent(out PubNpc pubNpc)) return;
		
		pubNpc.QueueStopMoving();

		if (!PubHelper.IsFirstInQueue(pubNpc)) return;

		pubNpc.TryEntry();
	}

	public void EnterPub()
	{
		PubEvents.InvokeMoveQueueAhead();
	}
}