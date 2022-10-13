using System.Collections.Generic;
using UnityEngine;

public class StealIteamsScanner : MonoBehaviour
{
    public static StealIteamsScanner instance;
    
    public List<GameObject> UnderScanner;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("pickups") && !UnderScanner.Contains(other.gameObject))
        {
            UnderScanner.Add((other.gameObject));
        }
        /*if (!UnderScanner.Contains(other.gameObject))
        {
            UnderScanner.Add((other.gameObject));
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        if (UnderScanner.Contains(other.gameObject))
        {
            UnderScanner.Remove((other.gameObject));
        }
    }
}
