using UnityEngine;

public class bullet : MonoBehaviour
{
    public bool isplayerhit, isplayerescaped;
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
            isplayerhit = true;
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            isplayerescaped = true;
        }
    }
}
