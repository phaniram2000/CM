using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailParticle : MonoBehaviour
{
    public ParticleSystem effect;
    public GameObject Disable;
    public GameObject Enable;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
           // ParticleEffects();
            Disable.SetActive(false);
            Enable.SetActive(true);
        }
        
    }
    void ParticleEffects()
    {
      effect.Play(true);     
    }
   
    
}
