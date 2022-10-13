using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class golddigger : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator car,boy;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void dooropen()
    {
        car.SetTrigger("Open");
        if(AudioManager.instance)
            AudioManager.instance.Play("caropen");
    }

    public void doorclose()
    {
        car.SetTrigger("Close");
        DOVirtual.DelayedCall(1.8f, () =>
        {
            if (AudioManager.instance)
                AudioManager.instance.Play("carclose");
        });

    }

    public void kick()
    {
        boy.SetTrigger("Kick");
        boy.transform.DORotate(new Vector3(0, -131.749f, 0), .3f);
    }       
    
}
