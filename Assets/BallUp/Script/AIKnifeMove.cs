using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
            
public class AIKnifeMove : MonoBehaviour
{
    
    public GameObject AiPartical;
    [FormerlySerializedAs("EndCubes")] public Vector3 endCubes = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalMove(endCubes + new Vector3(-0.6f+BallUpUIManager.instance.AiKnifeOffsetx, 0, 0.4f+BallUpUIManager.instance.AiKnifeOffsetz), 0.1f);
        AiPartical.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
