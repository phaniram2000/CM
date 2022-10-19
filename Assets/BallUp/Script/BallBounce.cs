using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using FIMSpace.Jiggling;
using UnityEngine.LowLevel;

public class BallBounce : MonoBehaviour
{
    //public MeshRenderer ballMesh;
    public GameObject Jump, SkinChange, jumpHips;
    public Animator anim;
    public GameObject Partical;
    public GameObject knife, MainCamera, FinalCam;
    private bool isDead;
    public Material EnemyDie;
    private static readonly int Idle = Animator.StringToHash("Idle");
    public GameObject WinEffect, Playertext;
    public GameObject AiPlayer, Aiknife;

    private bool _gameLose, _gameWon;

    // public LayerMask layermask;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(1))
        // {
        //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("knife") && !isDead)
        {
           // ballMesh.enabled = false;
            //Jump.SetActive(true);
            anim.SetTrigger("Jump");
            GetComponent<Rigidbody>().AddForce(0f, 10f, 0f, ForceMode.Impulse);
            collision.transform.GetChild(0).GetComponent<Animator>().SetTrigger(Idle);
            StartCoroutine(backToball());
        }

        if (collision.transform.CompareTag("Deathknife"))
        {
            anim.SetTrigger("Death");
            Partical.SetActive(true);
            isDead = true;
            //ballMesh.enabled = false;
            //Jump.SetActive(true);
           
           // print("Death");
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            collision.gameObject.GetComponent<Collider>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            knife.GetComponent<Knife>().enabled = false;
            transform.parent = (collision.transform);
            //transform.localPosition = new Vector3(0.84f,-1.2f,0.17f);
            transform.DOLocalMove(new Vector3(0.84f, -0.71f, 0.17f), 0.1f);
            //SkinChange.transform.GetComponent<SkinnedMeshRenderer>().material = EnemyDie;
            //GetComponent<Rigidbody>().AddForce(0f,0f,0f,ForceMode.Impulse);
             StartCoroutine(LosePanel());
        }

        if (collision.gameObject.CompareTag("Diveknife"))
        {
            if (gameObject.name == "AIPlayer")
            {
                Debug.Log("AI Won");
                gameObject.tag = "Respawn";
                _gameLose = true;
            }

            _gameWon = true;
            isDead = true;
            //StopCoroutine(backToball());
            //ballMesh.enabled = false;
           // Jump.SetActive(true);
            anim.SetTrigger("Dive");
            if (_gameLose)
            {
                DOVirtual.DelayedCall(1F, () => GameEvents.InvokeGameLose(-1));
                knife.GetComponent<Knife>().enabled = false;
            }

            if (_gameWon)
            {
                StartCoroutine(WinCam());
                if(Playertext)
                    Playertext.SetActive(false);
            }
            //jumpHips.GetComponent<Collider>().enabled = true;
            // gameObject.GetComponent<Collider>().enabled = false;
        }

         // if (((1<< collision.gameObject.layer)&layermask)!=0)
         // {
         //     StartCoroutine(LosePanel());
         // }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dollar"))
        {
            other.gameObject.SetActive(false);
        }
    }

    IEnumerator backToball()
    {
        yield return new WaitForSeconds(1.2f);
        if (isDead) yield break;
        anim.SetTrigger("Return");
        //Jump.SetActive(false);
       // ballMesh.enabled = true;
    }

    IEnumerator WinCam()
    {
        yield return new WaitForSeconds(1.5f);
        MainCamera.transform.DOMove(FinalCam.transform.position, 1f).SetEase(Ease.Linear);
        StartCoroutine(WinPartical());
       
    }

    IEnumerator WinPartical()
    {
        yield return new WaitForSeconds(1.2f);
        WinEffect.SetActive(true);
       // AudioManager.instance.Play("Win");
        if (Aiknife && AiPlayer)
        {
            Aiknife.GetComponent<AIKnife>().enabled = false;
            AiPlayer.GetComponent<Collider>().enabled = false;
        }

        //CoinEffects.instance.PlayCoinEffects(RectTransform coinIconRect, Vector3 startPos);

         StartCoroutine(WinPanel());
    }

    IEnumerator WinPanel()
    {
        yield return new WaitForSeconds(1f);
        GameEvents.InvokeGameWin();
        
     }

    IEnumerator LosePanel()
    {
        yield return new WaitForSeconds(0.1f);
        GameEvents.InvokeGameLose(-1);
    }
}