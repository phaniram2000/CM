using DG.Tweening;
using UnityEngine;

public class carchase_Cars : MonoBehaviour
{
public float speed;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalMove(Vector3.forward,  30);
    }

    // Update is called once per frame
    void Update()
    {
     //transform.position += Vector3.forward * (Time.deltaTime * speed);
        
    }
}
