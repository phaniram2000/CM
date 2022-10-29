using System.Xml.Xsl;
using UnityEngine;

public class PlatformController : MonoBehaviour
{

	[SerializeField] private float zScale,xScale;
	
	
	private Vector3 _defaultScalevalue;


	private void OnEnable()
	{
		MemoryBetGameEvents.IncreasePlatformSize += OnIncreasePlatformSize;
	}

	private void OnDisable()
	{
		MemoryBetGameEvents.IncreasePlatformSize -= OnIncreasePlatformSize;
	}

	
	private void Start()
	{
		_defaultScalevalue = transform.localScale;
	}
	
	private void OnIncreasePlatformSize(int obj)
	{
		if (obj < 12)
		{
			transform.localScale = _defaultScalevalue;
			return;
		}

		Vector3 temp = transform.localScale;
		temp.z = zScale;

		if (obj == 16)
		{
			temp.x = xScale;
		}

		transform.localScale = temp;

	}
}