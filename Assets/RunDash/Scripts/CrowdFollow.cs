using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdFollow : MonoBehaviour
{
    public Vector3 followOffset;
    public Transform charToFollow;
    public float damping;

    public static Player instance;
    [Header("Controls")]
    [Header("Components to Assign")]
    // public Rigidbody rb;
    //public Camera cam;
    Vector2 startPos, endPos;
    private Animator PlayerAnim;
    private string currentAnimaton;
    public SkinnedMeshRenderer skin;

    //Collider m_collider;
    // public ParticleSystem CoinParticle;
    //public ParticleSystem ObstacleParticle;
    // public TimeManager timeManager;
    //  public Rigidbody[] RagDollRb;
    //  public GameObject DummyPlayer;
    //   public GameObject Player_;
    //  public Transform playerpos;
    Vector3 matric;
    Vector3 moveToPosition;
    //  public List<GameObject> trucks;
    //  public List<GameObject> dummytrucks;
    // public int jumpforceTrucktoTruck;
    //public int FOV;

    [Header("FinalPlatform")]
    public int finalJumpForce;
    public int finalJumpSpeed;
    public bool isvictory;
    public bool iscollision;
    public bool isFinal;
    public float CourtineTime;
    public float DanceCourtineTime;
    //public float slowdownFactor;
     public ParticleSystem particle;
    public bool specialLevel;
    public SkinnedMeshRenderer skin1;
    public SkinnedMeshRenderer skin2;


    //Animation States
    const string PLAYER_RUN = "Fast_Run";
    const string PLAYER_POSE = "Pose";
    const string PLAYER_KICK = "Flying_Kick";
    const string PLAYER_FLIP = "Forward_Flip";
    const string PLAYER_JUMP_POSE = "Jumping_Pose";
    const string PLAYER_JUMP_DEATH = "Death";
    const string PLAYER_JUMP_VICTORY = "Victory";
    const string PLAYER_JUMP_OVER = "Jump_Over";
    const string PLAYER_JUMP_OVER_CON = "Jump_Over_CON";
    const string PLAYER_ROLL = "Roll";
    const string PLAYER_SLOW_RUN = "Slow_Run";
    const string PLAYER_FALLING = "Falling";
    const string PLAYER_IDLE = "Idle";
    const string PLAYER_FINAL_KICK = "Final_Kick";
    const string PLAYER_FINAL_FLIP = "Flip_Kick";
    const string PLAYER_DEFEAT = "Defeat";


    private void Awake()
    {
        PlayerAnim = GetComponent<Animator>();
    }
    private void Start()
    {
        Vibration.Init();
        if (specialLevel == true)
        {
        skin1 = gameObject.transform.GetChild(7).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<SkinnedMeshRenderer>();
        skin2 = gameObject.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();

        }
        //Debug.Log(skin1.transform.name);
        skin =gameObject.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        
    }
    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;
        PlayerAnim.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

    private void FixedUpdate()
    {
        Vector3 smoothPos = Vector3.Lerp(transform.position, charToFollow.position, Time.deltaTime * damping);
        transform.position = smoothPos;
        transform.eulerAngles = charToFollow.eulerAngles;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {

            AudioManager.instance.Play("Death");
            uimanagr.instance.lost_panel();
            ChangeAnimationState(PLAYER_JUMP_DEATH);
            Vector3 playerpos = transform.position;
        
            //  particle.Play(true);
        }

        if (other.gameObject.CompareTag("FinalPlatform"))
        {
           
          //  StartCoroutine(DanceDelay());
            ChangeAnimationState(PLAYER_JUMP_VICTORY);
            // StartCoroutine(VictoryDelay());
           // print("Platform");
        }
        //IEnumerator DanceDelay()
        //{
        //    yield return new WaitForSeconds(DanceCourtineTime);
        //    if (isvictory == true)
        //    {
        //        ChangeAnimationState(PLAYER_JUMP_VICTORY);
        //    }
        //}
        //IEnumerator VictoryDelay()
        //{
        //    yield return new WaitForSeconds(CourtineTime);
        //    ChangeAnimationState(PLAYER_IDLE);
        //    ChangeAnimationState(PLAYER_IDLE);
        //    FireParticle.Play(true);
        //    uimanagr.instance.win_panel();
        //}

        if (other.gameObject.CompareTag("MainTruck"))
        {
            ChangeAnimationState(PLAYER_RUN);
           // print("FastRun");
        }


        //if (other.gameObject.CompareTag("Obstacles"))
        //{
        //    // ObstacleParticle.Play(true);
        //    AudioManager.instance.Play("Death");
        //    uimanagr.instance.lost_panel();
        //   // print("Dead");
        //    ChangeAnimationState(PLAYER_JUMP_DEATH);
        //    //  particle.Play(true);
        //}

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RedEnable"))
        {
            ChangeAnimationState(PLAYER_JUMP_DEATH);
        
            //    StartCoroutine(YellowDisable());
        }
        if (other.gameObject.CompareTag("YellowEnable"))
        {
    
            ChangeAnimationState(PLAYER_JUMP_DEATH);
            // StartCoroutine(RedDisable());
        }

        if (other.gameObject.CompareTag("Truck"))
        {
            ChangeAnimationState(PLAYER_POSE);
            AudioManager.instance.Play("Jump");
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            AudioManager.instance.Play("Hit");
            //transform.parent = other.transform;
            //ChangeAnimationState(PLAYER_RUN);
            //EnemyRb.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 20) * 10, ForceMode.Impulse);
        }

        if (other.gameObject.CompareTag("FinalJump"))
        {
            // timeManager.DoSlowMotion();
            ChangeAnimationState(PLAYER_POSE);
        }
        if (other.gameObject.CompareTag("HighJump"))
        {
            ChangeAnimationState(PLAYER_POSE);
            // speed = poseSpeedTrucktoTruck; //level1 and level 2 110 and level 3 and 4
        }
        if (other.gameObject.CompareTag("SpringBoard"))
        {
            // timeManager.DoSlowMotion();
            ChangeAnimationState(PLAYER_POSE);
        }

        if (other.gameObject.CompareTag("ResetJump"))
        {
            //cam.transform.DORotate(new Vector3(13, -1.4f, 0), 2f);
            ChangeAnimationState(PLAYER_RUN);
        }
        if (other.gameObject.CompareTag("Pose"))
        {
            // timeManager.DoSlowMotion();
            ChangeAnimationState(PLAYER_POSE);
        }
        if (other.gameObject.CompareTag("PoseHigh"))
        {
            // timeManager.DoSlowMotion();
            ChangeAnimationState(PLAYER_POSE);
            //jumpForce = 2500;
        }

        if (other.gameObject.CompareTag("Kick"))
        {
            ChangeAnimationState(PLAYER_KICK);
            //Time.timeScale = slowdownFactor * Time.deltaTime;
            // speed = 90;
            // jumpForce = 1100;
        }
        if (other.gameObject.CompareTag("FinalKick"))
        {
            ChangeAnimationState(PLAYER_FINAL_KICK);
            StartCoroutine(DelayFinalKick());
            if (iscollision == true)
            {

                print("Collision");
            }

            // transform.DOMove(new Vector3(0, -4, 3112f), 0.5f);

            //main_m_collider.GetComponent<CapsuleCollider>().isTrigger = true;

        }
        IEnumerator DelayFinalKick()
        {
            yield return new WaitForSeconds(1.7f);
            ChangeAnimationState(PLAYER_IDLE);
        }
        if (other.gameObject.CompareTag("Run"))
        {
            //cam.transform.DORotate(new Vector3(13, -1.4f, 0), 2f);
            ChangeAnimationState(PLAYER_RUN);
            //   timeManager.ResetSlowMotion();
            //AudioManager.instance.Play("Run");
        }
        if (other.gameObject.CompareTag("SlowRun"))
        {
            ChangeAnimationState(PLAYER_SLOW_RUN);
            //  timeManager.ResetSlowMotion();
        }
        //if (other.gameObject.CompareTag("Roll"))
        //{
        //    ChangeAnimationState(PLAYER_ROLL);
        //    cam.transform.DORotate(new Vector3(13, -1.4f, 0), 2f);
        //    //ChangeAnimationState(PLAYER_RUN);
        //}
        if (other.gameObject.CompareTag("Fall"))
        {
            ChangeAnimationState(PLAYER_FALLING);
            //ChangeAnimationState(PLAYER_RUN);
        }
        if (other.gameObject.CompareTag("Flip"))
        {
            // ChangeAnimationState(PLAYER_FLIP);
            ChangeAnimationState(PLAYER_JUMP_POSE);

        }
        if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            // CoinParticle.Play(true);
            AudioManager.instance.Play("Coins");
            // m_collider.enabled = false;
        }

        if (other.gameObject.CompareTag("DownJump"))
        {
            ChangeAnimationState(PLAYER_JUMP_OVER);
        }

        if (other.gameObject.CompareTag("ResetSpeed"))
        {
            ChangeAnimationState(PLAYER_POSE);
        }
        if (other.gameObject.CompareTag("FlipKick"))
        {
            ChangeAnimationState(PLAYER_FLIP);
            Time.timeScale = 35 * Time.fixedDeltaTime;
            //  timeManager.DoSlowMotion();
            //movement = false;
            //speed = 131f;
        }
        if (other.gameObject.CompareTag("Stop"))
        {
            ChangeAnimationState(PLAYER_DEFEAT);
            isvictory = false;
            isFinal = false;
            uimanagr.instance.lost_panel();
        } 
        if (other.gameObject.CompareTag("additionGate"))
        {
            particle.Play();
        }
        if (other.gameObject.CompareTag("YellowColor"))
        {
            if (specialLevel == true)
            {
                skin1.material = Player.instance.YellowSkin;
                skin2.material = Player.instance.YellowSkin;
            }
            skin.material = Player.instance.YellowSkin;
           
                //Debug.Log("Yellow");
        }
        if (other.gameObject.CompareTag("RedColor"))
        {
            if (specialLevel == true)
            {
                skin1.material = Player.instance.RedSkin;
                skin2.material = Player.instance.RedSkin;
            }
            skin.material = Player.instance.RedSkin;
          
        }
        if (other.gameObject.CompareTag("RandomColor"))
        {
            //Debug.Log("trigered");

            if (Player.instance.r == 1)
            {
                if (specialLevel == true)
                {
                    skin1.material = Player.instance.YellowSkin;
                    skin2.material = Player.instance.YellowSkin;
                }
                skin.material = Player.instance.YellowSkin;
              
            }
            if (Player.instance.r == 2)
            {
                if (specialLevel == true)
                {
                    skin1.material = Player.instance.RedSkin;
                    skin2.material = Player.instance.RedSkin;
                }
                skin.material = Player.instance.RedSkin;
     
            }
        }
        }
}
