using System.Collections.Generic;
using UnityEngine;

public class SimpleCustomerPool : MonoBehaviour
{
	public static SimpleCustomerPool pool;
	
	[SerializeField] private List<GameObject> prefabs;
	private readonly List<Customer> _available = new();
	[SerializeField] private int poolSize;

	private Transform _parent;

	private void Awake()
	{
		if (!pool) pool = this;
		else Destroy(gameObject);

		_parent = new GameObject("PoolParent").transform;
		_parent.parent = transform;
		
		Generate(poolSize);
	}

	private void Generate(int size)
	{
		for (var i = 0; i < size; i++)
		{
			var inst = Instantiate(prefabs[Random.Range(0, prefabs.Count)], _parent, true);
			
			inst.SetActive(false);
			_available.Add(inst.GetComponent<Customer>());
		}
	}

	public Customer GetNewCustomer()
	{
		if (_available.Count == 0) return null;

		var item = _available[0];
		item.gameObject.SetActive(true);

		_available.Remove(item);
		
		return item;
	}

	public void ReturnUsedCustomer(Customer customer)
	{
		customer.gameObject.SetActive(false);
		customer.transform.parent = _parent;

		_available.Add(customer);
	}
}