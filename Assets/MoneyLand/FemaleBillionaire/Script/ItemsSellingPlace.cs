using System.Collections.Generic;
using UnityEngine;

public class ItemsSellingPlace : MonoBehaviour
{
    public bool isPlayerHere;
    public Transform deliveryParent,sellingParent;
    public List<Transform> deliveryPointList = new List<Transform>();
    public bool canMoveAi;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerHere = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerHere = false;
        }
    }
}
