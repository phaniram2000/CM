using DG.Tweening;
using UnityEngine;

public class TBRBoyDialogesController : DialogueBaseController
{

    [SerializeField] private GameObject helpDialogBox,closeDoorDialogBox;
    
    private const int TakItMsg = 0;
    private const int HelpMsg = 1;
    private const int FinalHelpMsg = 2;
    private const int NowHelpMsg = 3;
    private const int CloseDoorMsg = 4;


    private void OnEnable()
    {
        TBREvents.GirlDoorLockingDone += OnGirlDoorLockingDone;
        TBREvents.ItemsButtonPressed += OnItemsButtonPressed;
        TBREvents.ItemPickedUpByGirl += OnItemsPickedByGirl;
        TBREvents.GirlStaringToOpenDoor += OnGirlStaringToOpenDoor;
        TBREvents.GirlCloseDoorDone += OnGirlCloseDoorDone;
        TBREvents.GirlPrankingDoneNowEscape += OnGirlPrankingDone;
    }

    private void OnDisable()
    {
        TBREvents.GirlDoorLockingDone -= OnGirlDoorLockingDone;
        TBREvents.ItemsButtonPressed -= OnItemsButtonPressed;
        TBREvents.ItemPickedUpByGirl -= OnItemsPickedByGirl;
        TBREvents.GirlStaringToOpenDoor -= OnGirlStaringToOpenDoor;
        TBREvents.GirlCloseDoorDone -= OnGirlCloseDoorDone;
        TBREvents.GirlPrankingDoneNowEscape -= OnGirlPrankingDone;
    }

    protected override void Initialise()
    {
        helpDialogBox.SetActive(false);
        closeDoorDialogBox.SetActive(false);
    }

    private void OnGirlDoorLockingDone()
    {
        print("invoked door locking done");
        
        dialogText.text = GetMessage(HelpMsg);
        
        EnableDialogBox();
        

    }


    private void OnItemsButtonPressed(int obj)
    {
        DisableDialogBox();
        dialogText.text = GetMessage(TakItMsg);
        
        EnableDialogBox();
    }

    private void OnItemsPickedByGirl()
    {
       DisableDialogBox();
       dialogText.text = GetMessage(NowHelpMsg);
       
       EnableDialogBox();
       
    }
    
    private void OnGirlStaringToOpenDoor()
    {
       /*DisableDialogBox();
       dialogText.text = GetMessage(CloseDoorMsg);
       
       EnableDialogBox();*/
       
       closeDoorDialogBox.SetActive(true);
    }
    
    private void OnGirlCloseDoorDone()
    {
        DisableDialogBox();
        
        closeDoorDialogBox.SetActive(false);
    }
    
    private void OnGirlPrankingDone()
    {
        
        DOVirtual.DelayedCall(4f, () =>
        {
            DisableDialogBox();
            helpDialogBox.SetActive(true);
        });
    }
    
    


}
