using UnityEngine;

public class CanvasLookAtCam : MonoBehaviour
{
    private Canvas _canvas;
	private Transform _mainCam;

    private void Start()
    {
        _canvas = GetComponent<Canvas>();
		if(_canvas)
			_canvas.worldCamera = Camera.main;
		
		_mainCam = Camera.main.transform;
	}

    private void Update()
    {
        var direction = transform.root.position - _mainCam.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
