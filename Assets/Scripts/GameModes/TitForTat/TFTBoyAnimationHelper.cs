using UnityEngine;

public class TFTBoyAnimationHelper : MonoBehaviour
{
    private TFTBoyController boyController;

    private void Start()
    {
        boyController = GetComponent<TFTBoyController>();
    }


    public void OnLaughAnimationDone()
    {
        if(!boyController) return;
        
        boyController.SetTriggerWalk();   
    }
}
