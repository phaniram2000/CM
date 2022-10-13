using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Y_R_N_Pranker : MonoBehaviour
{
    public static event Action Onbuttons,Onfood,Onhands;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Buttons()
    {
      Onbuttons.Invoke();  
    }
    public void food()
    {
        Onfood.Invoke();  
    } 
    public void Onhandsdown()
    {
        Onhands.Invoke();  
    }
}
