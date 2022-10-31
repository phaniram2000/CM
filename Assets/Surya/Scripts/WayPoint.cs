using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public Animator Waypoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Waypoint.SetTrigger("Win");
        }
    }
}
