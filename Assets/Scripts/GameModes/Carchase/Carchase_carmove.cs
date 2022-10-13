using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Carchase_carmove : MonoBehaviour
{
public   float zSpeed, xSpeed;
public Transform finalpoint,finalpoint2,hitmove,movepoint;
public Animator cam;
public ParticleSystem money;
public Transform tut;

void Start()
    {
        money.Play();
        tut.DOScale(Vector3.one, .3f).SetEase(Ease.OutElastic);
        DOVirtual.DelayedCall(3, () => tut.DOScale(Vector3.zero, .3f).SetEase(Ease.OutElastic));

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }
    private void PlayerMovement()
    {
        
        transform.position += Vector3.forward * (Time.deltaTime * zSpeed);
        if (!GetMouseHeld()) return;

        var position = transform.position;

        position += new Vector3(GetDeltaMousePos().x, 0, 0) * (Time.deltaTime * xSpeed);
        var x = Mathf.Clamp(position.x, -4.35f, 4.3f);
        position = new Vector3(x, position.y, position.z);
        transform.position = position;
    }
    private Vector2 GetDeltaMousePos() => InputExtensions.GetInputDelta();
    private bool GetMouseHeld() => InputExtensions.GetFingerHeld();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bots"))
        {
            zSpeed = 0;
            transform.DOMove(hitmove.position, 1f);
            transform.DORotate(new Vector3(0, 30f, 0), 1f);
            money.Stop();
            GameEvents.InvokeGameLose(-1);
            if(AudioManager.instance)
                AudioManager.instance.Pause("Carstart");
            AudioManager.instance.Play("drift"); 
            AudioManager.instance.Play("car");
        }

        if (other.gameObject.CompareTag("Endpoint"))
        {
          
            zSpeed = 0;
            Carsequence();
            money.Stop();
            GameEvents.InvokeGameWin();
            if(AudioManager.instance)
                AudioManager.instance.Pause("Carstart");
            AudioManager.instance.Play("drift");
        }

        if (other.gameObject.CompareTag("Finish"))
        {
            cam.SetTrigger("finalpoint");
        }
    }

    public void Carsequence()
    {
        var seq = DOTween.Sequence();
       
        seq.Append(transform.DORotate(new Vector3(0, 90, -20), .1f));
      seq.Append(  transform.DOMove(finalpoint.position, 2f).SetEase(Ease.Linear));
      seq.Append(   transform.DOMove(finalpoint2.position, 1f).SetEase(Ease.Linear));
      seq.Append(transform.DORotate(new Vector3(0, 38, 0), .1f));
   
    }

   
}
