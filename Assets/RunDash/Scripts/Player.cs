using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using DitzeGames.Effects;
using SWS;
using Cinemachine;
using StylizedWater2;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public static Player instance;
    [Header("Controls")]
    public int jumpForce = 1000;
    // public float moveSpeed;
    public float speed;
    [Header("Components to Assign")]
    public Rigidbody rb;
    //public Camera cam;
    public bool touchinput = false;
    float mousex;
    Vector3 direction;
    public bool movement;
    Vector2 startPos, endPos;
    private Animator PlayerAnim;
    private string currentAnimaton;
    SkinnedMeshRenderer skin;
    SkinnedMeshRenderer skin1;
    //Collider m_collider;
    public ParticleSystem CoinParticle;
    public GameObject ShieldEnable;
    //public ParticleSystem ObstacleParticle;
    public TimeManager timeManager;
    public Rigidbody[] BoxRb;
    public Rigidbody[] RagDollRb;
    public GameObject DummyPlayer;
    public GameObject Player_;
    public Transform playerpos;
    public Camera cam;
    public Rigidbody EnemyRb;
    public ParticleSystem FinalParticle;
    public ParticleSystem FireParticle;
    Vector3 matric;
    Vector3 moveToPosition;
    public List<GameObject> trucks;
    public List<GameObject> dummytrucks;
    public GameObject IntroCanvas;
    public GameObject UICanvas;
    public int r;
    // public int jumpforceTrucktoTruck;
    //public int FOV;
    [Header("Skins")]
    public SkinnedMeshRenderer skinnedMesh;
    public SkinnedMeshRenderer skinnedMesh1;
    public Material YellowSkin;
    public Material RedSkin;
    public List<GameObject> RedEnable;
    public List<GameObject> YellowEnable;

    [Header("FinalPlatform")]
    public int finalJumpForce;
    public int finalJumpSpeed;
    public bool isvictory;
    public bool iscollision;
    public bool isFinal;
    public bool isBossFinal;
    public bool isFight;
    public bool isControl;
    public bool isControlWork;
    public bool isControlFall;
    public bool isWaterRun;
    public bool isWaterFallRun;
    public bool isCatchLevel;
    public bool isShieldFly;
    public bool isBrickLevel;
    public float CourtineTime;
    public float DanceCourtineTime;
    public int count = 0;

    //public float slowdownFactor;

    [Header("JumpSpeed")]
    public int jumpSpeedTrucktoTruck;
    public int kickspeedTrucktotruck;
    public int poseSpeedTrucktoTruck;
    public int runSpeedTrucktoTruck;
    public int flipSpeedTrucktoTruck;
    public int resetJumpSpeedTrucktoTruck;

    [Header("OnEnableTruckMoveScript")]
    public List<splineMove> truckScript;
    public Collider m_collider;
    public Collider main_m_collider;

    [Header("CoinCount")]
    public int coinsCount;
    public TextMeshProUGUI coinsText;

    [Header("Boss")]
    public GameObject centerSurrounderScript;
    public Transform Boss;
    public CinemachineVirtualCamera cineCam;
    int playerHealth;
    public GameObject canvas;
    public ParticleSystem PlayerPunch;

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
    const string PLAYER_PUNCH = "Punch";
    const string PLAYER_HIPHOPDANCE = "HipHopDancing";
    const string PLAYER_SLIDE = "Slide";
    const string PLAYER_DIVE = "Dive";
    const string PLAYER_COVER = "Cover";
    const string PLAYER_FLY = "Fly";
    const string PLAYER_WALLRUN = "WallRun";
    const string PLAYER_AIRPUNCH = "AirPunch";
    const string PLAYER_FLYINGCATCH = "Flying";

    public float horizontalAxis;
    public GameObject YellowParticle;
    public GameObject RedParticle;
    public GameObject MainParticle;
    public float sensitivity = 6000f;
    public int Coins;

    public GameObject BossDisable;
    public GameObject BossEnable;
    public GameObject BossFreeEnable;
    public GameObject cam1;
    public GameObject cam2;
    public GameObject particle1;
    public GameObject particle2;
    float delay;
    public List<GameObject> SplineActive;
    private void Awake()
    {
        if (instance == null) instance = this;
        Physics.gravity = new Vector3(0, -40, 0);
        PlayerAnim = GetComponent<Animator>();
        Coins = PlayerPrefs.GetInt("Coins");
    }
    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;
        PlayerAnim.Play(newAnimation);
        currentAnimaton = newAnimation;
    }
    void Start()
    {
        Application.targetFrameRate = 180;
        skin = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        skin1 = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        rb = GetComponent<Rigidbody>();
        //  movement = true;
        isControl = false;
        isControlFall = false;
        //isControlWork = false;
        playerHealth = 10;
       // PlayerAnim.enabled = false;
        Vibration.Init();
//        coinsText.text = coinsCount.ToString();
    }
    public Vector2 v;
    private bool isDrag;
    private void Update()
    {
        fighttouch();
        if (isControlWork)
        {
            if (BossFight.instance.bosshealth <= 0 && playerHealth > 0)
            {
                isControl = false;
                isControlFall = false;
                StartCoroutine(DelayDance());
            }

            IEnumerator DelayDance()
            {
                yield return new WaitForSeconds(2f);
                PlayerAnim.Play("HipHopDancing");
            }
        }

        if (isDrag)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, 7.5f), transform.position.y, transform.position.z);
        }
        
        SwipeContol();
        // if (Input.GetMouseButton(0))
         v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, FOV, .5f * Time.deltaTime);
        //   Player_.GetComponent<Rigidbody>().AddForce(transform.forward * 1000f); 
    }
    void FixedUpdate()
    {
        if (movement)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void SwipeContol()
    {
        if (movement)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                rb.isKinematic = false;
            }
            
            if (Input.GetMouseButton(0))
            {
                endPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                Vector3 diff = endPos - startPos;
                transform.position += (new Vector3(diff.x, 0, 0) * sensitivity * Time.deltaTime);
                startPos = endPos;
                if (isBrickLevel == true)
                {
                    transform.position = new Vector3(Mathf.Clamp(transform.position.x, -40, 40), transform.position.y, transform.position.z);
                }
                if (isWaterRun == true)
                {
                    transform.position = new Vector3(Mathf.Clamp(transform.position.x, 10, 65), transform.position.y, transform.position.z);
                }
                if (isWaterFallRun == true)
                {
                    transform.position = new Vector3(Mathf.Clamp(transform.position.x, -40, 170), transform.position.y, transform.position.z);
                } 
                if (isCatchLevel == true)
                {
                    transform.position = new Vector3(Mathf.Clamp(transform.position.x, -27, 29), transform.position.y, transform.position.z);
                }
            }
        }
    }
    private void Jump()
    {
        rb.velocity = Vector3.zero;
        rb.AddRelativeForce(Vector3.up * jumpForce);
    }

    private void OnEnable()
    {
        GameEvents.TapToPlay += StartTime;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= StartTime;
    }

    private void StartTime()
    {
        movement = true;
        ChangeAnimationState(PLAYER_RUN);
        isDrag = true;
        DOVirtual.DelayedCall(0.5f, () => isDrag = false);
    }

    public void cameramove()
    {
        //Assigning new position to moveTOPosition
        moveToPosition = new Vector3(-2, 23f, 1415f);
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, moveToPosition, ref matric, speed);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            cam.transform.DORotate(new Vector3(30, 0, 0), 2f);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 65, 40f * Time.deltaTime);
            AudioManager.instance.Play("Death");
            Vibration.VibratePop();
            uimanagr.instance.lost_panel();
            CameraEffects.ShakeOnce();
            ChangeAnimationState(PLAYER_JUMP_DEATH);
            Vector3 playerpos = transform.position;
            DummyPlayer.transform.position = playerpos;
            Player_.SetActive(false);
            cam.transform.parent = null;
            DummyPlayer.SetActive(true);
            speed = 0;
            jumpForce = 0;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            isControl = true;
        }
        if (other.gameObject.CompareTag("BossFall"))
        {
            isControl = false;
        }
        if (isFinal)
        {
            if (other.gameObject.CompareTag("FinalPlatform"))
            {
                StartCoroutine(DanceDelay());
                StartCoroutine(VictoryDelay());
                speed = 0;
                jumpForce = 0;
                movement = false;
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 55, 40f * Time.deltaTime);
              //  m_collider.GetComponent<SphereCollider>().isTrigger = true;
            }
            IEnumerator DanceDelay()
            {
                yield return new WaitForSeconds(DanceCourtineTime);
                if (isvictory == true)
                {
                    ChangeAnimationState(PLAYER_JUMP_VICTORY);
                }
            }
            IEnumerator VictoryDelay()
            {
                yield return new WaitForSeconds(CourtineTime);
                // ChangeAnimationState(PLAYER_IDLE);
                cam.transform.DORotate(new Vector3(25, -185, 0), 1f);
                cam.transform.DOMove(transform.position + new Vector3(-3f, 20f, 40f), 1f);
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60, 90f * Time.deltaTime);
                // ChangeAnimationState(PLAYER_IDLE);
                // FireParticle.Play(true);

                uimanagr.instance.win_panel();
                for (int i = 0; i < dummytrucks.Count; i++)
                {
                    dummytrucks[i].SetActive(true);
                    dummytrucks[i].GetComponent<TruckMove>().move = true;
                }
                for (int i = 0; i < trucks.Count; i++)
                {
                    trucks[i].SetActive(false);
                }
            }
        }

        if (other.gameObject.CompareTag("MainTruck"))
        {
         //   ChangeAnimationState(PLAYER_RUN);
            //Time.timeScale = 0.1f;
        }

        if (other.gameObject.CompareTag("Run"))
        {
            //cam.transform.DORotate(new Vector3(13, -1.4f, 0), 2f);
            ChangeAnimationState(PLAYER_RUN);
            speed = runSpeedTrucktoTruck; //level1 and level 2 70 and level 3 and 4
            timeManager.ResetSlowMotion();
            //AudioManager.instance.Play("Run");
        }
        if (other.gameObject.CompareTag("WaterRun"))
        {
            //cam.transform.DORotate(new Vector3(13, -1.4f, 0), 2f);
            ChangeAnimationState(PLAYER_RUN);
            speed = runSpeedTrucktoTruck; //level1 and level 2 70 and level 3 and 4
            timeManager.ResetSlowMotion();
            //AudioManager.instance.Play("Run");
            cam.transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, -2f), 2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                cam.transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, 2f), 2f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            });
            isWaterRun = true;

        }
        if (other.gameObject.CompareTag("WallRun"))
        {
            //cam.transform.DORotate(new Vector3(13, -1.4f, 0), 2f);
            ChangeAnimationState(PLAYER_WALLRUN);
            speed = runSpeedTrucktoTruck; //level1 and level 2 70 and level 3 and 4
            timeManager.ResetSlowMotion();
            //AudioManager.instance.Play("Run");
        }
        if (other.gameObject.CompareTag("Fall"))
        {
            ChangeAnimationState(PLAYER_POSE);
            uimanagr.instance.lost_panel();
            movement = false;
            speed = 0;
            jumpForce = 0;
            cam.transform.DORotate(new Vector3(50, 0, 0), 2f);
            // cam.transform.DOMove(transform.position + new Vector3(0, 0,0), 1f);
            // Time.timeScale = 0;
        }


        if (other.gameObject.CompareTag("Obstacles"))
        {
            cam.transform.DORotate(new Vector3(30, 0, 0), 2f);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 70, 40f * Time.deltaTime);
            AudioManager.instance.Play("Death");
            Vibration.VibratePop();
            uimanagr.instance.lost_panel();
            CameraEffects.ShakeOnce();
            ChangeAnimationState(PLAYER_JUMP_DEATH);
            dummyplayer();
            speed = 0;
            jumpForce = 0;
        }

        if (other.gameObject.CompareTag("Box"))
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 5000, ForceMode.Force);
            other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 5000, ForceMode.Force);
            //other.gameObject.GetComponent<Collider>().enabled = false;
        }

        if (other.gameObject.CompareTag("GlassActive"))
        {
            other.transform.GetChild(0).gameObject.SetActive(false);
            other.transform.GetChild(1).gameObject.SetActive(true);
            CameraEffects.ShakeOnce(0.5f, 5f);
            AudioManager.instance.Play("Glass");
        } 
        if (other.gameObject.CompareTag("WallActive"))
        {
            other.transform.GetChild(0).gameObject.SetActive(false);
            other.transform.GetChild(1).gameObject.SetActive(true);
            CameraEffects.ShakeOnce(0.5f, 5f);
           // AudioManager.instance.Play("Glass");
        }

        if (isBossFinal)
        {
            if (other.gameObject.CompareTag("Boss"))
            {
                StartCoroutine(DanceDelay());
                StartCoroutine(VictoryDelay());
                speed = 0;
                jumpForce = 0;
                movement = false;
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 55, 40f * Time.deltaTime);
                // m_collider.GetComponent<SphereCollider>().isTrigger = true;
                // FinalParticle.Play(true);
                ChangeAnimationState(PLAYER_IDLE);
                ChangeAnimationState(PLAYER_SLOW_RUN);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                canvas.SetActive(false);
            }
            IEnumerator DanceDelay()
            {
                yield return new WaitForSeconds(DanceCourtineTime);
                if (isvictory == true)
                {
                    //    ChangeAnimationState(PLAYER_JUMP_VICTORY);
                }
            }
            IEnumerator VictoryDelay()
            {
                yield return new WaitForSeconds(CourtineTime);
                // ChangeAnimationState(PLAYER_IDLE);
                // cam.transform.DORotate(new Vector3(25, -185, 0), 1f);
                //  cam.transform.DOMove(transform.position + new Vector3(-3f, 20f, 40f), 1f);
                speed = 0;
                //  cam.transform.DOMove(transform.position + new Vector3(0, 20f, -40f), 1f);
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60, 90f * Time.deltaTime);
                NavmeshAi.instance.navMesh.enabled = true;
                NavmeshAi.instance.GetComponent<NavmeshAi>().enabled = true;
                //  bossControllerScript.GetComponent<BossController>().enabled = true;
                centerSurrounderScript.GetComponent<CenterSurrounder>().enabled = true;
                // Player_.transform.DOMove(transform.position + new Vector3(-1.1f, -1f, 60f), 4f);
                // ChangeAnimationState(PLAYER_IDLE);
                // FireParticle.Play(true);
                //uimanagr.instance.win_panel();
                for (int i = 0; i < dummytrucks.Count; i++)
                {
                    dummytrucks[i].SetActive(true);
                }
            }
        }
        if (isFinal)
        {
            if (other.gameObject.CompareTag("EndFinalPlatform"))
            {
                ChangeAnimationState(PLAYER_IDLE);
                StartCoroutine(DanceDelay());
                StartCoroutine(VictoryDelay());
                speed = 0;
                jumpForce = 0;
                movement = false;
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 120, 40f * Time.deltaTime);
                m_collider.GetComponent<SphereCollider>().isTrigger = true;

                // FinalParticle.Play(true);

            }
            IEnumerator DanceDelay()
            {
                yield return new WaitForSeconds(DanceCourtineTime);
                if (isvictory == true)
                {
                    ChangeAnimationState(PLAYER_JUMP_VICTORY);
                }
            }
            IEnumerator VictoryDelay()
            {
                yield return new WaitForSeconds(CourtineTime);
                // ChangeAnimationState(PLAYER_IDLE);
                cam.transform.DORotate(new Vector3(25, -185, 0), 1f);
                cam.transform.DOMove(transform.position + new Vector3(-3f, 20f, 40f), 1f);
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 100, 90f * Time.deltaTime);
                // 
                // FireParticle.Play(true);

                uimanagr.instance.win_panel();
                for (int i = 0; i < dummytrucks.Count; i++)
                {
                    dummytrucks[i].SetActive(true);
                }
            }
        }
        if (other.gameObject.CompareTag("RunJump"))
        {
            ChangeAnimationState(PLAYER_FLIP);
            Jump();
            jumpForce = 3500;
            speed = 130;
            Time.timeScale = 45 * Time.deltaTime;
            // speed = poseSpeedTrucktoTruck; //level1 and level 2 110 and level 3 and 4
        }
        if (other.gameObject.CompareTag("SpringBoard"))
        {
            AudioManager.instance.Play("Jump");
            Vibration.VibratePop();
            ChangeAnimationState(PLAYER_POSE);
            Jump();
            // jumpForce = 5000;
            speed = 110;//level1 and level 2 85 and level 3 and 4
        }
        if (other.gameObject.CompareTag("JumpRun"))
        {
            // timeManager.DoSlowMotion();
            ChangeAnimationState(PLAYER_RUN);
            // Jump();
            speed = 40; //level1 and level 2 110 and level 3 and 4 // jumpForce = jumpforceTrucktoTruck;
            AudioManager.instance.Play("Jump");
            Time.timeScale = 45 * Time.deltaTime;
        }

    }
    public void dummyplayer()
    {
        Vector3 playerpos = transform.position;
        DummyPlayer.transform.position = playerpos;
        Player_.SetActive(false);
        cam.transform.parent = null;
        DummyPlayer.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacles"))
        {
            cam.transform.DORotate(new Vector3(30, 0, 0), 2f);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 70, 40f * Time.deltaTime);
            AudioManager.instance.Play("Death");
            Vibration.VibratePop();
            uimanagr.instance.lost_panel();
            CameraEffects.ShakeOnce(0.5f, 10f);
            ChangeAnimationState(PLAYER_JUMP_DEATH);
            dummyplayer();
            speed = 0;
            jumpForce = 0;
        }

        if (other.gameObject.CompareTag("BossFight"))
        {
            speed = 0;
            cineCam.enabled = false;
        }
        if (other.gameObject.CompareTag("Truck"))
        {
            ChangeAnimationState(PLAYER_POSE);
            Jump();
            speed = jumpSpeedTrucktoTruck; //level1 and level 2 110 and level 3 and 4 // jumpForce = jumpforceTrucktoTruck;
            AudioManager.instance.Play("Jump");
            Time.timeScale = 45 * Time.deltaTime;
        }


        if (other.gameObject.CompareTag("Enemy"))
        {
           AudioManager.instance.Play("Hit");
            Vibration.VibratePop();
            CameraEffects.ShakeOnce(0.5f, 10f);
        }

        if (other.gameObject.CompareTag("CameraChange"))
        {
            AudioManager.instance.Play("Jump");
            cam.transform.DORotate(new Vector3(30, 0, 0), 2f);
            ChangeAnimationState(PLAYER_JUMP_OVER);
            particle1.SetActive(false);
            particle2.SetActive(false);
        }
        if (other.gameObject.CompareTag("CameraRotate"))
        {
            AudioManager.instance.Play("Jump");
            Player_.transform.DORotate(new Vector3(90, 0, 0), 2f);
            cam2.SetActive(true);
            isWaterRun = false;
            isWaterFallRun = true;
            sensitivity = 20000;
            for (int i = 0; i < SplineActive.Count; i++) 
            {
                SplineActive[i].SetActive(true);
            }
        }
        
        if (other.gameObject.CompareTag("BlendCameraRotate"))
        {
            AudioManager.instance.Play("Jump");
            ChangeAnimationState(PLAYER_FLIP);
            speed = 300;
            Player_.transform.DORotate(new Vector3(30, 0, 0), 2f);
            isWaterRun = false;
            isWaterFallRun = true;
            sensitivity = 20000;
        }

        if (other.gameObject.CompareTag("TRK"))
        {
            cam.transform.DORotate(new Vector3(13, -1.4f, 0), 2f);
            ChangeAnimationState(PLAYER_RUN);
        }
        if (other.gameObject.CompareTag("ResetCameraChange"))
        {
            AudioManager.instance.Play("Jump");
            cam.transform.DORotate(new Vector3(30, 0, 0), 2f);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 100, 40f * Time.deltaTime);
            ChangeAnimationState(PLAYER_JUMP_OVER);
        }

        if (other.gameObject.CompareTag("FinalJump"))
        {
            ChangeAnimationState(PLAYER_POSE);
            Jump();
            jumpForce = finalJumpForce;
            speed = finalJumpSpeed;
            AudioManager.instance.Play("Jump");
            Time.timeScale = 45 * Time.deltaTime;
        }
        if (other.gameObject.CompareTag("HighJump"))
        {
            ChangeAnimationState(PLAYER_POSE);
            Jump();
            jumpForce = 3500;
            speed = 150;
            Time.timeScale = 45 * Time.deltaTime;
            // speed = poseSpeedTrucktoTruck; //level1 and level 2 110 and level 3 and 4
        }

        if (other.gameObject.CompareTag("SpringBoard"))
        {
            AudioManager.instance.Play("Jump");
            Vibration.VibratePop();
            ChangeAnimationState(PLAYER_POSE);
            Jump();
            // jumpForce = 5000;
            speed = 110;//level1 and level 2 85 and level 3 and 4
        }

        if (other.gameObject.CompareTag("ResetJump"))
        {
            //cam.transform.DORotate(new Vector3(13, -1.4f, 0), 2f);
            ChangeAnimationState(PLAYER_RUN);
            speed = runSpeedTrucktoTruck; //level1 and level 2 70 and level 3 and 4
            jumpForce = 1550;
            timeManager.ResetSlowMotion();
        }
        if (other.gameObject.CompareTag("Pose"))
        {
            speed = poseSpeedTrucktoTruck; //level1 and level 2 100 and level 3 and 4
            ChangeAnimationState(PLAYER_POSE);
        }
        if (other.gameObject.CompareTag("PoseHigh"))
        {
            ChangeAnimationState(PLAYER_POSE);
            Jump();
            speed = 70;
            //jumpForce = 2500;
        }

        if (other.gameObject.CompareTag("Kick"))
        {
            timeManager.DoSlowMotion();
            ChangeAnimationState(PLAYER_KICK);
            speed = kickspeedTrucktotruck;
            //Time.timeScale = slowdownFactor * Time.deltaTime;
            // speed = 90;
            // jumpForce = 1100;
        }
        if (other.gameObject.CompareTag("AirPunch"))
        {
            timeManager.DoSlowMotion();
            ChangeAnimationState(PLAYER_AIRPUNCH);
            speed = 200;

            //Time.timeScale = slowdownFactor * Time.deltaTime;
            // speed = 90;
            // jumpForce = 1100;
        }
        if (other.gameObject.CompareTag("FinalKick"))
        {
            for (int i = 0; i < trucks.Count; i++)
            {
                trucks[i].gameObject.SetActive(false);
            }
            timeManager.DoSlowMotion();
            ChangeAnimationState(PLAYER_FINAL_KICK);
            Vibration.VibratePop();
            StartCoroutine(DelayFinalKick());
            if (iscollision == true)
            {
                m_collider.GetComponent<Collider>().isTrigger = false;
            }

            // transform.DOMove(new Vector3(0, -4, 3112f), 0.5f);
            movement = false;
            //main_m_collider.GetComponent<CapsuleCollider>().isTrigger = true;
            speed = 0;
            jumpForce = 0;
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
            speed = runSpeedTrucktoTruck; //level1 and level 2 70 and level 3 and 4
            timeManager.ResetSlowMotion();
            //AudioManager.instance.Play("Run");
        }
        if (other.gameObject.CompareTag("Fly"))
        {
            //cam.transform.DORotate(new Vector3(13, -1.4f, 0), 2f);
            ChangeAnimationState(PLAYER_FLY);
            speed = 300; //250 //level1 and level 2 70 and level 3 and 4
            //Physics.gravity = new Vector3(0, -1, 0);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //GetComponent<Collider>().enabled = false;
            Time.timeScale = 45 * Time.deltaTime;
            // timeManager.ResetSlowMotion();
            //AudioManager.instance.Play("Run");
            cam.transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, -9f), 2f).SetEase(Ease.Linear).OnComplete(() =>
             {
                 cam.transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, 9f), 2f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
             });
        }
        if (other.gameObject.CompareTag("FlyingCatch"))
        {
            //cam.transform.DORotate(new Vector3(13, -1.4f, 0), 2f);
            //ChangeAnimationState(PLAYER_FLYINGCATCH);
            ShieldEnable.SetActive(false);
            BossDisable.SetActive(false);
            BossEnable.SetActive(true);
            speed = 250; //level1 and level 2 70 and level 3 and 4
            //Physics.gravity = new Vector3(0, -1, 0);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //GetComponent<Collider>().enabled = false;
            timeManager.ResetSlowMotion();
            //AudioManager.instance.Play("Run");
            cam.transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, -9f), 2f).SetEase(Ease.Linear).OnComplete(() =>
             {
                 cam.transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, 9f), 2f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
             });
        }
        if (other.gameObject.CompareTag("BossFree"))
        {
            BossEnable.SetActive(false);
            BossFreeEnable.SetActive(true);
            isControlFall = true;
            movement = false;
            this.transform.DOMove(transform.position + new Vector3(0, 10, 0), 0f);
            speed = 250; //level1 and level 2 70 and level 3 and 4
            //Physics.gravity = new Vector3(0, -1, 0);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //GetComponent<Collider>().enabled = false;
            timeManager.ResetSlowMotion();
            //AudioManager.instance.Play("Run");
            cam.transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, -9f), 2f).SetEase(Ease.Linear).OnComplete(() =>
             {
                 cam.transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, 9f), 2f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
             });
            speed = 0;

        }
        if (other.gameObject.CompareTag("SlowRun"))
        {
            cam.transform.DORotate(new Vector3(13, -1.4f, 0), 2f);
            ChangeAnimationState(PLAYER_SLOW_RUN);
            speed = 90;
        }

        if (other.gameObject.CompareTag("Fall"))
        {
            ChangeAnimationState(PLAYER_FALLING);
            //ChangeAnimationState(PLAYER_RUN);
        }
        if (other.gameObject.CompareTag("Flip"))
        {
            // ChangeAnimationState(PLAYER_FLIP);
            timeManager.DoSlowMotion();
            ChangeAnimationState(PLAYER_JUMP_POSE);
            speed = flipSpeedTrucktoTruck;  //level1 and level 2 90 and level 3 and 4

        }
        if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            CoinParticle.Play(true);
            AudioManager.instance.Play("Coins");
            coinsCount++;
          //  coinsText.text = coinsCount.ToString();
            // m_collider.enabled = false;
        }
        if (other.gameObject.CompareTag("Shield"))
        {
            other.gameObject.SetActive(false);
            CoinParticle.Play(true);
            if(isShieldFly == true)
            {
                this.transform.DOMove(transform.position + new Vector3(0, 5, 0), 0f);
            }
            AudioManager.instance.Play("Jump");
            // m_collider.enabled = false;
        }
        if (other.gameObject.CompareTag("Idle"))
        {
            ChangeAnimationState(PLAYER_IDLE);
            // m_collider.enabled = false;
        }

        if (other.gameObject.CompareTag("DownJump"))
        {
            ChangeAnimationState(PLAYER_JUMP_OVER);
            speed = 90;
        }

        if (other.gameObject.CompareTag("TruckMove"))
        {
            for (int i = 0; i < truckScript.Count; i++)
            {
                truckScript[i].enabled = true;
            }
        }
        if (other.gameObject.CompareTag("ResetSpeed"))
        {
            ChangeAnimationState(PLAYER_POSE);
            speed = resetJumpSpeedTrucktoTruck;
        }
        if (other.gameObject.CompareTag("FlipKick"))
        {
            ChangeAnimationState(PLAYER_FLIP);
            Time.timeScale = 45 * Time.fixedDeltaTime;
            //  timeManager.DoSlowMotion();
            speed = 150;
            //movement = false;
            //speed = 131f;
        }
        if (other.gameObject.CompareTag("Slide"))
        {
            ChangeAnimationState(PLAYER_SLIDE);
            Time.timeScale = 45 * Time.fixedDeltaTime;
            //  timeManager.DoSlowMotion();
            speed = 140;
            //movement = false;
            //speed = 131f;
        }
        if (other.gameObject.CompareTag("Dive"))
        {
            ChangeAnimationState(PLAYER_DIVE);
            Time.timeScale = 45 * Time.fixedDeltaTime;
            //  timeManager.DoSlowMotion();
            speed = 140;
            //movement = false;
            //speed = 131f;
        }
        if (other.gameObject.CompareTag("Cover"))
        {
            ChangeAnimationState(PLAYER_COVER);
            Time.timeScale = 45 * Time.fixedDeltaTime;
            //  timeManager.DoSlowMotion();
            speed = 100;
            jumpForce = 1550;
            ShieldEnable.SetActive(true);
            //movement = false;
            //speed = 131f;
        }
        if (other.gameObject.CompareTag("Stop"))
        {
            speed = 0;
            jumpForce = 0;
            cam.transform.DORotate(new Vector3(30, 0, 0), 2f);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 55, 40f * Time.deltaTime);
            ChangeAnimationState(PLAYER_DEFEAT);
            isvictory = false;
            isFinal = false;
            movement = false;
            //enemyEnd.enabled = true;
            //ExplosiveBarrel.instance.GetComponent<SphereCollider>().isTrigger = false;
            // Enemy.instance.anim.Play("Punch");

            uimanagr.instance.lost_panel();
        }
        if (other.gameObject.CompareTag("YellowColor"))
        {
            skinnedMesh.material = YellowSkin;
            skinnedMesh1.material = YellowSkin;
            //StartCoroutine(YellowDisable());
        }
        if (other.gameObject.CompareTag("RedColor"))
        {
            skinnedMesh.material = RedSkin;
            skinnedMesh1.material = RedSkin;
            //StartCoroutine(RedDisable());
        }
        if (other.gameObject.CompareTag("RandomColor"))
        {
            r = Random.Range(1, 3);
            print(r);
            if (r == 1)
            {
                skinnedMesh.material = YellowSkin;
                skinnedMesh1.material = YellowSkin;
                for (int i = 0; i < YellowEnable.Count; i++)
                {
                    YellowEnable[i].SetActive(true);
                }
                YellowParticle.SetActive(true);
                MainParticle.SetActive(false);
            }
            if (r == 2)
            {
                skinnedMesh.material = RedSkin;
                skinnedMesh1.material = RedSkin;
                for (int i = 0; i < RedEnable.Count; i++)
                {
                    RedEnable[i].SetActive(true);
                }
                RedParticle.SetActive(true);
                MainParticle.SetActive(false);
            }
        }

        if (other.gameObject.CompareTag("DisableColor"))
        {
            for (int i = 0; i < YellowEnable.Count; i++)
            {
                YellowEnable[i].SetActive(false);
            }
            for (int i = 0; i < RedEnable.Count; i++)
            {
                RedEnable[i].SetActive(false);
            }
        }

        if (other.gameObject.CompareTag("RedEnable"))
        {
            cam.transform.DORotate(new Vector3(30, 0, 0), 2f);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 70, 40f * Time.deltaTime);
            dummyplayer();
            AudioManager.instance.Play("Death");
            Vibration.VibratePop();
            uimanagr.instance.lost_panel();
            CameraEffects.ShakeOnce();
            ChangeAnimationState(PLAYER_JUMP_DEATH);
            speed = 0;
            jumpForce = 0;
            //    StartCoroutine(YellowDisable());
        }
        if (other.gameObject.CompareTag("YellowEnable"))
        {
            cam.transform.DORotate(new Vector3(30, 0, 0), 2f);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 70, 40f * Time.deltaTime);
            dummyplayer();
            AudioManager.instance.Play("Death");
            Vibration.VibratePop();
            uimanagr.instance.lost_panel();
            CameraEffects.ShakeOnce();
            ChangeAnimationState(PLAYER_JUMP_DEATH);
            speed = 0;
            jumpForce = 0;
            // StartCoroutine(RedDisable());
        }


    }

    public void fighttouch()
    {
        if (isControl == true)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.fingerId == 0)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        PlayerAnim.Play("Punch");
                        BossFight.instance.takedamage(1);
                        AudioManager.instance.Play("FinalBossHit");
                        PlayerPunch.Play(true);
                    }
                }
            }
        }
        if (isControlFall == true)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.fingerId == 0)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        PlayerAnim.Play("AirAttack");
                        BossFall.instance.takedamage(1);
                        AudioManager.instance.Play("FinalBossHit");
                        PlayerPunch.Play(true);
                    }
                }
            }
        }
    }

    public void takedamageplayer(int Damage_amount)
    {
        playerHealth -= Damage_amount;
        //   print(playerHealth);
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            PlayerAnim.Play("Death");
            isControl = false;
        }
    }


    public void disable()
    {
        for (int i = 0; i < trucks.Count; i++)
        {
            trucks[i].SetActive(false);
        }
    }


}
