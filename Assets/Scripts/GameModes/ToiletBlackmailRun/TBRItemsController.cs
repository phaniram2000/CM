using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TBRItemsController : MonoBehaviour
{
    
    [System.Serializable]
    public struct Items
    {
        public int id;
        public GameObject ItemGameObject;
    }


    [SerializeField] private List<Items> itemsList;


    private Transform itemsEndPointTransform;
    
    
    private const string ItemsEndPointName = "ItemsEndPoint";
    
    private void OnEnable()
    {
        TBREvents.ItemsButtonPressed += OnItemsButtonPressed;
    }

    private void OnDisable()
    {
        TBREvents.ItemsButtonPressed -= OnItemsButtonPressed;
    }


    private void Start()
    {
        itemsEndPointTransform = GameObject.Find(ItemsEndPointName).transform;
        
        DisableAllItems();
    }


    private void DisableAllItems()
    {
        for (int i = 0; i < itemsList.Count; i++)
        {
            itemsList[i].ItemGameObject.SetActive(false);
        }
    }

    private void OnItemsButtonPressed(int id)
    {
        for (int i = 0; i < itemsList.Count; i++)
        {
            if (id == itemsList[i].id)
            {
                MakeItemAppear(itemsList[i].ItemGameObject);
            }
        }
    }

    private void MakeItemAppear(GameObject obj)
    {
        if (!itemsEndPointTransform) return;
        
        obj.SetActive(true);

        obj.transform.DOMove(itemsEndPointTransform.position, 0.7f).SetEase(Ease.Linear).OnComplete(()=>
        {
            TBREvents.InvokeOnItemReadyToPick(obj);
            
            if (AudioManager.instance)
                 AudioManager.instance.Play("Itemspass");
        });

    }
}
