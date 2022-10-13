using UnityEngine;

public class TFTSplineTrigerHelper : MonoBehaviour
{

    private TFTGirlController girlController;
    private TFTBoyController boyController;


    public void OnBoyWalkTrigger()
    {
        if (!boyController) return;
            
        boyController.OnTiggerReached();
    }

    public void OnGirlWalkTrigger()
    {
        if (!girlController) return;
        
       
        girlController.OnTiggerReached();
    }

    public void OnGirlLiftOutWalkTrigger()
    {
        if (!girlController) return;
        
        girlController.OnSecondCharacterTriggerReached();
    }
    
    public void OnBoyLiftInWalkTrigger()
    {
        if (!boyController) return;
        
        boyController.OnSecondCharacterTriggerReached();
    }


    public void AssignGrilController(TFTGirlController controller)
    {
        girlController = controller;
    }


    public void AssignBoyController(TFTBoyController controller)
    {
       boyController = controller;

    }

}
