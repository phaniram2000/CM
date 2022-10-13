using UnityEngine;

public class TFTEmojiLookAtCamera : MonoBehaviour
{

    void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        
        
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }
}
