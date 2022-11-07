using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.AI;

public class CrowdController : MonoBehaviour
{
    public static CrowdController instance;
    public float followerGap;
    public Transform lastFollower;
    public List<GameObject> crowd;
    //public Transform playerLastPosition;
    //public TextMeshProUGUI crowdCountText;
    private int crowdCounter = 1;
    //public Transform dummyPosition;
    public List<Vector3> _crowdPositions;
    public GameObject playersGameObject;
    public GameObject man;
    [Header("Formation")]
    public bool playerformaion;
    public Transform right;
    public Transform left;
    public int rightindex = 0;
    public int leftindex = 1;
    public int PlayerCount;
    public TextMeshProUGUI PlayerText;
    public List<GameObject> DummyPlayer;
    Vector3 matric;
    Vector3 moveToPosition;
    public Transform playerpos;
    public ParticleSystem IncrementGlow;
    private void Awake()
    {
        instance = this;
        crowd = new List<GameObject>();
    }

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tail"))
        {
            CrowdFollow crowdFollow = other.GetComponent<CrowdFollow>();
            crowdFollow.enabled = true;
            if (lastFollower == null)
            {
                crowdFollow.charToFollow = this.transform;
            }
            else
            {
                crowdFollow.charToFollow = lastFollower;
            }
            other.GetComponent<Collider>().enabled = false;
            other.transform.tag = "Player";
            lastFollower = other.transform;
            crowd.Add(other.gameObject);
            crowdCounter += 1;
        }

        //if (other.gameObject.CompareTag("multiplierGate"))
        //{
        //    other.gameObject.tag = "Untagged";
        //    int numToSpawn = (other.GetComponent<MultiplierGate>().value - 1) * crowdCounter;
        //    SpawnCrowd(numToSpawn, other.gameObject);
        //    other.transform.DOScale(Vector3.zero, 2f);
        //   
        //}
        if (other.gameObject.CompareTag("additionGate"))
        {
            AudioManager.instance.Play("Increment");
            other.gameObject.tag = "Untagged";
            int numToAdd = other.GetComponent<AdditionGate>().value;
            SpawnCrowd(numToAdd, other.gameObject);
            Vibration.VibratePop();
            // other.transform.DOScale(Vector3.zero, 2f);
        }

