using UnityEngine;

public class DCGirlCollisionDetection : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.transform.CompareTag("DCObstacle")) return;
        
        //Invoke to make girl fall.
        DCEvents.InvokeOnGirlCollidedWithObstacle();
        
    }
}
