using System;
using System.Collections.Generic;
using UnityEngine;

public class Cartheft_Car : MonoBehaviour
{
    // Start is called before the first frame update
    public static event Action Ondoordidnotopen,OnDooropen;
    [Header("Glassbreak")] public List<Rigidbody> glassrbs;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Ondooropen()
    {
        Ondoordidnotopen?.Invoke();
        for (int i = 0; i < glassrbs.Count; i++)
        {
            glassrbs[i].isKinematic= false;
            glassrbs[i].AddForce(Vector3.left*10,ForceMode.Force);
        }
        if(AudioManager.instance)
            AudioManager.instance.Play("Glass");
        
    }
    public void win()
    {
        OnDooropen?.Invoke();
       
        
    }

    public void audio()
    {
        if(AudioManager.instance)
            AudioManager.instance.Play("Open");
            AudioManager.instance.Pause("NailPuckier");
    }
    public void audio2()
    {
        if(AudioManager.instance)
            AudioManager.instance.Play("NailPuckier");
    }
}
