using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummyWall : MonoBehaviour
{
    public bool iswalltouched = false;
    int v = 1;
    // Start is called before the first frame update
    void Start()
    {
        iswalltouched = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //print("BreakWall");
            iswalltouched = true;
            if (v == 1)
            {
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.Play("Glassbreak");
                }
                v += 1;
            }

        }
    }
}
