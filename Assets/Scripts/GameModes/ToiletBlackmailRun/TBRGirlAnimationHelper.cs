using DG.Tweening;
using UnityEngine;

public class TBRGirlAnimationHelper : MonoBehaviour
{

    private TBRGirlController controller;

    private void Start()
    {
        controller = GetComponent<TBRGirlController>();
    }

    public void OnStartDoorOpenAnimation()
    {
        TBREvents.InvokeOnGirlStaringToOpenDoor();

        if (AudioManager.instance)
            DOVirtual.DelayedCall(0.5f, () => AudioManager.instance.Play("DoorOpen"));
    }

    public void OnDoorCloseAnimationDone()
    {
        TBREvents.InvokeOnGirlCloseDoorDone();
        
        if (AudioManager.instance)
            DOVirtual.DelayedCall(0.5f, () => AudioManager.instance.Play("DoorClose"));
    }

    public void OnStartLockingDoor()
    {
        TBREvents.InvokeOnGirlStartingToLockDoor();
        
        if (AudioManager.instance)
            DOVirtual.DelayedCall(0.6f, () => AudioManager.instance.Play("LockDoor"));
    }

    public void OnGirlLockDoorAnimationDone()
    {
        if(!controller) return;
        
        controller.MakeGirlWalkBackWard();
    }

    public void OnItemPickUpStartAnimation()
    {
        if (!controller) return;
        
        controller.OnItemPickedUpStart();
    }

    public void OnItemPickUpEndAnimation()
    {
        if (!controller) return;
        
        controller.OnItemPickedUpEnd();
        
    }
}
