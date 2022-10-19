using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using StylizedWater2;
using Unity.Mathematics;

public class Obsticle : MonoBehaviour
{
    public GameObject knifeHolder,AiHolder;
    public GameObject OriginalKnife, DieKnife;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Deathknife") )
        {
            //collision.gameObject.tag = "Untagged";
            print("knifeDie");
            other.gameObject.SetActive(false);
            Instantiate(DieKnife, transform.position, quaternion.identity);
            DieKnife.GetComponent<Rigidbody>().AddForce(15f,15f,15f, ForceMode.Impulse);
            DieKnife.GetComponentInChildren<Rigidbody>().AddForce(8f,8f,8f,ForceMode.Impulse);
            AudioManager.instance.Play("knifeDie");
            knifeHolder.GetComponent<Knife>().enabled = false;
            AiHolder.GetComponent<AIKnife>().enabled = false;
            StartCoroutine(LosePanel());
            //collision.transform.DOKill();
            //collision.transform.GetComponent<knifeMOve>().DOKill();
            //collision.gameObject.GetComponent<knifeMOve>().enabled = false;
            //collision.gameObject.AddComponent<Rigidbody>();
            //DieKnife.GetComponent<Rigidbody>().AddForce(150f,500f,150f,ForceMode.Force);
            //knifeHolder.GetComponent<Knife>().enabled = false;
        }

        if (other.gameObject.CompareTag("knife"))
        {
            DOTween.Kill(gameObject);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    { //print("knifeDie");
        // if (collision.gameObject.CompareTag("Deathknife") )
        // {
        //     //collision.gameObject.tag = "Untagged";
        //     print("knifeDie");
        //     collision.gameObject.SetActive(false);
        //     Instantiate(DieKnife, transform.position, quaternion.identity);
        //     knifeHolder.GetComponent<Knife>().enabled = false;
        //     AiHolder.GetComponent<AIKnife>().enabled = false;
        //     StartCoroutine(LosePanel());
        //     //collision.transform.DOKill();
        //     //collision.transform.GetComponent<knifeMOve>().DOKill();
        //     //collision.gameObject.GetComponent<knifeMOve>().enabled = false;
        //     //collision.gameObject.AddComponent<Rigidbody>();
        //     //DieKnife.GetComponent<Rigidbody>().AddForce(150f,500f,150f,ForceMode.Force);
        //     //knifeHolder.GetComponent<Knife>().enabled = false;
        // }
        //
        // if (collision.gameObject.CompareTag("knife"))
        // {
        //     DOTween.Kill(gameObject);
        //     gameObject.GetComponent<Rigidbody>().isKinematic = true;
        // }
    }

    IEnumerator LosePanel()
    {
        yield return new WaitForSeconds(1f);
        AudioManager.instance.Play("Lose");
        BallUpUIManager.instance.levelNum.SetActive(false);
        BallUpUIManager.instance.LosePanel.SetActive(true);
    }
}
