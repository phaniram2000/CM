using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIKnife : MonoBehaviour
{

    public GameObject AIknife;
    public Vector3 Offset;

    public bool AIknifeHolderMove;
    private float timer=0;

    public float timeinterival;

    [SerializeField]
    private GameObject aiPlayer;
    // Start is called before the first frame update
    void Start()
    {
        AIknifeHolderMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        var dist = Vector3.Distance(transform.position, aiPlayer.transform.position);
        if (dist < 5)
        {
            return;
        }
        else
        {


            AIknifeHolderMove = true;

            if (AIknifeHolderMove)
            {
                var shoot = Random.Range(0, 50);
                if (Time.time > timer && shoot < 5)
                {
                    timer = Time.time + timeinterival;
                    CheckCube();
                }
            }
        }
    }
    void offset()
    {
        transform.position += Offset;
    }

    public void CheckCube()
    {
        if (Physics.Raycast(transform.position, transform.right, out RaycastHit hit, Mathf.Infinity))
        {
            var obj = hit.transform.gameObject;
            Physics.IgnoreRaycastLayer.CompareTo(2);
            if (hit.transform.CompareTag("Cube"))
            {//print(hit.transform.name);
                if (obj.GetComponent<Cube>().emptyState)
                {
                   // print("AIShoot");
                    offset();
                    instanceKnife();
                     var childobj = transform.GetChild(0);
                     childobj.GetComponent<AIKnifeMove>().endCubes = hit.point;
                     childobj.SetParent(null);
                     childobj.GetComponent<AIKnifeMove>().enabled = true;
                     
                    
                }
            }
        }
    }
    
    public void instanceKnife()
    {
        var obj = Instantiate(AIknife, transform.position, transform.rotation);
        obj.transform.SetParent(transform);
    }
}
