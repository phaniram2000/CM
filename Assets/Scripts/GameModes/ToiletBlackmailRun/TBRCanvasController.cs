using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TBRCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject tapToLockInstruction,buttonPanels;
    [SerializeField] private List<GameObject> buttonslist;

    private const int Mobile = 0;
    private const int CarKeys = 1;
    private const int Money = 2;
    private const int Watch = 3;
    
    
    private void OnEnable()
    {
        TBREvents.GirlCloseDoorDone += EnableTapInstruction;
        TBREvents.OnTapToLock += DisableTapInstruction;
        TBREvents.GirlDoorLockingDone += OnGirlDoorLockingDone;
        TBREvents.ItemsButtonPressed += OnItemsButtonPressed;
        TBREvents.ItemPickedUpByGirl += OnItemsPickedUpByGirl;
    }

    private void OnDisable()
    {
        TBREvents.GirlCloseDoorDone -= EnableTapInstruction;
        TBREvents.OnTapToLock -= DisableTapInstruction;
        TBREvents.GirlDoorLockingDone -= OnGirlDoorLockingDone;
        TBREvents.ItemsButtonPressed -= OnItemsButtonPressed;
        TBREvents.ItemPickedUpByGirl -= OnItemsPickedUpByGirl;
    }

    private void Start()
    {
        DisableTapInstruction();
        DisableButtonPanels();
        
        EnableButtons(0,1);
    }

    private void EnableButtons(int a,int b)
    {
        for (int i = 0; i < buttonslist.Count; i++)
        {
            if (i == a || i == b)
            {
                buttonslist[i].SetActive(true);
            }
            else
            {
                buttonslist[i].SetActive(false);
            }
        }
    }


    private void EnableTapInstruction()
    {
        tapToLockInstruction.SetActive(true);
    }

    private void DisableTapInstruction()
    {
        tapToLockInstruction.SetActive(false);
    }

    private void EnableButtonPanels()
    {
        buttonPanels.transform.localScale = Vector3.zero;
        buttonPanels.SetActive(true);
        buttonPanels.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InBack);
    }

    private void DisableButtonPanels()
    {
        buttonPanels.SetActive(false);
    }
    
    private void OnGirlDoorLockingDone()
    {
        DOVirtual.DelayedCall(1.3f, () => EnableButtonPanels());
    }
    
    private void OnItemsButtonPressed(int obj)
    {
         DOVirtual.DelayedCall(0.2f,()=>DisableButtonPanels());
         
         if(AudioManager.instance)
             AudioManager.instance.Play("Button");
         
         Vibration.Vibrate(30);
    }
    
    private void OnItemsPickedUpByGirl()
    {
        DOVirtual.DelayedCall(0.4f, () =>
        {
            
            if (TBRGameController.Get.GameItemsPickedCount >= TBRGameController.Get.ItemsToPick) return;
            
            EnableButtons(2,3);
            EnableButtonPanels();
        });
    }




}
