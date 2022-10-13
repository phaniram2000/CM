using UnityEngine;

public class JCJwelleryPlaceCheckController : MonoBehaviour
{
    private void OnEnable()
    {
        JCEvents.CheckJwelleryPlace += CheckIfJwelleryWentToRightPlace;
    }

    private void OnDisable()
    {
        JCEvents.CheckJwelleryPlace -= CheckIfJwelleryWentToRightPlace;
    }


    private void CheckIfJwelleryWentToRightPlace(bool jewlleryRealAuthanticity,bool jwelleryAunthancityChossed)
    {
        if (jewlleryRealAuthanticity == jwelleryAunthancityChossed)
        {
            JCEvents.InvokeOnSelectNextJwelleryItem();
            return; 
        }
        
        
        //invoke game lose
        JCEvents.InvokeOnMakeGirlSad();
    }
}
