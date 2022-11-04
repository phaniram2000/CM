using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzeGames.Effects;
public class TruckMove : MonoBehaviour
{
    public float speed;
    public ParticleSystem BlastParticle;
    public AudioSource blast;

    public bool move;

    public static TruckMove instance;


    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        if(move == true)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        GameEvents.TapToPlay += taptoplay;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= taptoplay;
    }

    private void taptoplay()
    {
        move = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Shake"))
        {
           /// AudioManager.instance.Play("Explosion");
            BlastParticle.Play(true);
            // CameraEffects.ShakeOnce(0.5f, 10f);
        }
        if(other.gameObject.CompareTag("DummyWall"))
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            speed = 50;
        }
    }
}
