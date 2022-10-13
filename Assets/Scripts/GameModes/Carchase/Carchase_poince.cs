using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Carchase_poince : MonoBehaviour
{
    public float speed,Rotation;
    public GameObject car;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.forward * (Time.deltaTime * speed);
        if (car.GetComponent<Carchase_carmove>().zSpeed == 0)
        {
            speed = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Endpoint"))
        {
            speed = 0;
            transform.DORotate(new Vector3(0, Rotation, 0),1);
        }
    }
}
