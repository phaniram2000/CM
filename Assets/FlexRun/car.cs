using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Dreamteck.Splines;

public class car : MonoBehaviour
{
    SplineFollower CarSP;
    [SerializeField]
    List<GameObject> Wheels;

    [SerializeField]
    List<GameObject> Lights;


    GameObject Shadow;

    Rigidbody rb;
    Collider Col;
    Vector3 rot=new Vector3(10,0,0);
    bool isCarDestroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        isCarDestroyed = false;
        CarSP = GetComponent<SplineFollower>();
        CarSP.follow = true;

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        Col = GetComponent<Collider>();
        Col.isTrigger = true;

        foreach(Transform t in gameObject.GetComponentsInChildren<Transform>(true))
        {
            if (t.tag == "Wheel")
            {
                Wheels.Add(t.gameObject);
            }
            else if (t.tag == "Carlights")
            {
                Lights.Add(t.gameObject);
            }
            else if (t.tag == "Shadow")
            {
                Shadow = t.gameObject;
            }
        }


        FlickerLights();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCarDestroyed)
        {
            WheelRot();
            //FlickerLights();
        }
    }

    public void WheelRot()
    {
        if (Wheels!= null)
        {
            for (int i = 0; i <Wheels.Count; i++)
            {
                Wheels[i].transform.Rotate(rot * Time.deltaTime * 20f);
            }
        }
       
    }

   
    public void DestroyCar()
    {
        isCarDestroyed = true;
        rb.isKinematic =false; 
        CarSP.follow = false;
        //rb.AddForce(Vector3.right*100f, ForceMode.Impulse);
        Col.isTrigger = false;
        rb.useGravity =true;
        rb.AddExplosionForce(500f,-Vector3.up, 200f);
        Shadow.SetActive(false);

    }

    public void FlickerLights()
    {
        if (Lights != null)
        {
            for (int i = 0; i < Lights.Count; i++)
            {
                Lights[i].GetComponent<Light>().DOIntensity(0.2f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash);
            }
        }
    }

}
