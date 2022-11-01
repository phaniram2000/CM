using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TrafficRoadObstacle : MonoBehaviour
{
    // Start is called before the first frame update
	[SerializeField] private Transform startTransform;
	[SerializeField] private Transform endTransform;
	
	[SerializeField] private List<GameObject> carGroups;

	[SerializeField] private float spawnDelay = 0.5f;
	[SerializeField] private float repeatDelay = 0.5f;
	[SerializeField] private float moveDuration;
	
    private void Start()
	{
		for (var index = 0; index < carGroups.Count; index++)
		{
			MoveTheCars(carGroups[index], index);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftAlt)) Time.timeScale = 6f;
		else if(Input.GetKeyUp(KeyCode.LeftAlt)) Time.timeScale = 1f;
	}

	private void MoveTheCars(GameObject carGroup, int index)
	{
		carGroup.transform.DOMove(endTransform.position, moveDuration)
			.SetEase(Ease.Linear)
			.SetDelay(spawnDelay * index)
			.SetLoops(-1)
			.OnComplete(() => carGroup.transform.position = startTransform.position);
	}
}
