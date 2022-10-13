using DG.Tweening;
using UnityEngine;

public class TBRToiletDoorController : MonoBehaviour
{
    [SerializeField] private float doorCloseValue, doorOpenValue,openDuration,closeDuration;

    private Transform _transform;
    private Tween doorShakeTween;
    
    private void OnEnable()
    {
        TBREvents.GirlStaringToOpenDoor += OpenDoor;
        TBREvents.BoyAngryDone += CloseDoor;
        TBREvents.GirlDoorLockingDone += OnGirlLockingDone;
        TBREvents.ItemPickedUpByGirl += OnItemsPickedByGirl;
    }

    private void OnDisable()
    {
        TBREvents.GirlStaringToOpenDoor -= OpenDoor;
        TBREvents.BoyAngryDone -= CloseDoor;
        TBREvents.GirlDoorLockingDone -= OnGirlLockingDone;
        TBREvents.ItemPickedUpByGirl -= OnItemsPickedByGirl;
    }

    private void Start()
    {
        _transform = transform;
    }


    private void OpenDoor()
    {
        Quaternion rot = Quaternion.Euler(0, doorOpenValue, 0);
        _transform.DOLocalRotateQuaternion(rot, openDuration).SetEase(Ease.Linear).OnComplete(()=>TBREvents.InvokeOnGirlOpenDoorDone());
    }

    private void CloseDoor()
    {
        Quaternion rot = Quaternion.Euler(0, doorCloseValue, 0);
        _transform.DOLocalRotateQuaternion(rot, closeDuration).SetEase(Ease.Linear);
    }

    private void ShakeDoor()
    {
        doorShakeTween =  transform.DOScale(transform.localScale * 1.03f, 0.2f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        if(AudioManager.instance)
            AudioManager.instance.Play("DoorKnock");
        
        DOVirtual.DelayedCall(2f, () =>
        {
            doorShakeTween.Kill();
        });
    }
    
    private void OnGirlLockingDone()
    {
        DOVirtual.DelayedCall(0.7f, () => ShakeDoor());
    }
    
    private void OnItemsPickedByGirl()
    {
        DOVirtual.DelayedCall(0.6f, () => ShakeDoor());
    }
}
