using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using DG.Tweening;

public class SwipeControl : MonoBehaviour
{
    public Animator Anime;
    public GameObject ShadowPlayer,ShadowCanvas,deadeffect,hiteffect,effectPos;
    public PlayerRigid[] PlayerRigids;
    public GameObject[] ShadowPOs;
    public List<GameObject> Lasers;
    public float StartPosValue,ObstacleSpeed,FeverTimeObstacleValue;
    float oldValue;
    [Range(0,1)]
    public float Va;
    int animationStateVal;
    public float Distance;
    public int IncreaseValue;

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
        deadeffect.SetActive(false);
        rb = GetComponent<Rigidbody>();
        IncreaseValue = 0;
        Va = Mathf.Clamp01(StartPosValue);
        ShadowCanvas.SetActive(true);
        Anime.enabled = true;
        animationStateVal = Animator.StringToHash("AnimationFrame");
        for(int i = 0; i < PlayerRigids.Length; i++)
        {
            PlayerRigids[i].GetComponent<Rigidbody>().isKinematic = true;
        }
        ShadowCanvas.transform.SetParent(ShadowPOs[0].transform);
        for(int i = 1; i < Lasers.Count; i++)
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
    float temp;

    // Update is called once per frame
    void Update()
    {
        Anime.SetFloat(animationStateVal, Va);

        Distance = transform.position.z - ShadowPlayer.transform.position.z;

        if (Distance >= -0.001f)
        {
            IncreaseValue += 1;
            if (IncreaseValue < ShadowPOs.Length)
            {
                if (Lasers[IncreaseValue].CompareTag("Lasers")|| Lasers[IncreaseValue].CompareTag("Car"))
                {
                    ShadowCanvas.SetActive(true);
                    ShadowCanvas.transform.position = new Vector3(0f, ShadowCanvas.transform.position.y, ShadowPOs[IncreaseValue].transform.position.z);
                    ShadowCanvas.transform.SetParent(ShadowPOs[IncreaseValue].transform);
                    Lasers[IncreaseValue].SetActive(true);
                    Lasers[IncreaseValue].GetComponent<SplineFollower>().followSpeed = ObstacleSpeed;
                    if (Lasers[IncreaseValue].CompareTag("Lasers"))
                    {
                        StartCoroutine(PassingEffect(0f));
                        StartCoroutine(Waitdisableoldlaser());
                    }
                    else if (Lasers[IncreaseValue].CompareTag("Car"))
                    {
                        StartCoroutine(PassingEffect(0f));
                        StartCoroutine(Waitdisableoldlaser(1.5f));
                    }
                   
                    if (isFeverMOde == false)
                    {
                        flexRunGM.FeverBg.transform.gameObject.SetActive(true);
                        flexRunGM.FeverBg.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.6f, 0, 1);
                        flexRunGM.FeverBar.DOFillAmount(flexRunGM.FeverBar.fillAmount + flexRunGM.FeverIncreaseValue, 0.2f);
                    }
           
                }
                else if (!Lasers[IncreaseValue].CompareTag("Lasers")) 
                {
                    if (isFeverMOde == false)
                    {
                        ShadowCanvas.SetActive(false);
                        ShadowCanvas.transform.SetParent(ShadowPOs[IncreaseValue].transform);
                        var position = ShadowCanvas.transform.position;
                        position = new Vector3(position.x, position.y, ShadowPOs[IncreaseValue].transform.position.z);
                        ShadowCanvas.transform.position = position;
                        Lasers[IncreaseValue].SetActive(true);
                        StartCoroutine(PassingEffect(0.3f));
                        StartCoroutine(Waitdisableoldlaser());
                        flexRunGM.FeverBg.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.6f, 0, 1);
                        flexRunGM.FeverBar.DOFillAmount(flexRunGM.FeverBar.fillAmount + flexRunGM.FeverIncreaseValue, 0.2f);
                    }
                    else if(isFeverMOde==true)
                    {
                        ShadowCanvas.SetActive(false);
                        ShadowCanvas.transform.SetParent(ShadowPOs[IncreaseValue].transform);
                        var position = ShadowCanvas.transform.position;
                        position = new Vector3(position.x, position.y, ShadowPOs[IncreaseValue].transform.position.z);
                        ShadowCanvas.transform.position = position;
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
            print("TextParticleCalling");
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
            temp = Va;
            //Debug.Log(temp);
        }
        if (flexRunGM.isFeverBarFull)
        {
            isFeverMOde = true;
            ObstacleSpeed = FeverTimeObstacleValue;
            flexRunGM.SpeedEffect.Play();
        }
        else if (flexRunGM.feverwatcher == true)
        {
            ObstacleSpeed = oldValue;
            //isFeverMOde = false;
        }
        for (int i = 0; i < PlayerRigids.Length; i++)
        {
            if (PlayerRigids[i].isGothit == true&&isFeverMOde==false)
            {
                KillPlayer();
                flexRunGM.isplayerDead = true;
                if (v == 1)
                {
                    Instantiate(flexRunGM.hiteffect, effectPos.transform.position, effectPos.transform.rotation);
                    if (AudioManager.instance != null)
                    {
                        AudioManager.instance.Play("Shock");
                    }
                    v += 1;
                }
            }
            else if(PlayerRigids[i].isGothit == true && isFeverMOde == true)
            {
                print("Laser__AttackReverse");
            }
            else if (PlayerRigids[i].isGotShot == true&&isFeverMOde==false)
            {
                KillPlayer();
                flexRunGM.isplayerDead = true;
                if (v == 1)
                {
                    Instantiate(flexRunGM.ShotDeadEffect, effectPos.transform.position, effectPos.transform.rotation);
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
                    Instantiate(flexRunGM.BangEffect, effectPos.transform.position, effectPos.transform.rotation);
                    v += 1;
                }
            }
            else if (PlayerRigids[i].isGotSlamed == true && isFeverMOde == true)
            {
                print("Slam__AttackReverse");
                PlayerRigids[i].isGotSlamed = false;
            }
            else if (PlayerRigids[i].isdiamond == true)
            {
                //particle effect
                PlayerRigids[i].isdiamond = false;
            }
        }
    }

    IEnumerator Waitdisableoldlaser(float time =0.8f)
    {
        yield return new WaitForSeconds(time);
        Lasers[IncreaseValue - 1].SetActive(false);
    }
    IEnumerator PassingEffect(float time)
    {
        yield return new WaitForSeconds(time);
        if (flexRunGM.isplayerDead == false)
        {
            Instantiate(flexRunGM.StarEffect, flexRunGM.ExternalEffectPOs.transform.position, flexRunGM.ExternalEffectPOs.transform.rotation);
            Vibration.Vibrate(20);
            if (flexRunGM.isFeverBarFull==false)
            {
                flexRunGM.PerfectEFfect[IncreaseValue - 1].Play();
            }
        }
    }
    float j = 1;
    public void KillPlayer()
    {
        Anime.enabled = false;
        print("Vibrate");
        if (j == 1)
        {
            Vibration.Vibrate(20);
            j += 1;
        }
        ShadowCanvas.SetActive(false);
        deadeffect.SetActive(true);
        for (int i = 0; i < PlayerRigids.Length; i++)
        {
            PlayerRigids[i].GetComponent<Rigidbody>().isKinematic = false;
        }
        for(int i = 0; i < Lasers.Count; i++)
        {
            if (Lasers[i].CompareTag("Lasers"))
            {
                Lasers[i].GetComponent<SplineFollower>().follow = false;
            }
        }

    }
    private void Movement()
    {
        Va = Va + (GetInput()) *Time.deltaTime*speed;
        Va = Mathf.Clamp(Va, 0, 1);
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
