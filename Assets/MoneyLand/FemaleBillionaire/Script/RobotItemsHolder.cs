using System.Collections.Generic;
using UnityEngine;

public class RobotItemsHolder : MonoBehaviour
{

    public List<GameObject> childObjs = new List<GameObject>();
    [HideInInspector]
    private float num;
    [Header("Increase to Reduce Space Between Objects")]
    [SerializeField]
    private float spaceBtwObjects = 5;
    public bool isSelling;
    public PlayerRobot _Ai;
    private void Update()
    {
        //MetaUiManager.instance.playerCapacityText.text = "Capacity : " + transform.childCount + "/" + GameManager.instance.playerMaxCapacity;
        if (!isSelling)
        {
            if (_Ai.aiHoldingItems.Count > 0)
            {
                for (int i = 0; i < _Ai.aiHoldingItems.Count; i++)
                {
                    if (!childObjs.Contains(_Ai.aiHoldingItems[i]))
                    {
                        childObjs.Add(_Ai.aiHoldingItems[i]);
                    }
                }
            }
            ArrangeinStack();
        }
    }
    public void ArrangeinStack()
    {
        for (int i = 0; i < _Ai.aiHoldingItems.Count; i++)
        {
            num = transform.GetSiblingIndex() / 10f;
            childObjs[i].transform.position = Vector3.Lerp(childObjs[i].transform.position, transform.position + new Vector3(0, (i + num) / spaceBtwObjects, 0), 2);
            childObjs[i].transform.rotation = transform.rotation;
        }
    }
}
