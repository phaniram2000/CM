using System;
using DG.Tweening;
using UnityEngine;
using FIMSpace.Jiggling;
using UnityEngine.Serialization;

public class knifeMOve : MonoBehaviour
{
    public GameObject Aim;
    public float speed;
    public Tween StopMove;
    public GameObject HitPartical;
    [FormerlySerializedAs("EndCubes")] public Vector3 endCubes = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        StopMove = transform.DOLocalMove(endCubes - new Vector3(-1f, 0, 0.5f), 0.1f);
        // transform.DOLocalMoveX(-.3F, 0.25F).SetEase(Ease.Linear);
        // transform.DOLocalMove(new Vector3(-0.479999989f,-2.81999993f,4.44000006f), 0.25F).SetEase(Ease.Linear);
        // transform.DOLocalRotate(new Vector3(90f,0f,0f), 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.right * speed * Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Cube"))
        {
            speed = 0;
            gameObject.tag = "knife";
            Aim.SetActive(true);
            Vibration.Vibrate(20);
            HitPartical.SetActive(true);
            Destroy(GetComponent<Rigidbody>());
        }
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("knifeDie"))
    //     {
    //         print("DieKnife");
    //         if(StopMove.IsActive())
    //             StopMove.Kill(); 
    //         gameObject.GetComponent<Rigidbody>().AddForce(150f,500f,150f,ForceMode.Force);
    //     }
    // }
}