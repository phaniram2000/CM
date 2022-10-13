using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dreamteck.Splines;

public class PLAYER : MonoBehaviour
{
    public static PLAYER instance;
    Vector3 prevMousePos, mousePos, myrotation;
    float offset, speed = 20, time,prevoffset;
    private Animator anim;
    //private UIMANAGER UIM;
    private Rigidbody RB;
    public Transform cam;
    private Quaternion rot;
    public skates[] SK;
    public bool grounded=true;
    public bool jump=false;
    private camfollow MC;
    private ShiftRun_GameManager GM;
    
    public SplineFollower SF;
    bool reached;
    public float speedtimer;
    [Range(0,1)]
    public float Mychar;
    public bool longjump = false;
   public int mouseclickno = 0;

   private void Awake()
   {
       instance = this;
   }
   private void OnEnable()
   {
       
       GameEvents.TapToPlay += Start_Game;
   }
   private void OnDisable()
   {
       GameEvents.TapToPlay -= Start_Game;
   }
   private void Start_Game()
   {
       SF.follow = true;
   }
   void Start()
    {
        RB = GetComponent<Rigidbody>();
        GM = ShiftRun_GameManager.instance;
        SF = transform.parent.GetComponent<SplineFollower>();
        SK = FindObjectsOfType<skates>();
        MC = FindObjectOfType<camfollow>();
        rot = Quaternion.Euler(0,GM.myrot-120f, 0);
        anim = GetComponent<Animator>();
        //UIM = FindObjectOfType<UIMANAGER>();
       
    }
    int i = 0;
    // Update is called once per frame
    Vector3 lastpos;
    float TC = 0, TC2 = 0;
    private void FixedUpdate()
    {
        if (SK[0].springjump && SK[1].springjump)
        {
            print("11");
            RB.velocity = Vector3.up * 14 + Vector3.forward * 11;
            // if (SOUNDMANAGER.instance)
            //     SOUNDMANAGER.instance.AS.PlayOneShot(SOUNDMANAGER.instance.aud1);
            if(AudioManager.instance)
                AudioManager.instance.Play("Button");
            SK[0].springjump = false;
            SK[1].springjump = false;
        }
        if (SK[0].skatejump && SK[1].skatejump)
        {
            RB.velocity = Vector3.up * 8 + Vector3.forward * 10;
            // if (SOUNDMANAGER.instance)
            //     SOUNDMANAGER.instance.AS.PlayOneShot(SOUNDMANAGER.instance.aud1);
            if(AudioManager.instance)
                AudioManager.instance.Play("Button");
            SK[0].skatejump = false;
            SK[1].skatejump = false;
        }
        if(SK[0].obstaclepass && SK[1].obstaclepass)
        {
            // if(SOUNDMANAGER.instance)
            // SOUNDMANAGER.instance.AS.PlayOneShot(SOUNDMANAGER.instance.aud1);
            if(AudioManager.instance)
                AudioManager.instance.Play("Button");
            SK[0].obstaclepass = false;
            SK[1].obstaclepass = false;
        }
        if (SK[0].playerkilled || SK[1].playerkilled)
        {
            SF.enabled = false;
            SK[0].playerkilled=false;
            SK[1].playerkilled = false;
        }
        if (longjump)
        {
            RB.velocity = Vector3.up * 15 + Vector3.forward *120;
            // if (SOUNDMANAGER.instance)
            //     SOUNDMANAGER.instance.AS.PlayOneShot(SOUNDMANAGER.instance.aud1);
            if(AudioManager.instance)
                AudioManager.instance.Play("Button");
            longjump = false;
        }
    }
    float rotX;
    public Text rotxtex;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            prevMousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            prevMousePos = Camera.main.ScreenToViewportPoint(prevMousePos);
            offset = prevoffset;
            //  prevMousePos = prevMousePos - lastpos;
        }
        if (Input.GetMouseButton(0))
        {
            mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            mousePos = Camera.main.ScreenToViewportPoint(mousePos);
             offset = (mousePos.x - prevMousePos.x)*5;
            //if (Time.timeSinceLevelLoad > time + 0.05f)
            //{
            //    time = Time.timeSinceLevelLoad;
            //    prevMousePos = mousePos;// -lastpos;
            //}
            //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            //{
            //    Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            //    rotX = touchDeltaPosition.x * 50 / 18 * Mathf.Deg2Rad;
            //}
        }
        if (Input.GetMouseButtonUp(0))
        {
           // lastpos= Camera.main.ScreenToViewportPoint(mousePos);
            mouseclickno = 0;
            prevoffset = offset;
        }
        this.anim.Play("STRECH", 0, offset);
        if (reached && i == 0)
        {
            cam.rotation = Quaternion.Slerp(cam.rotation, rot, 0.8f * Time.deltaTime);
        }
        //if (Application.platform == RuntimePlatform.Android) {
        //    TouchControls();
        //}
        if (SK[0].springjump && SK[1].springjump)
        {
            anim.SetLayerWeight(2, 0.8f);
            anim.SetTrigger("JUMP");
        }
        if (SK[0].skatejump && SK[1].skatejump)
        {
            anim.SetLayerWeight(2, 0.8f);
            anim.SetTrigger("JUMP");
            print("JUMP");
        }
        //if(jump && SK[0].land && SK[1].land)
        //{
        //    anim.SetTrigger("LANDING");
        //    print("111");
        //    SK[0].land = false;
        //    SK[0].land = false;
        //}

    }
    public void TouchControls()
    {
        if (Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 newMP = Input.GetTouch(0).position;
            TC = newMP.x / Screen.width;
            TC2 = Mathf.Lerp(-0.3f, 1.3f, TC);
            this.anim.Play("STRECH", 0, TC2*1.5f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BASE")
        {
            if (jump || !SK[0].land || !SK[1].land)
            {
                anim.SetTrigger("LANDING");

                jump = false;
                SK[0].land = true;
                SK[0].land = true;
            }
            if (longjump)
            {
                SF.followSpeed = GM.playerspeed;
            }
            grounded = true;
            longjump = false;
            if(jump && grounded) { anim.SetTrigger("LANDING"); }
            //SK[0].skatejump = false;
            //SK[1].skatejump = false;
            //SK[0].springjump = false;
            //SK[1].springjump = false;
        }    
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "BASE")
        {
            grounded = false;
        }
        if (collision.gameObject.tag == "JUMP")
        {
            anim.SetTrigger("JUMP");
            SK[0].faster = false;
            SK[1].faster = false;
            SF.followSpeed = GM.playerspeed * 3;
            longjump = true;
            jump = true;
        }
    }
    IEnumerator lander()
    {
        yield return new WaitForSeconds(0.0f);
        anim.SetLayerWeight(2, 0.3f);
        anim.SetTrigger("LANDING");
    }
    public void lastpoint()
    {
        // SF.motion.rotationOffset= new Vector3(0,GM.myrot,0);
        this.transform.rotation = Quaternion.Euler(0, GM.myrot, 0);
        GM.PE1.SetActive(true);
        GM.PE2.SetActive(true);
        MC.transform.parent = cam.transform;
        reached = true;
        if(AudioManager.instance) AudioManager.instance.Play("Awe");
        anim.SetTrigger("DANCE");
        //Calling Level Complete
        GameCanvas.game.MakeGameResult(0,0);
        //UIM.LC();
    }
    public void Applymotionrot()
    {
        //transform.GetComponentInParent<SplineFollower>().motion.applyRotationX = true;
        //transform.GetComponentInParent<SplineFollower>().motion.applyRotationY= true;
        //transform.GetComponentInParent<SplineFollower>().motion.applyRotationZ= true;
    }
}