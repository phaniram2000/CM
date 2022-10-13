using UnityEngine;
using UnityEngine.UI;

namespace ShuffleCups
{
	public class SquidCharacterCanvas : MonoBehaviour
	{
		[SerializeField] private bool isNameCanvas;


		[SerializeField] private bool randomiseNumber;
		[SerializeField] private Text id;
		[SerializeField] private GameObject nameTag;
		private Canvas _canvas;

		private void Start()
		{
			_canvas = GetComponent<Canvas>();
			_canvas.worldCamera = Camera.main;
		
			if(isNameCanvas) return;
		
			if(randomiseNumber)
				id.text = Random.Range(1, 512).ToString();
		}
	
		/*
		private void Update()
		{
			var direction = transform.root .position - _canvas.worldCamera.transform.position;
			transform.rotation = Quaternion.LookRotation(direction);
		}*/
	}
}


