using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MouseRotation : MonoBehaviour {

	public float rotSpeed;
	public bool collided;
	float initialY;
	
	public Vector2 xClamp = new Vector2(-25f, 25f);
	public Vector2 zClamp = new Vector2(-25f,25f);


	void Start()
	{
		initialY = transform.eulerAngles.y;

	}
	void LateUpdate()
	{
		#if UNITY_EDITOR

		if(Input.GetMouseButton(0) /* && !buttonCanvas.mobile */)
		{
			float rotX = Input.GetAxis("Mouse X")* rotSpeed* Mathf.Deg2Rad;
			float rotY = Input.GetAxis("Mouse Y")* rotSpeed* Mathf.Deg2Rad;				
			Vector3 pos = new Vector3 (rotX*Time.deltaTime, 0,rotY*Time.deltaTime);
           
        }	

		#elif UNITY_ANDROID		

		if(/* buttonCanvas.mobile &&  */Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Moved && collided)
		{


			
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			float rotX = touchDeltaPosition.x* rotSpeed/18* Mathf.Deg2Rad;
			float rotY = touchDeltaPosition.y* rotSpeed/18* Mathf.Deg2Rad;				
			Vector3 pos = new Vector3 (rotX*Time.deltaTime, 0,rotY*Time.deltaTime);		
			transform.Rotate(pos, Space.World);			
		}	
		#endif

		transform.eulerAngles = new Vector3(ClampAngle(transform.eulerAngles.x, xClamp.x, xClamp.y),
								initialY, ClampAngle(transform.eulerAngles.z, zClamp.x, zClamp.y));
		
		
		
	}

	static float ClampAngle(float angle, float min, float max)
    {
        if (min < 0 && max > 0 && (angle > max || angle < min))
        {
            angle -= 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }
        else if(min > 0 && (angle > max || angle < min))
        {
            angle += 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }
 
        if (angle < min) return min;
        else if (angle > max) return max;
        else return angle;
    }

	
	
	
}
