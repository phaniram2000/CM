using UnityEngine;

public class SwipeForceTest : MonoBehaviour
{
    Vector3 startPos, endPos;
    public float speed;
    float zcoord;

    private void OnMouseDown()
    {  
        startPos = Input.mousePosition;
        startPos.z = startPos.y;
        startPos.y = 0;
    }

    private void OnMouseDrag()
    {
        
    }
    private void OnMouseUp()
    {
        endPos = Input.mousePosition;
        endPos.z = endPos.y;
        endPos.y = 0;
        Vector3 dir = (endPos - startPos).normalized;
        GetComponent<Rigidbody>().AddForce(dir * speed);
    }
    
    void SetTouchPosition(Vector3 pos)
    {
        
    }
}
