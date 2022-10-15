using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CollectableArea : MonoBehaviour
{
	[SerializeField] private bool shouldAutoGenerateItems;
	[Header("If this belongs to a shop, the shop will override this value")]
	[Header("Otherwise it defaults back to this value")]
	public int revenue;
	public GameObject[] spawnedItems = new GameObject[25];
	
	public List<Vector3> points = new();
	public Transform cloneParent;
	public int availableSlotCount;

	[SerializeField] private Transform shopTransform;
	[SerializeField] private string itemName;
	[SerializeField] private float delay, yOffset;
	[SerializeField] private bool isInContactWithPlayer;

	private FormationBase _formation;
	private CollectibleMoney _item;
	private float _timer;

	private FormationBase Formation
	{
		get
		{
			if (_formation == null)
				_formation = GetComponent<FormationBase>();
			return _formation;
		}
	}

	private void Start()
	{
		_item = GameManager.instance.collectiblePrefab;
		
		foreach (var pos in Formation.EvaluatePoints()) points.Add(pos);

		delay = 1f;
		availableSlotCount = points.Count;
	}

	private void Update()
	{
		if(!shouldAutoGenerateItems) return;
		
		_timer += Time.deltaTime;
		if (!(_timer > delay) || availableSlotCount <= 0 || isInContactWithPlayer) return;
		
		CreateItem();
		_timer = 0;
	}
	
	public void CreateItem()
	{
		var slot = FindFirstEmptySlot(); 
		if(slot == -1) return;
		
		var cloneItem = Instantiate(_item, cloneParent, true);
		cloneItem.transform.position = shopTransform.position;
		cloneItem.transform.DOScale(0f, 0.25f).From()
			.SetEase(Ease.OutBack);
		cloneItem.name = itemName;

		var obj = cloneItem.gameObject;
		
		AddItemToArray(ref obj, slot);
		cloneItem.area = this;
		foreach (var _ in points)
			cloneItem.transform.position = transform.position + points[slot] + Vector3.up * yOffset;
	}

	private void AddItemToArray(ref GameObject cloneItem, int slot)
	{
		spawnedItems[slot] = cloneItem;
		availableSlotCount--;
	}

	public void RemoveItemFromArray(GameObject item)
	{
		for (var i = spawnedItems.Length - 1; i >= 0; i--)
		{
			if (spawnedItems[i] != item) continue;
			
			spawnedItems[i] = null;
			availableSlotCount++;
			return;
		}
	}

	private int FindFirstEmptySlot()
	{
		for (var i = 0; i < spawnedItems.Length; i++)
		{
			if (!spawnedItems[i]) return i;
		}

		return -1;
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Assistant"))
		{
			GameManager.instance.playerRobotWaitingTime += Time.deltaTime;
			if (GameManager.instance.playerRobotWaitingTime > 1.5f)
			{
				var rm = other.GetComponent<RobotMovement>();
				rm.myState = RobotMovement.State.selling;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Assistant"))
		{
			isInContactWithPlayer = false;
			GameManager.instance.playerRobotWaitingTime = 0;
		}
	}
}