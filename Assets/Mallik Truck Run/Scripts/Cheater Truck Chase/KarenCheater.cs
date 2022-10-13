using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class KarenCheater : MonoBehaviour
{
    public static KarenCheater instance;
    public List<Rigidbody> bodyPartsRb;
    public List<Collider> bodyPartsCollider;
    
    private Animator _animator;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator StandOnBike()
    {
        yield return new WaitForSeconds(1);
        _animator.SetTrigger("stand");
        transform.DOLocalMoveY(0.0037f, 0.35f);
    }
    
    public IEnumerator JumpToTruck()
    {
        //_animator.SetTrigger("jump");
        _animator.SetTrigger("jump");
        TruckMovement.instance.truckRb.isKinematic = true;
        transform.DOScale(Vector3.zero, 1.5f).SetEase(Ease.Flash);
        transform.DOMove(TruckMovement.instance.jumpPos.position, 0.65f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            TruckMovement.instance.canControl = true;
            TruckMovement.instance.speed = GameManagerTruck.instance.truckSpeed;
            TruckMovement.instance.TruckAttacked();
        });
        transform.DORotate(Vector3.forward * 60, 0.65f);
            yield return new WaitForSeconds(1.5f);
            GameObject barMeter = TruckMovement.instance.barMeter;
        barMeter.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
        {
            barMeter.SetActive(false);
        });
    }

    public void FallDown()
    {
        Transform bike = transform.root;
        for (int i = 0; i < bodyPartsRb.Count; i++)
        {
            bodyPartsRb[i].isKinematic = false;
        }

        for (int i = 0; i < bodyPartsCollider.Count; i++)
        {
            bodyPartsCollider[i].enabled = true;
        }
        _animator.enabled = false;
        transform.parent = null;
        Rigidbody bikeRb = bike.GetComponent<Rigidbody>(); 
        bikeRb.isKinematic = false;
        bike.GetComponent<Collider>().enabled = true;
        
        GameManagerTruck.instance.gameOver = true;
        VirtualCameraManager.instance.vCamFollower.Follow = null;
        //SoundsManager.instance.BGMusicSource.enabled = false;
        TruckMovement.instance.speed = 0;
        GameEvents.InvokeGameLose(-1);
        //SoundsManager.instance.PlayOnce(SoundsManager.instance.fail);
        EventHandler.instance.GameOver();
        Vibration.Vibrate(27);
    }
}
