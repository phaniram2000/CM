using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class PlayerRobot : MonoBehaviour
{
	public List<GameObject> aiHoldingItems = new();
	public Transform _AiItemsHoldingTransform;
	public RobotItemsHolder _AiItemsHolder;

	[Header("Number of Holding Items")]
	public int holdingItemsCount;

	[Header("----------------")]
	[Header("Loading Bar")]
	public SpriteRenderer fillerSprite;

	public float spriteFilledAmount;
	public bool isCollectingItems;
	GameManager gm;

	private void Start() => gm = GameManager.instance;

	private void Update()
	{
		holdingItemsCount = aiHoldingItems.Count;
		fillerSprite.enabled = isCollectingItems;
	}

	private void SellingItems(Collider other)
	{
		if (aiHoldingItems.Count <= 0) return;
		
		var place = other.GetComponent<TableManager>();
		if (aiHoldingItems.Count <= 0) return;
		
		var Currentobj = aiHoldingItems[aiHoldingItems.Count - 1];
		Currentobj.transform.parent = place.transform;
		Vector3 scale = new Vector3(0.25f, 0.25f, 0.25f);
		Currentobj.transform.DOScale(scale, 0.25f).SetEase(Ease.Linear).OnComplete(() =>
		{
			if (aiHoldingItems.Contains(Currentobj))
			{
				aiHoldingItems.Remove(Currentobj);
			}

			_AiItemsHolder.childObjs.Remove(Currentobj);
		});
	}

	public void AddObjectsTolist(Collider other, int num, int maxItems)
	{
		print("this feature is not working, if you want it to work, copy over code from PlayerMoneyCollect.cs." +
			  "\nAlso this is not how things work anymore. make the robot have slow movement or something");
		/*CollectableArea item = other.GetComponent<CollectableArea>();
		num = item.cloneParent.childCount - 1;
		if (item.availableItems.Count > 0)
		{
			if (aiHoldingItems.Count < maxItems)
			{
				if (!aiHoldingItems.Contains(item.availableItems[item.availableItems.Count - 1].gameObject))
				{
					aiHoldingItems.Add(item.availableItems[item.availableItems.Count - 1].gameObject);
					item.availableItems.Remove(item.availableItems[item.availableItems.Count - 1].gameObject);
				}

				foreach (GameObject obj in aiHoldingItems)
				{
					obj.transform.parent = _AiItemsHoldingTransform;
				}
			}
		}*/
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("CollectingArea"))
		{
			spriteFilledAmount = 360f;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("CollectingArea"))
		{
			isCollectingItems = false;
			spriteFilledAmount = 360f;
		}

		if (other.gameObject.CompareTag("CollectingArea") || other.gameObject.CompareTag("SellingArea"))
		{
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("CollectingArea"))
		{
			if (holdingItemsCount < gm.playerRobotCapacity)
			{
				spriteFilledAmount -= (360f * gm.playerCollectingSpeed) * Time.deltaTime;
				fillerSprite.material.SetFloat("_Arc1", spriteFilledAmount);
				isCollectingItems = true;
			}
			else
			{
				spriteFilledAmount = 360f;
				isCollectingItems = false;
			}

			if (spriteFilledAmount <= 0)
			{
				AddObjectsTolist(other, holdingItemsCount, gm.playerRobotCapacity);
				CollectableArea cc = other.GetComponent<CollectableArea>();
				if (cc.points.Count > cc.availableSlotCount)
					cc.availableSlotCount++;
				spriteFilledAmount = 360f;
			}

			_AiItemsHolder.isSelling = false;
		}

		if (other.gameObject.CompareTag("SellingArea"))
		{
			SellingItems(other);
			_AiItemsHolder.isSelling = true;
		}
	}
}