        if (other.gameObject.CompareTag("subtractionGate"))
        {
            int numToSubtract = other.GetComponent<SubtractionGate>().value;
            //print("Number to kill= " + numToSubtract);
            other.GetComponent<Collider>().enabled = false;
            if (numToSubtract <= crowd.Count)
            {
                for (int i = 0; i < numToSubtract; i++)
                {
                    int lastIndex = crowd.Count - 1;
                    crowd[lastIndex].SetActive(false);
                    crowd.Remove(crowd[lastIndex]);
                    if (crowd.Count == 0)
                        lastFollower = transform;
                    else
                        lastFollower = crowd[lastIndex - 1].transform;
                    PlayerCount -= 1;
                    PlayerText.text = PlayerCount.ToString();
                }
                if (crowd.Count == 0)
                {
                    uimanagr.instance.lost_panel();
                    Time.timeScale = 45 * Time.deltaTime;
                    Player.instance.dummyplayer();
                }
               // lastFollower = crowd[crowd.Count - numToSubtract - 1].transform;
                
            }
        }
        if (other.gameObject.CompareTag("Formation") && playerformaion == false)
        {
            Player.instance.speed = 0;
            for (int i = 0; i < crowd.Count; i++)
            {
                crowd[rightindex].transform.DOMove(right.position, 1).OnComplete(() =>
                {
                    crowd[rightindex].transform.DOKill();

                });
                right.position = new Vector3(right.position.x + 3.5f, right.position.y, right.position.z + 2f);
                crowd[rightindex].gameObject.GetComponent<CrowdFollow>().enabled = false;
                rightindex++;
                playerformaion = true;
                //crowd[rightindex].SetActive(false);
                //crowd.Remove(crowd[i]);
            }
            for (int i = 1; i <= crowd.Count; i++)
            {
                crowd[leftindex].transform.DOMove(left.position, 1).OnComplete(() =>
                {
                    crowd[leftindex].transform.DOKill();

                });
                left.position = new Vector3(left.position.x + 3.5f, left.position.y, left.position.z);
                crowd[leftindex].gameObject.GetComponent<CrowdFollow>().enabled = false;
                leftindex++;
                playerformaion = true;
                //crowd[leftindex].SetActive(false);
                //crowd.Remove(crowd[i]);
            }
        }
        if(other.gameObject.CompareTag("RedEnable"))
        {
            for (int i = 0; i < crowd.Count; i++)
            {
                crowd[i].SetActive(false);
                dummyplayers();
                uimanagr.instance.lost_panel();
                // crowd.Remove(crowd[i]);
            }
        }
        if(other.gameObject.CompareTag("YellowEnable"))
        {
            for (int i = 0; i < crowd.Count; i++)
            {
                crowd[i].SetActive(false);
                dummyplayers();
                uimanagr.instance.lost_panel();
                // crowd.Remove(crowd[i]);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss") && playerformaion == false)
        {
            //Debug.Log("kicing");
            //for (int i = 0; i < crowd.Count; i++)
            //{
            //    crowd[rightindex].transform.DOMove(right.position, 1).OnComplete(() =>
            //     {
            //         crowd[rightindex].transform.DOKill();

            //     });
            //    right.position = new Vector3(right.position.x + 3.5f, right.position.y, right.position.z + 2f);
            //    crowd[rightindex].gameObject.GetComponent<CrowdFollow>().enabled = false;
            //    rightindex++;
            //    Debug.Log("rightindex-" + rightindex);
            //    playerformaion = true;
            //    crowd[rightindex].SetActive(false);
            //    crowd.Remove(crowd[i]);
            //}
            //for (int i = 1; i <= crowd.Count; i++)
            //{
            //    crowd[leftindex].transform.DOMove(left.position, 1).OnComplete(() =>
            //    {
            //        crowd[leftindex].transform.DOKill();

            //    });
            //    left.position = new Vector3(left.position.x + 3.5f, left.position.y, left.position.z);
            //    crowd[leftindex].gameObject.GetComponent<CrowdFollow>().enabled = false;
            //    leftindex++;
            //    Debug.Log("leftindex-" + leftindex);
            //    playerformaion = true;
            //    crowd[leftindex].SetActive(false);
            //    crowd.Remove(crowd[i]);
            //}
            for (int i = 0; i < crowd.Count; i++)
            {
                crowd[i].SetActive(false);
                // crowd.Remove(crowd[i]);
            }

        }
        if (collision.gameObject.CompareTag("Obstacles"))
        {
             dummyplayers();
           // dummyPlayerPar.Play(true);
            for (int i = 0; i < crowd.Count; i++)
            {
                crowd[i].SetActive(false);
                // crowd.Remove(crowd[i]);
            }
        }
      
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (crowd.Count > 0)
            {
                dummyplayers();
                for (int i = 0; i < crowd.Count; i++)
                {
                    crowd[i].SetActive(false);
                    // crowd.Remove(crowd[i]);
                }
            }
        }

    }
    public void dummyplayers()
    {
        for (int i = 0; i < DummyPlayer.Count; i++)
        {
            Vector3 playerpos = transform.position;
            DummyPlayer[i].transform.position = playerpos;
            DummyPlayer[i].SetActive(true);
        }
    }
    public void dummyplayers_()
    {
        for (int i = 0; i < DummyPlayer.Count; i++)
        {
            Vector3 playerpos = transform.position;
            DummyPlayer[i].transform.position = playerpos;
            StartCoroutine(EachDummyPlayerDelay());
            StartCoroutine(enableDummyPlayers());
        }
        IEnumerator EachDummyPlayerDelay()
        {
            for (int i = 0; i < DummyPlayer.Count; i++)
            {
                yield return new WaitForSeconds(0.015f);

                DummyPlayer[i].SetActive(true);
            }
        }
        IEnumerator enableDummyPlayers()
        {
            yield return new WaitForSeconds(1f);
            for(int i = 0; i < DummyPlayer.Count; i++)
            {
            DummyPlayer[i].SetActive(false);
            }
        }
    }

   
    void SpawnCrowd(int num, GameObject other)
    {
        for (int i = 0; i < num; i++)
        {
            Vector3 spawnPos;
            if (lastFollower == null) spawnPos = transform.position;
            else spawnPos = lastFollower.position;

            GameObject manObj = Instantiate(man, spawnPos, transform.rotation);
            CrowdFollow crowdFollow = manObj.GetComponent<CrowdFollow>();

            if (lastFollower == null)
            {
                crowdFollow.charToFollow = this.transform;
            }
            else
            {
                crowdFollow.charToFollow = lastFollower;
            }

            crowdFollow.enabled = true;
            crowdFollow.GetComponent<Animator>().SetTrigger("run");
            lastFollower = crowdFollow.gameObject.transform;
            crowd.Add(crowdFollow.gameObject);
            PlayerCount += 1;
            PlayerText.text = PlayerCount.ToString();
            IncrementGlow.Play();
        }
    }
   
}
