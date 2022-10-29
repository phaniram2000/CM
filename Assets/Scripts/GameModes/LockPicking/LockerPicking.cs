using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LockerPicking : MonoBehaviour
{
	[SerializeField] private float rotationSpeed = 20f;

	[SerializeField] private GameObject detectionRayObj;
	[SerializeField] private GameObject firstLayer;
	[SerializeField] private GameObject secondLayer;
	[SerializeField] private GameObject thirdLayer;

	[SerializeField] private List<GameObject> layersList;

	[SerializeField] private List<GameObject> firstLayerObjects;
	[SerializeField] private List<GameObject> secondLayerObjects;
	[SerializeField] private List<GameObject> thirdLayerObjects;

	[SerializeField] private List<GameObject> correctCodeObjects;

	[SerializeField] private int[] correctCode;
	[SerializeField] private int correctKeys = 3;
	[SerializeField] private int correctKeysEntered = 0;
	
	private void Start()
	{
		// firstLayer.transform.parent = null;

		for (var i = 1; i < layersList.Count; i++)
		{
			layersList[i].transform.parent = null;
			layersList[i].transform.GetChild(0).gameObject.SetActive(false);
		}

		correctCodeObjects = new List<GameObject>();
		
		correctCodeObjects.Add(firstLayerObjects[correctCode[0]]);
		correctCodeObjects.Add(secondLayerObjects[correctCode[1]]);
		correctCodeObjects.Add(thirdLayerObjects[correctCode[2]]);

		foreach (var t in correctCodeObjects)
		{
			t.tag = "RightAnswer";
		}
	}
	
	public void Rotate(float x)
	{
		transform.Rotate(Vector3.forward,x * rotationSpeed * Time.deltaTime);
	}
	
	public void CheckForKeyCode()
	{
		var ray = new Ray(detectionRayObj.transform.position, Vector3.left );
		if (Physics.Raycast(ray, out var hit, 0.5f))
		{
			print(hit.collider.gameObject);
			if (hit.collider.CompareTag("RightAnswer"))
			{
				if (correctCodeObjects.Contains(hit.collider.gameObject))
				{
					layersList[correctKeysEntered].transform.parent = null;
					layersList[correctKeysEntered].transform.GetChild(0).gameObject.SetActive(false);
					layersList[correctKeysEntered].GetComponent<SpriteRenderer>().color = Color.green;
					correctKeysEntered += 1;
					if (correctKeysEntered != correctKeys)
					{
						layersList[correctKeysEntered].transform.parent = transform;
						layersList[correctKeysEntered].transform.GetChild(0).gameObject.SetActive(true);
					}
					if (AudioManager.instance)
						AudioManager.instance.Play("RightAnswer");
					correctCodeObjects.Remove(hit.collider.gameObject);

					if (correctKeys == correctKeysEntered)
					{
						layersList[0].transform.parent = transform;
						layersList[1].transform.parent = transform;
						layersList[2].transform.parent = transform;
						BlackmailingEvents.InvokeFinalWin();
						DOVirtual.DelayedCall(2f, ()=>GameCanvas.game.MakeGameResult(0, 0));
					}
				}
			}
		}
		Debug.DrawRay(ray.origin, ray.direction  , Color.black, 3f);
	}
	
}
