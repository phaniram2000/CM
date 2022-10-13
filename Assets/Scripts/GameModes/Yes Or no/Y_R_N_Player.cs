using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_R_N_Player : MonoBehaviour
{
    public static event Action Onrightup;

    public static event Action Onleftup,Onjar,Onvomit; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jarinhand()
    {
        Onjar.Invoke();
    }
    public void rightup()
    {
     Onrightup.Invoke();   
    }

    public void leftup()
    {
        Onleftup.Invoke();
    } public void _vomit()
    {
        Onvomit.Invoke();
    }
    
}
