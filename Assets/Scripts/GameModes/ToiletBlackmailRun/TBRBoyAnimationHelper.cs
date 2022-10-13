using UnityEngine;

public class TBRBoyAnimationHelper : MonoBehaviour
{
    public void OnYellingAnimationDone()
    {
        TBREvents.InvokeOnBoyAngryDone();
    }
}
