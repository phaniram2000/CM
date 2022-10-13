using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAGDOL : MonoBehaviour
{
    public List<Rigidbody> RBS = new List<Rigidbody>();
    // Start is called before the first frame update
    private void Awake()
    {
        for (int i = 0; i < RBS.Count; i++)
        {
            RBS[i].GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void activeragdol() {
        for (int i = 0; i < RBS.Count; i++)
        {
            RBS[i].GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
