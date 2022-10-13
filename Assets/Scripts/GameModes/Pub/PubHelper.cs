using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PubHelper : HelperBase
{
	[SerializeField] private bool showPushingCinematic;
	[SerializeField] private List<PubNpc> npcQueue;
	[SerializeField] private Bouncer bouncer;
	[SerializeField] private float queueMoveDelay = 0.5f;

	[SerializeField] private Transform gameplayCameraSource, gameplayCameraDest;

	public static bool IsFirstInQueue(PubNpc pubNpc) => GetFirstInQueue == pubNpc;
	public static Bouncer GetBouncer => Get.bouncer;
	public static PubNpc GetFirstInQueue => Get.npcQueue.Count == 0 ? null : Get.npcQueue[0];

	public static bool GetIfShowPushingCinematic => Get.showPushingCinematic;
	private static PubHelper Get { get; set; }


	private void OnEnable()
	{
		PubEvents.MoveQueueAhead += OnMoveQueueAhead;
		PubEvents.StartInput += OnStartInput;
	}

	private void OnDisable()
	{
		PubEvents.MoveQueueAhead -= OnMoveQueueAhead;
		PubEvents.StartInput -= OnStartInput;
	}

	private void Awake()
	{
		if (!Get) Get = this;
		else Destroy(gameObject);
	}

	private void SetGameplayCamera()
	{
		gameplayCameraDest.position = gameplayCameraSource.position;
		gameplayCameraDest.rotation = gameplayCameraSource.rotation;
	}

	private void OnStartInput() => SetGameplayCamera();

	private void OnMoveQueueAhead()
	{
		var raju = npcQueue[0];
		npcQueue.RemoveAt(0);

		raju.StartFollowing();

		var cumulative = 0f;
		foreach (var npc in npcQueue)
		{
			cumulative += queueMoveDelay;
			DOVirtual.DelayedCall(cumulative, npc.StartFollowing);
		}
	}
}