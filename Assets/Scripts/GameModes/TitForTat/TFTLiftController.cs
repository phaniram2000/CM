using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TFTLiftController : MonoBehaviour
{
    [SerializeField] private List<Transform> doorHolders;
    [SerializeField] private float liftDuration;
    [SerializeField] private Ease liftEase;
    private float initalZScale;

    private void Start()
    {
        if (doorHolders.Count < 1) return;

        initalZScale = doorHolders[0].transform.localScale.z;
    }


    private void OnEnable()
    {
        TFTGameEvents.OpenLiftDoors += OpenLiftDoors;
        TFTGameEvents.CloseLiftDoors += CloseLiftDoors;
    }

    private void OnDisable()
    {
        TFTGameEvents.OpenLiftDoors -= OpenLiftDoors;
        TFTGameEvents.CloseLiftDoors -= CloseLiftDoors;
    }


    private void OpenLiftDoors()
    {
      
        for (int i = 0; i < doorHolders.Count; i++)
        {
            doorHolders[i].transform.DOScaleZ(0, liftDuration).SetEase(liftEase);
        }
        
        TFTGameEvents.InvokeOnLiftOpenDoorsDone(liftDuration);
        
        if(AudioManager.instance)
            AudioManager.instance.Play("ElevatorDoor");
        
    }

    private void CloseLiftDoors()
    {
        for (int i = 0; i < doorHolders.Count; i++)
        {
            doorHolders[i].transform.DOScaleZ(initalZScale, liftDuration).SetEase(liftEase);
        }
        if(AudioManager.instance)
            AudioManager.instance.Play("ElevatorDoor");
    }
}
