using System;
using UnityEngine;

public class EraseTarget : MonoBehaviour
{
	[SerializeField] private float cleaningRadius = 0.05f;
	[SerializeField] private float totalCleaned = 0f;
	public float minClean;
	public MeshFilter meshSource;

	public bool isFullyErased;

	private Vector3[] _vertices;
	private Color[] _colors;
	private Mesh _mesh;


	public static event Action Ondoneerasing;
	private static void InvokeOnDoneErasing() => Ondoneerasing?.Invoke();
	private void Start()
    {
        _mesh = meshSource.mesh;
        _vertices = _mesh.vertices;
        _colors = new Color[_vertices.Length];

        for (var i = 0; i < _colors.Length; i++) _colors[i] = Color.red;
		
		UpdateMesh();
	}

	public void EraseAt(Vector3 position)
    {
		var cleanVertices = 0;
		
		for (var i = 0; i < _vertices.Length; i++)
        {
            var pos = meshSource.transform.TransformPoint(_vertices[i]);
            var distance = (pos - position).magnitude;

            if (distance < cleaningRadius)
            {
				var t = distance / cleaningRadius;
                _colors[i] = Color.Lerp(_colors[i], Color.green, 2f - t);
			}
            if (_colors[i].g > 0.8f)
                cleanVertices++;
        }
		totalCleaned = (float)cleanVertices / _vertices.Length;

		if (totalCleaned >= minClean) AutoClear();
		
		UpdateMesh();
    }

	private void UpdateMesh() => _mesh.colors = _colors;

	private void AutoClear()
    {
        for (var i = 0; i < _vertices.Length; i++) _colors[i] = Color.Lerp(_colors[i], Color.green, 2);
		UpdateMesh();
		
		if (isFullyErased) return;
		transform.parent = null;
		isFullyErased = true;
		InvokeOnDoneErasing();
    }
}
