using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CustomerRotation : MonoBehaviour
{
	public CollectableArea area;
	public List<Transform> standingPositions;
	[SerializeField] private Transform customerParent;
	[SerializeField] private float moveDuration, timeBetweenRotation;
	[SerializeField] private ParticleSystem poofFront, poofBack;

	private readonly List<Customer> _customers = new();
	private Tween _rotationTween;

	public void UnlockShop()
	{
		Debug.Log("rotat", this);
		_rotationTween = DOVirtual.DelayedCall(timeBetweenRotation, MoveEveryoneForward)
			.SetDelay(1f)
			.SetLoops(-1);

		for (var i = 0; i < standingPositions.Count - 1; i++) 
			MakeAndAddNewCustomer();
	}
	
	public void MakeAndAddNewCustomer()
	{
		var cust = SimpleCustomerPool.pool.GetNewCustomer();

		_customers.Add(cust);
		
		cust.Init(this, _customers.Count, moveDuration);
		cust.MakeVisible();
		poofBack.Play();

		var custTransform = cust.transform;
		var destTransform = standingPositions[_customers.Count];
		cust.myIdx = _customers.Count;
		
		custTransform.position = destTransform.position;
		custTransform.rotation = destTransform.rotation;
		//custTransform.localScale = destTransform.localScale;
		
		custTransform.parent = customerParent;
	}

	public void RemoveCustomer(Customer customer)
	{
		_customers.Remove(customer);
		poofFront.Play();
		SimpleCustomerPool.pool.ReturnUsedCustomer(customer);
		
		area.CreateItem();
	}

	private void MoveEveryoneForward()
	{
		foreach (var t in _customers) 
			t.MoveForward();
	}
}