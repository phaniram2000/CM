using UnityEngine;

public class ATMGirlAnimationHelper : MonoBehaviour
{
    public void OnAnimationSnatchDone()
    {
        ATMEvents.InvokeOnDisableBoysPocketAtmCard();
    }
}
