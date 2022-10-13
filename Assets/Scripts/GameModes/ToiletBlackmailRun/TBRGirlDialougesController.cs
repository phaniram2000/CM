
using DG.Tweening;
using UnityEngine;


public class TBRGirlDialougesController : DialogueBaseController
{
    private const int GiveStuffMsg = 0;
    private const int NotEnough = 1;
    
    private void OnEnable()
    {
        TBREvents.GirlDoorLockingDone += OnGirlDoorLockingDone;
        TBREvents.ItemPickedUpByGirl += OnItemsPickedUpByGirl;
        TBREvents.ItemReadyToPick += OnItemReadyForPickUp;

    }

    private void OnDisable()
    {
        TBREvents.GirlDoorLockingDone -= OnGirlDoorLockingDone;
        TBREvents.ItemPickedUpByGirl -= OnItemsPickedUpByGirl;
        TBREvents.ItemReadyToPick -= OnItemReadyForPickUp;
        
    }

    
    private void OnGirlDoorLockingDone()
    {
        /*dialogText.text = GetMessage(GiveStuffMsg);
        DOVirtual.DelayedCall(0.6f, () => EnableDialogBox());*/
    }
    
    private void OnItemsPickedUpByGirl()
    {
        DOVirtual.DelayedCall(0.4f, () => DisplayNotEnoughDialog());
    }

    private void DisplayNotEnoughDialog()
    {
        if (TBRGameController.Get.GameItemsPickedCount >= TBRGameController.Get.ItemsToPick) return;
        
        dialogText.text = GetMessage(NotEnough);
        DOVirtual.DelayedCall(0.6f, () => EnableDialogBox());
    }
    
    private void OnItemReadyForPickUp(GameObject obj)
    {
        DisableDialogBox();
    }
}
