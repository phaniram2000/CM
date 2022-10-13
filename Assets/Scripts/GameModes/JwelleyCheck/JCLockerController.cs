using DG.Tweening;
using UnityEngine;

public class JCLockerController : MonoBehaviour
{
    [SerializeField] private Transform lockerDoor,lockerHolder;
    [SerializeField] private float lockerDoorDuration;


    private void OnEnable()
    {
        JCEvents.CloseLocker += CloseLockerDoor;
        
    }

    private void OnDisable()
    {
        JCEvents.CloseLocker -= CloseLockerDoor;
        
    }

    
    private void CloseLockerDoor()
    {
        lockerDoor.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), lockerDoorDuration).SetEase(Ease.Linear).OnComplete(
            () =>
            {
                if(AudioManager.instance)
                    AudioManager.instance.Play("LockerDoorClose");
                
                JCEvents.InvokeOnMakeLockerTransformToGirlHand();
                
                MakeLockerTransformToGirlHand();
                
            });
    }
    
    private void MakeLockerTransformToGirlHand()
    {
        DOVirtual.DelayedCall(0.4f, () =>
        {

            transform.DOJump(lockerHolder.position, 0.5f, 1,0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                if(AudioManager.instance)
                    AudioManager.instance.Play("DiamondSlide");
                
                print("transform locker parent");
                transform.parent = lockerHolder;

                DOVirtual.DelayedCall(0.2f, () => JCEvents.InvokeOnMakeGirlRunWithLocker());
            });
            
          
        });
    }
    
    
}
