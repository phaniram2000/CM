using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using DG.Tweening;

public class SwipeControlsFortwo : MonoBehaviour
{
    public float speedmultipiler = 2f;
    public Animator Anime1,Anime2;
    public GameObject ShadowPlayer, ShadowCanvas, deadeffect1,deadeffect2, hiteffect, effectPos1,effectPos2;
    public PlayerRigid[] PlayerRigids;
    public GameObject[] ShadowPOs;
    public List<GameObject> Lasers;
    public float StartPosValue1,StartPosValue2,MaxSliderValue, ObstacleSpeed, FeverTimeObstacleValue;
    float oldValue;
    [Range(0, 1)]
    public float Va1,Va2;
    int animationStateVal;
    public float Distance;
    int IncreaseValue;

    public bool isFeverMOde = false;
    Rigidbody rb;
    [Header("/////SetSpeed = = 0.08f////for mobile build_______3.5 for editor ")]
    float speed;
    float v = 1;
    private FlexRun_GameManager flexRunGM;
    private void OnEnable()
    {
        GameEvents.TapToPlay += StartGame;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= StartGame;
    }
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        speed = 3f;
#elif !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
        speed=0.065f;
#endif
        deadeffect1.SetActive(false);
        deadeffect2.SetActive(false);
        rb = GetComponent<Rigidbody>();
        IncreaseValue = 0;
        Va1 = Mathf.Clamp01(StartPosValue1);
        Va2 = Mathf.Clamp01(StartPosValue2);
        ShadowCanvas.SetActive(true);
        Anime1.enabled = true;
        Anime2.enabled = true;
        animationStateVal = Animator.StringToHash("AnimationFrame");
        for (int i = 0; i < PlayerRigids.Length; i++)
        {
            PlayerRigids[i].GetComponent<Rigidbody>().isKinematic = true;
        }
        ShadowCanvas.transform.SetParent(ShadowPOs[0].transform);
        for (int i = 1; i < Lasers.Count; i++)
        {
            if (Lasers[i].CompareTag("Lasers"))
            {
                Lasers[i].SetActive(false);
                Lasers[i].GetComponent<SplineFollower>().followSpeed = 0;
            }
            else
            {
                Lasers[i].SetActive(false);
            }
        }
        oldValue = ObstacleSpeed;
        isFeverMOde = false;
        flexRunGM = FlexRun_GameManager.Instance;
    }
    float temp1,temp2;


    // Update is called once per frame
    void Update()
    {
        Anime1.SetFloat(animationStateVal, Va1);
        Anime2.SetFloat(animationStateVal, Va2);

        Distance = transform.position.z - ShadowPlayer.transform.position.z;

        if (Distance >= -0.001f)
        {
            IncreaseValue += 1;
            if (IncreaseValue < ShadowPOs.Length)
            {
                if (Lasers[IncreaseValue].CompareTag("Lasers"))
                {
                    ShadowCanvas.SetActive(true);
                    ShadowCanvas.transform.position = new Vector3(0f, ShadowCanvas.transform.position.y, ShadowPOs[IncreaseValue].transform.position.z);
                    ShadowCanvas.transform.SetParent(ShadowPOs[IncreaseValue].transform);
                    Lasers[IncreaseValue].SetActive(true);
                    Lasers[IncreaseValue].GetComponent<SplineFollower>().followSpeed = ObstacleSpeed;
                    StartCoroutine(PassingEffect(0f));
                    StartCoroutine(Waitdisableoldlaser());
                    if (isFeverMOde == false)
                    {
                        flexRunGM.FeverBg.transform.gameObject.SetActive(true);
                        flexRunGM.FeverBg.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.6f, 0, 1);
                        flexRunGM.FeverBar.DOFillAmount(flexRunGM.FeverBar.fillAmount + flexRunGM.FeverIncreaseValue, 0.2f);
                    }
                }
                else if (Lasers[IncreaseValue].tag != "Lasers")
                {
                    if (isFeverMOde == false)
                    {
                        ShadowCanvas.SetActive(false);
                        ShadowCanvas.transform.SetParent(ShadowPOs[IncreaseValue].transform);
                        ShadowCanvas.transform.position = new Vector3(ShadowCanvas.transform.position.x, ShadowCanvas.transform.position.y, ShadowPOs[IncreaseValue].transform.position.z);
                        Lasers[IncreaseValue].SetActive(true);
                        StartCoroutine(PassingEffect(0.5f));
                        StartCoroutine(Waitdisableoldlaser());
                        flexRunGM.FeverBg.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.6f, 0, 1);
                        flexRunGM.FeverBar.DOFillAmount(flexRunGM.FeverBar.fillAmount + flexRunGM.FeverIncreaseValue, 0.2f);
                    }
                    else if (isFeverMOde == true)
                    {
                        ShadowCanvas.SetActive(false);
                        ShadowCanvas.transform.SetParent(ShadowPOs[IncreaseValue].transform);
                        ShadowCanvas.transform.position = new Vector3(ShadowCanvas.transform.position.x, ShadowCanvas.transform.position.y, ShadowPOs[IncreaseValue].transform.position.z);
                        Lasers[IncreaseValue].SetActive(true);
                        StartCoroutine(PassingEffect(0.3f));
                        StartCoroutine(Waitdisableoldlaser());
                        flexRunGM.PerfectEFfect[IncreaseValue - 1].Play();
                    }
                }

            }
            else
            {
                ShadowCanvas.SetActive(false);
            }
        }
        if (Lasers.Count == IncreaseValue)
        {
            //print("TextParticleCalling");
            flexRunGM.PerfectEFfect[IncreaseValue - 1].Play();
            flexRunGM.FeverBg.gameObject.SetActive(false);
            //flexRunGM.FeverBar.DOFillAmount(flexRunGM.FeverBar.fillAmount + 0.2f, 1f);
        }
        if (IncreaseValue > Lasers.Count)
        {
            flexRunGM.islasershowended = true;
        }
        if (Input.GetMouseButton(0))
        {
            Movement();
        }
        if (Input.GetMouseButtonUp(0))
        {
            temp1 = Va1;
            temp2= Va2;
            Debug.Log(temp1);
            Debug.Log(temp2);
        }
        if (flexRunGM.isFeverBarFull)
        {
            isFeverMOde = true;
            ObstacleSpeed = FeverTimeObstacleValue;
            flexRunGM.SpeedEffect.Play();
        }
        else if (flexRunGM.feverwatcher == true)
        {
            isFeverMOde = false;
            ObstacleSpeed = oldValue;
        }
        for (int i = 0; i < PlayerRigids.Length; i++)
        {
            if (PlayerRigids[i].isGothit == true&&isFeverMOde==false)
            {
                KillPlayer();
                flexRunGM.isplayerDead = true;
                if (v == 1)
                {
                    Instantiate(flexRunGM.hiteffect, effectPos1.transform.position, effectPos1.transform.rotation);
                    Instantiate(flexRunGM.hiteffect, effectPos2.transform.position, effectPos2.transform.rotation);
                    if (AudioManager.instance != null)
                    {
                        AudioManager.instance.Play("Shock");
                    }
                    v += 1;
                }
            }
            else if (PlayerRigids[i].isGothit == true && isFeverMOde == true)
            {
                print("Laser__AttackReverse");
            }
            else if (PlayerRigids[i].isGotShot == true&&isFeverMOde==false)
            {
                KillPlayer();
                flexRunGM.isplayerDead = true;
                if (v == 1)
                {
                    Instantiate(flexRunGM.ShotDeadEffect, effectPos1.transform.position, effectPos1.transform.rotation);
                    Instantiate(flexRunGM.ShotDeadEffect, effectPos2.transform.position, effectPos2.transform.rotation);
                    v += 1;
                }
            }
            else if (PlayerRigids[i].isGotShot == true && isFeverMOde == true)
            {
                print("Bullet__AttackReverse");
            }
            else if (PlayerRigids[i].isGotSlamed == true && isFeverMOde == false)
            {
                KillPlayer();
                flexRunGM.isplayerDead = true;
                if (v == 1)
                {
                    Instantiate(flexRunGM.BangEffect, effectPos1.transform.position, effectPos1.transform.rotation);
                    Instantiate(flexRunGM.BangEffect, effectPos2.transform.position, effectPos2.transform.rotation);
                    v += 1;
                }
            }
            else if (PlayerRigids[i].isGotSlamed == true && isFeverMOde == true)
            {
                print("Slam__AttackReverse");
                PlayerRigids[i].isGotSlamed = false;
            }
        }
    }
    
    IEnumerator Waitdisableoldlaser()
    {
        yield return new WaitForSeconds(1f);
        Lasers[IncreaseValue - 1].SetActive(false);
    }
    IEnumerator PassingEffect(float time)
    {
        yield return new WaitForSeconds(time);
        if (flexRunGM.isplayerDead == false)
        {
            Vibration.Vibrate(20);
            Instantiate(flexRunGM.StarEffect, flexRunGM.ExternalEffectPOs.transform.position, flexRunGM.ExternalEffectPOs.transform.rotation);
            if (flexRunGM.isFeverBarFull == false)
            {
                flexRunGM.PerfectEFfect[IncreaseValue - 1].Play();
            }
        }
    }
    float j = 1;
    public void KillPlayer()
    {
        Anime1.enabled = false;
        Anime2.enabled = false;
        if (j == 1)
        {
            Vibration.Vibrate(20);
            j += 1;
        }
        ShadowCanvas.SetActive(false);
        deadeffect1.SetActive(true);
        deadeffect2.SetActive(true);
        for (int i = 0; i < PlayerRigids.Length; i++)
        {
            PlayerRigids[i].GetComponent<Rigidbody>().isKinematic = false;
        }
        for (int i = 0; i < Lasers.Count; i++)
        {
            if (Lasers[i].CompareTag("Lasers"))
            {
                Lasers[i].GetComponent<SplineFollower>().follow = false;
            }
        }
    }
    private void Movement()
    {
        Va1 = Va1 + (-GetInput()) * Time.deltaTime * speed/speedmultipiler;
        Va1 = Mathf.Clamp(Va1, 0, MaxSliderValue);

        Va2 = Va2 + (-GetInput()) * Time.deltaTime * speed/speedmultipiler;
        Va2 = Mathf.Clamp(Va2, 0, MaxSliderValue);
    }

    private float GetInput()
    {
#if UNITY_EDITOR
        if (InputHeldDown())
            return Input.GetAxis("Mouse X");

        return 0;
#elif !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		return Input.GetTouch(0).deltaPosition.x;
#endif
    }
    private bool InputHeldDown()
    {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		return Input.touchCount != 0;
#endif


#if UNITY_EDITOR
        return Input.GetMouseButton(0);
#elif !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		return Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary;
#endif
    }
    public void StartGame()
    {
        Lasers[0].GetComponent<SplineFollower>().follow = true;
    }
}
