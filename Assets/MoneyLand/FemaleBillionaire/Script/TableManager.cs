using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TableManager : MonoBehaviour
{
    public List<GameObject> childObjs = new();
    public List<Transform> refPositionsList = new();
    public Transform refTransform;
    public int index;

	private void Start()
    {
        for (int i = 0; i < refTransform.childCount; i++)
        {
            refPositionsList.Add(refTransform.GetChild(i));
        }
    }

	private void Update() => ArrangeinTable();

	private void ArrangeinTable()
	{
		if (transform.childCount <= 0) return;
		
		for (int i = 0; i < transform.childCount; i++)
		{
			if (!childObjs.Contains(transform.GetChild(i).gameObject))
			{
				childObjs.Add(transform.GetChild(i).gameObject);
				childObjs[i].transform.DOMove(refPositionsList[index].position, 0.1f,false);
				childObjs[i].transform.rotation = refPositionsList[index].transform.rotation;
				index++;
				if (index > refPositionsList.Count) index = 0;
				TapSound();
			}
			if (childObjs.Count >= refPositionsList.Count) index = 0;
		}
	}
	
    public bool once;

	private void TapSound()
    {
        once = true;
        if (MetaAudioManager.instance && once && AiBuying.instance.isPlayerSelling)
        {
            MetaAudioManager.instance.PlayTapSound();
            if (Application.platform == RuntimePlatform.Android)
            {
                Vibration.Vibrate(15);
            }
            once = false;
        }
    }
}