using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace ShuffleCups
{
	public class Shuffler : MonoBehaviour
{
	public Sequence shuffleSequence;
	[SerializeField] private Transform[] slots, cups;
	
	[SerializeField] private Ease myEase = Ease.InOutSine;
	[SerializeField] private float zDistance;
	private int _attemptsMade;
	public float shuffleDuration = 0.5f;

	public void AssignBallToSlot(Transform[] balls)
	{
		var list = NewList(slots.Length);
		
		foreach (var ball in balls)
		{
			var dest = RandomPop(list);
			cups[dest].GetComponent<CupController>().AcceptBall(ball);
		}
	}

	public void DoShuffle(int noOfShuffles)
	{
		StartCoroutine(Shuffle(noOfShuffles));
	}
	
	private IEnumerator Shuffle(int noOfShuffles)
	{
		/*shuffleSequence = DOTween.Sequence();
		for (var i = 0; i < noOfShuffles; i++)
			while (!MakeMove(shuffleSequence))
				yield return null;*/
		
		yield return MyHelpers.GetWaiter(1f);
		for (var i = 0; i < noOfShuffles; i++)
		{
			MakeMove();
			//wait till shuffle is over
			yield return MyHelpers.GetWaiter(shuffleDuration);
		}
		
		GameEvents.Singleton.InvokeShuffleEnd();
	}
	
	private bool MakeMove()
	{
		var tempSet = NewList(slots.Length);
		return SwapCups(RandomPop(tempSet), RandomPop(tempSet));
	}
	
	private bool SwapCups(int a, int b)
	{
		//if(DOTween.TotalActiveTweens() > 0) return false;
		
		AudioManager.instance.Play("cup");
		
		var aPos = slots[a].position;
		var bPos = slots[b].position;
		
			//maintain duration and sequence insert at duration
		
		//perpendicular of 2D vector (x, y) = (-y, x)
		cups[a].DOMoveX(bPos.x, shuffleDuration).SetEase(myEase).OnPlay(() => AudioManager.instance.Play("shuffle"));
		cups[b].DOMoveX(aPos.x, shuffleDuration).SetEase(myEase);
		cups[b].DOMoveZ(aPos.z + zDistance, shuffleDuration * 0.5f).SetEase(myEase).SetLoops(2, LoopType.Yoyo);
		cups[a].DOMoveZ(bPos.z - zDistance, shuffleDuration * 0.5f).SetEase(myEase).SetLoops(2, LoopType.Yoyo);
		
		SwapSlots(a, b);
		
		return true;
	}
	
	private void SwapSlots(int a, int b)
	{
		var temp = slots[a];
		slots[a] = slots[b];
		slots[b] = temp;
	}

	private static int RandomPop(IList<int> list)
	{
		var idx = Random.Range(0, list.Count);
		var x = list[idx];
		list.RemoveAt(idx);
		return x;
	}

	private List<int> NewList(int size)
	{
		var list = new List<int>();
		int i = 0;
		while(size-- > 0)
			list.Add(i++);
		
		return list;
	}
}
}


