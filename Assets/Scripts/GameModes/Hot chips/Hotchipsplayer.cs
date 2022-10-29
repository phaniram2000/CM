using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Hotchipsplayer : MonoBehaviour
{

    public static event Action Onstage3;
    public static event Action drinkandvomit ;
    public static event Action end ;
    public GameObject chipspacket;
    public ParticleSystem fire;
    public GameObject Drinkontable, drinkinhand;

    public ParticleSystem spry;


    public Transform point1,point2;
    
    
     
    public Color startColor = Color.green;
    public Color endColor = Color.black;
    [Range(0, 10)]
    public float speed = 1;

    public Material RedMat;
    public bool ChangeColor;

    public bool final;

    public List<Transform> points;

    public int index;

    public float distance;

    public  ParticleSystem vomit;
    [SerializeField] private float lerpMultiplier;


    public Animator pranker;
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (final)
        {
            // var dir = points[index].transform.position - transform.position;
            // dir.y = 0;
            transform.LookAt(points[index]);
            // transform.rotation = Quaternion.Lerp(transform.rotation,
            //     Quaternion.LookRotation(dir),
            //     Time.deltaTime * lerpMultiplier);
            
            transform.position = Vector3.Lerp(transform.position, 
                points[index].transform.position,
                Time.deltaTime * lerpMultiplier);
            //transform.DOMove(point1.transform.position, 1f).SetEase(Ease.Linear);
            distance = Vector3.Distance(transform.position, points[index].transform.position);
            if (distance <.9f&& index <= points.Count)
            {
                index++;
            }

            if (index == points.Count)
            {
                index = 0;
            }
        }
        
    }

    public void Thorwchips()
    {
        fire.Play();
        chipspacket.GetComponent<Rigidbody>().isKinematic = false;
        chipspacket.GetComponent<Rigidbody>().AddForce(-transform.forward*100,ForceMode.Impulse);
        chipspacket.transform.parent = null;
        DOVirtual.DelayedCall(4, () => fire.Stop());

        RedMat.color = startColor;
        RedMat.DOColor(endColor, 1f)
        .SetLoops(4, LoopType.Yoyo)
        .OnComplete(() => RedMat.color = startColor);
        //RedMat.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
        DOVirtual.DelayedCall(3, () => ChangeColor = false);
        AudioManager.instance.Play("Scream");
    }

    public void drink()
    {
        Drinkontable.SetActive(false);
        drinkinhand.SetActive(true);
       
    }

    public void spray()
    {
        spry.Play();
        AudioManager.instance.Play("Soda open");
        DOVirtual.DelayedCall(.8f, () => Onstage3.Invoke());

    }

    private void OnDisable()
    {
        RedMat.color = startColor;
    }

    public void onpins()
    {
        final = true;
        pranker.SetBool("Dance",true);
  

    }

    public void drinkvomit()
    {
        
        drinkinhand.SetActive(false);
        drinkandvomit.Invoke();
        
    }

    public void vomitstart()
    {
        vomit.Play();
        if(AudioManager.instance)
            AudioManager.instance.Play("vomit");
    } 
    public void vomitstop()
    {
        vomit.Stop();
    }
}

