using UnityEngine;

public class ATMGameController : MonoBehaviour
{
    [SerializeField] private float userPinSeeTime,playerStealAccuracyValue;


    public float UserPinSeeTime => userPinSeeTime;


    public void CheckIfStealIsSuccessfull(float damage)
    {
        
        print("Damage: " + damage);
        if (damage < playerStealAccuracyValue)
        {
            ATMEvents.InvokeOnStealFail();
            return;
        }

        
        ATMEvents.InvokeOnStealSuccess();
        
        
    }
}
