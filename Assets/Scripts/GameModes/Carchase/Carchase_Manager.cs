using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;

public class Carchase_Manager : MonoBehaviour
{
    public Transform Movetocar, door, Incar;
    public Animator Player, cam, car;
    public GameObject Botcars,policecars;
    public ParticleSystem money;
    
    private void OnEnable()
    {
        GameEvents.TapToPlay += Ontaptoplay;
    }
    private void OnDisable()
    {
        GameEvents.TapToPlay -= Ontaptoplay;
    }
    private void Ontaptoplay()
    {
        storysequence();
    }

    public void storysequence()
    {
        var seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
           
            Player.SetTrigger("Run");
        });
        seq.AppendCallback(() => cam.SetTrigger("Incar"));
        seq.Append(Player.transform.DOMove(Movetocar.position, 1).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            Player.SetTrigger("Jump"); 
            if(AudioManager.instance)
                AudioManager.instance.Play("Swipe");
        });
        seq.Append(Player.transform.DOMove(Incar.position, .5f).SetEase(Ease.Linear));
        seq.Append(Player.transform.DOScale(Vector3.zero, .2f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            money.Play();
            car.enabled = true;
            if(AudioManager.instance)
                AudioManager.instance.Play("Carstart");
                AudioManager.instance.Play("Police");
        });
        seq.Append(door.transform.DOLocalRotate(new Vector3(0, 0, 0), .5f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            cam.SetTrigger("Move");
          
        });
        seq.AppendInterval(.5f);
        seq.AppendCallback(() =>
        {
            car.transform.GetComponent<Carchase_carmove>().enabled = true;
            Botcars.SetActive(true);
            policecars.SetActive(true);
            car.enabled = false;
        });
        

    }
}