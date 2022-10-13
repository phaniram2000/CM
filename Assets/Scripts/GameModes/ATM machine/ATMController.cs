using System;
using UnityEngine;

public class ATMController : MonoBehaviour
{
    [SerializeField] private ParticleSystem money;

    private void OnEnable()
    {
        ATMEvents.WithDrawlButtonPressed += OnWithDrawl;
    }

    private void OnDisable()
    {
        ATMEvents.WithDrawlButtonPressed -= OnWithDrawl;
    }

    
    private void Start()
    {
        money.gameObject.SetActive(false);
    }
    
    private void OnWithDrawl()
    {
        money.gameObject.SetActive(true);
    }
}
