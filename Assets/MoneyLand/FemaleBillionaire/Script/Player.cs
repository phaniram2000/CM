using UnityEngine;

public class Player : MonoBehaviour/*, IPointerUpHandler*/
{
    public static Player instance { get; private set; }
    public enum animationState { run, idle };
    public animationState playerAnimState;
    public Joystick joystick;
    [HideInInspector]
    public Animator animator;
    private bool canWalk;
    private bool idle,walking;
    private float x, y;
    private Vector2 dir2;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        x = joystick.Horizontal;
        y = joystick.Vertical;
    }

	private void Update()
    {
        JoystickControls();
    }
    public void WalkAnimation(bool condition)
    {
        animator.SetBool("Walk", condition);
        canWalk = true;
    }
    public void JoystickControls()
    {
        x = joystick.Horizontal;
        y = joystick.Vertical;
        Vector3 newPos = new Vector3(x, 0, y);
        dir2 = joystick.savedDir;
        //newPos.Normalize();
        transform.position += new Vector3(newPos.x, 0, newPos.z) * GameManager.instance.playerSpeed * Time.deltaTime;
        
        if (newPos == Vector3.zero)
        {
            idle = true;
            if (idle && playerAnimState == animationState.run)
            {
               // Debug.Log("idle");
                WalkAnimation(false);
                playerAnimState = animationState.idle;
                idle = false;
            }
        }
        else
        {
            if (canWalk && playerAnimState == animationState.idle)
            {
               // Debug.Log("run");
                WalkAnimation(true);
                playerAnimState = animationState.run;
                
                canWalk = false;
            }
            float rotY = Mathf.Atan2(dir2.x, dir2.y) * 180 / Mathf.PI;
            transform.eulerAngles = new Vector3(0, rotY, 0);
        }
    }
}
//Mouse Controls
//else
//{
//    if (Input.GetMouseButton(0))
//    {
//        if (!canWalk)
//        {
//            WalkAnimation(true);
//            canWalk = true;
//        }
//        float xAxis = Input.GetAxis("Mouse X") * Mathf.Deg2Rad;
//        float zAxis = Input.GetAxis("Mouse Y") * Mathf.Deg2Rad;
//        Vector3 pos = new Vector3(xAxis, 0, zAxis);
//        pos.Normalize();
//        transform.position += new Vector3(pos.x, transform.position.y, pos.z) * playerSpeed * Time.deltaTime;
//        if (pos != Vector3.zero)
//        {
//            Quaternion toRotation = Quaternion.LookRotation(pos, Vector3.up);
//            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
//        }
//    }
//    if (Input.GetMouseButtonUp(0))
//    {
//        WalkAnimation(false);
//        canWalk = false;
//    }
//}
//Vector3 moveVector = (Vector3.up * joystick.Vertical - Vector3.left * joystick.Horizontal);
//if (joystick.Horizontal != 0 || joystick.Vertical != 0)
//{
//    transform.rotation = Quaternion.LookRotation(transform.up, moveVector);
//}