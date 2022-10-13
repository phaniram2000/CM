using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class JCJwelleryItemController : MonoBehaviour
{
    
    [SerializeField] private List<GameObject> jwelleryList;
    [SerializeField] private float jwelleryMoveDuration;

    [SerializeField] private List<Transform> lockerEndPointTransformsList;

    private const string ItemCheckEndPointName = "JwelleryCheckEndPoint";

    private const string ItemStartPointName = "JwelleryStartPoint";

    private const string ItemCrushPointName = "JwelleryCrushEndPoint";


    private Transform itemCheckEndPointTransform,itemStartPointTransform,itemCrushPointTransform;

    private JCJwelleryItemProperty currentJcJwelleryItemProperty;
    
    private int count = 0;
    private GameObject currentJwelleryItem;


    private void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
        JCEvents.FakeSelected += MoveToCrushPlace;
        JCEvents.SelectNextJwelleryItem += SelectJwelleryItem;
        JCEvents.JwelleryCrushingDone += OnJwelleryCrushingDone;
        JCEvents.RealSelected += MoveToLockerPlace;
        JCEvents.MakeLockerTransformToGirlHand += OnMakeLockerTransformToGirlHand;

    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;
        JCEvents.FakeSelected -= MoveToCrushPlace;
        JCEvents.SelectNextJwelleryItem -= SelectJwelleryItem;
        JCEvents.JwelleryCrushingDone -= OnJwelleryCrushingDone;
        JCEvents.RealSelected -= MoveToLockerPlace;
        JCEvents.MakeLockerTransformToGirlHand -= OnMakeLockerTransformToGirlHand;
    }

    private void Start()
    {
        GetItemStartPointTransform();
        GetItemCheckEndPointTransform();
        GetItemCrushPointTransform();

        MoveToStartPoint();
        
    }

    private void GetItemCheckEndPointTransform()
    {
        var tempGameobject = GameObject.Find(ItemCheckEndPointName);

        if (!tempGameobject) return;

        itemCheckEndPointTransform = tempGameobject.transform;
    }

    private void GetItemStartPointTransform()
    {
        var tempGameobject = GameObject.Find(ItemStartPointName);

        if (!tempGameobject) return;

        itemStartPointTransform = tempGameobject.transform;
    }
    
    private void GetItemCrushPointTransform()
    {
        var tempGameobject = GameObject.Find(ItemCrushPointName);

        if (!tempGameobject) return;

        itemCrushPointTransform = tempGameobject.transform;
    }

    private void MoveToStartPoint()
    {
        if (!itemStartPointTransform) return;

        for (int i = 0; i <jwelleryList.Count; i++)
        {
            jwelleryList[i].transform.position = itemStartPointTransform.position;
        }
        
    }


    private void SelectJwelleryItem()
    {
        if (count >= jwelleryList.Count)
        {
            //game done
            DOVirtual.DelayedCall(0.3f,()=>JCEvents.InvokeOnCloseLocker());
            return;
        }

        currentJwelleryItem = jwelleryList[count];

        if (!currentJwelleryItem) return;
        
        GetJwelleryItemProperty(currentJwelleryItem);

        count++;
        
        JCEvents.InvokeOnJwelleryItemSelected(currentJwelleryItem);

        DOVirtual.DelayedCall(0.2f, () => MoveToJwelleryToCheckPlace());

    }

    private void GetJwelleryItemProperty(GameObject item)
    {
        if (!item.TryGetComponent(out JCJwelleryItemProperty itemProperty)) return;

        currentJcJwelleryItemProperty = itemProperty;
    }

    private void MoveToJwelleryToCheckPlace()
    {
        
        print("moved jwellery to place");
        
        if (!currentJwelleryItem) return;

        if (!itemCheckEndPointTransform) return;
        
        currentJwelleryItem.transform.DOMove(itemCheckEndPointTransform.position, jwelleryMoveDuration)
            .SetEase(Ease.Linear).OnComplete(() =>
            {
                //allow player to use meter.
                if(AudioManager.instance)
                    AudioManager.instance.Play("DiamondSlide");
                JCEvents.InvokeOnAllowToCheckJwellery();
            });
    }

    private void MoveToCrushPlace()
    {
        if (!currentJwelleryItem) return;

        if (!itemCrushPointTransform) return;
        
        currentJwelleryItem.transform.DOMove(itemCrushPointTransform.position, jwelleryMoveDuration)
            .SetEase(Ease.Linear).OnComplete(() =>
            {
                //invoke crush functionalty.
                
                if(AudioManager.instance)
                    AudioManager.instance.Play("DiamondSlide");
                JCEvents.InvokeOnJwelleryReadyToCrush(currentJwelleryItem);
            });
    }
    
    
    private void MoveToLockerPlace()
    {
        if (!currentJwelleryItem) return;

        if (lockerEndPointTransformsList.Count <= 0) return;

        var index = Random.Range(0, lockerEndPointTransformsList.Count);
        Transform lockerEndPointTransform = lockerEndPointTransformsList[index];
        
        
        currentJwelleryItem.transform.DOMove(lockerEndPointTransform.position, jwelleryMoveDuration)
            .SetEase(Ease.Linear).OnComplete(() =>
            {
                if(AudioManager.instance)
                    AudioManager.instance.Play("DiamondSlide");
                OnJwelleryGoneInLocker();
               
            });
    }
    
    
    

    
    private void OnTapToPlay()
    {
       SelectJwelleryItem();
    }

    
    private void OnJwelleryCrushingDone()
    {

        if (!currentJcJwelleryItemProperty) return;
        
        currentJwelleryItem.SetActive(false);
        JCEvents.InvokeOnCheckJwelleryPlace(currentJcJwelleryItemProperty.IsReal,false);
    }

    private void OnJwelleryGoneInLocker()
    {
        if (!currentJcJwelleryItemProperty) return;
        
        
        JCEvents.InvokeOnCheckJwelleryPlace(currentJcJwelleryItemProperty.IsReal,true);
    }

    
    private void OnMakeLockerTransformToGirlHand()
    {
        DisableAllJwelleryItems();
    }


    private void DisableAllJwelleryItems()
    {
        for (int i = 0; i < jwelleryList.Count; i++)
        {
            jwelleryList[i].SetActive(false);
        }
    }


}
