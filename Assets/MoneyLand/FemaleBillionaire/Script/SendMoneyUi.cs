using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SendMoneyUi : MonoBehaviour
{
	public static SendMoneyUi Instance;
	public Transform uiMoneyIcon;
	public List<GameObject> moneyCollected = new();
	public Vector3 quaternionAngles;
	private Vector3 _screenPoint;
	private Vector3 _worldPos;
	private Vector3 _playerPos;
	public bool isInvesting;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		quaternionAngles = new Vector3(0, 0, 30);
	}

	private void Update()
	{
		if (isInvesting) return;
		if (transform.childCount <= 0) return;
		for (int i = 0; i < transform.childCount; i++)
		{
			if (!moneyCollected.Contains(transform.GetChild(i).gameObject))
			{
				moneyCollected.Add(transform.GetChild(i).gameObject);
			}
		}

		MoveToui();
	}

	public void MoveToui()
	{
		_screenPoint = uiMoneyIcon.position + new Vector3(0, 0, 5);
		_worldPos = Camera.main.ScreenToWorldPoint(_screenPoint);
		for (int i = 0; i < moneyCollected.Count; i++)
		{
			moneyCollected[i].transform.position = Vector3.MoveTowards(moneyCollected[i].transform.position, _worldPos, 3f * Time.deltaTime);
			moneyCollected[i].transform.rotation = Quaternion.Euler(quaternionAngles);
			moneyCollected[i].transform.DOScale(0f, 2).OnComplete(() => DOVirtual.DelayedCall(1f, () =>
			{
				if (moneyCollected.Count <= 0) return;

				foreach (var t in moneyCollected)
				{
					var x = t.transform.position == _worldPos;
					print(x);
					if (t.transform.position == _worldPos) 
						t.SetActive(false);
				}
			}));
		}
	}


	public IEnumerator RemoveLast()
	{
		yield return new WaitForSeconds(0.5f);
		if (moneyCollected.Count > 0)
		{
			if (moneyCollected[moneyCollected.Count - 1].transform.position == _playerPos)
				moneyCollected.Remove(moneyCollected[moneyCollected.Count - 1]);
		}
	}

	private IEnumerator TurnOffMoney()
	{
		yield return new WaitForSeconds(1);
		if (moneyCollected.Count <= 0) yield break;
		
		for (int i = 0; i < moneyCollected.Count; i++)
		{
			if (moneyCollected[i].transform.position == _worldPos) 
				moneyCollected[i].SetActive(false);
		}
	}
}