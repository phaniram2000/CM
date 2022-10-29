using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    public SplineFollower PlayerGo;
    public List<GameObject> DelPackages, ColPackage;
    public List<GameObject> MoneyStack;
    public float speed;
    public TrailRenderer tireMark;
    [SerializeField] private bool decreaseSpeed;
    public Animator Boxanim;
    public Animator Moneyanim, KarenAnim;
    public Transform FinalMoney;
    
    public ParticleSystem MoneyEffect;
    private bool isLC;
    public GameObject ShopPanel;
    public Transform DropPoint;
    public GameObject cars;

    public Rigidbody rb;
    private void OnEnable()
    {
        GameEvents.TapToPlay += taptoplay;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= taptoplay;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void taptoplay()
    {
        PlayerGo = GetComponent<SplineFollower>();
        PlayerGo.followSpeed = speed;
       
        
    }


    // Update is called once per frame
    void Update()
    {
     
        if(ShopPanel.activeInHierarchy|| EventSystem.current.currentSelectedGameObject|| rb.isKinematic)
            return;
        PlayerGo.followSpeed = speed;
        if (decreaseSpeed && speed > 0)
        {
            speed = speed <= 0 ? 0 : speed -= Time.deltaTime * 25f;
            tireMark.enabled = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
           cars.SetActive(true);
            speed = 22;
            Boxanim.SetTrigger("MoveBack");
            Moneyanim.SetTrigger("Moneymove");
            tireMark.enabled = false;
            decreaseSpeed = false;
            if(AudioManager.instance)
                AudioManager.instance.Play("Scouter");
        }

        if (Input.GetMouseButtonUp(0))
        {
           
            speed = 0;
            decreaseSpeed = true;
            PlayerGo.direction = Spline.Direction.Forward;
            if (!enemy) return;

            /*if (!EventSystem.current.currentSelectedGameObject && !isLC) 
                AudioManager.instance.Play("Brake");*/
          
        }

        if (Input.touchCount <= 0 )
        {
            if(AudioManager.instance)
                AudioManager.instance.Pause("Scouter");
        }
      //  if (!CheckURNear()) return;
    }

   

    public bool CheckURNear()
    {
        if (!decreaseSpeed || !enemy) return false;
        if (Vector3.Distance(transform.position, enemy.transform.position) <= 4.5f)
        {
            GameEvents.InvokeGameLose(-1);
            PlayerGo.enabled = false;
            rb.isKinematic = true;
            enemy = null;
            if(AudioManager.instance)
                AudioManager.instance.Pause("Scouter");
            return true;
        }

        return false;
    }

    public void MakePackagesFall()
    {
        for (int i = 0; i < DelPackages.Count; i++)
        {
            DelPackages[i].transform.parent = null;
            DelPackages[i].GetComponent<Rigidbody>().isKinematic = false;
            DelPackages[i].GetComponent<Collider>().isTrigger = false;
            //if (FallDownCharacters.Count > 0)
            //{
            //  FallDownCharacters[i].SetTrigger("Fall");
            //}
        }

        for (int i = 0; i < MoneyStack.Count; i++)
        {
            MoneyStack[i].transform.parent = null;
            MoneyStack[i].GetComponent<Rigidbody>().isKinematic = false;
            MoneyStack[i].GetComponent<Collider>().isTrigger = false;
        }
    }

    public int counter;
    public int addCounter = 0;
    public int GuardCount = 0;
    int delPackDeactivateCounter;
    [SerializeField] private GameObject enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cpackage"))
        {
            other.enabled = false;
            other.transform.DOJump(DropPoint.position, 5f, 1, 0.5f).SetEase(Ease.Linear);
            //  other.transform.parent = DropPoint;
            // DelPackages[counter >= DelPackages.Count ? DelPackages.Count : ++counter].SetActive(true);
            DelPackages[counter].SetActive(true);
            counter++;
            StartCoroutine(OffCpackage(other.gameObject));
            MoneyEffect.Play();
            if(AudioManager.instance)
                AudioManager.instance.Play("Button");
            Vibration.Vibrate(30);
            
        }

        if (other.gameObject.CompareTag("KarenGrab"))
        {
            KarenAnim.SetTrigger("Grab");
            enemy = other.GetComponentInParent<Animator>().gameObject;
            other.GetComponentInParent<Animator>().SetTrigger("Robbed");
           
        }

        if (other.gameObject.CompareTag("Finish"))
        {
            if(AudioManager.instance)
                AudioManager.instance.Pause("Scouter");
            CameraFollow.instance.offset = new Vector3(4.56f, 20, -20.77f);
            // for (int i = 0; i < DelPackages.Count; i++)
            //     DelPackages[i].SetActive(false);
            for (int i = 0; i < MoneyStack.Count; i++)
            {
                MoneyStack[addCounter].SetActive(true);
                addCounter++;
            }
        }

       
        if (other.gameObject.CompareTag("Drop2"))
        {
            print("triggered");
            other.GetComponent<Collider>().enabled = false;
            for (int i = 0; i < MoneyStack.Count; i++)
            {
                rb.isKinematic = true;
              //  DelPackages[delPackDeactivateCounter++].SetActive(false);
              //  Drop2[i].SetActive(true);
                StartCoroutine(delaystack());
                print("in loop");
                MoneyEffect.Play();
                decreaseSpeed = true;
                StartCoroutine(delayWin());
                isLC = true; 
                GameEvents.InvokeGameWin();
                if (AudioManager.instance)
                {
                    AudioManager.instance.Pause("Scouter");
                    AudioManager.instance.Play("Brake");
                }
            }
        }
    }

    IEnumerator OffCpackage(GameObject other)
    {
        yield return new WaitForSeconds(0.3f);
        other.SetActive(false);
        //ColPackage[i].gameObject.SetActive(false);
    }

    IEnumerator delayWin()
    {
        yield return new WaitForSeconds(1f);
        /* if(ISManager.instance)
         {
             ISManager.instance.ShowInterstitialAds();
         }
         if (GAScript.Instance)
         {
             GAScript.Instance.LevelCompleted("levelnumber");
         }*/
        //GameManger.instance.WinPanel.SetActive(true);

        GameEvents.InvokeGameWin();
    }

    IEnumerator delaystack()
    {
        for (int i = MoneyStack.Count - 1; i >= 0; i--)
        {
            MoneyStack[i].transform.DOMove(FinalMoney.position, 1f);
            MoneyStack[i].transform.DOScale(Vector3.zero, 1.5f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.2f);
            if(AudioManager.instance)
                AudioManager.instance.Play("Button");
            
        }
    }
}