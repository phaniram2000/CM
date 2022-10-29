using UnityEngine;

public class Knife : MonoBehaviour
{
    public GameObject knife;
    public GameObject Cube;
    public float speed;
    public bool knifeHolderMove;
    public Vector3 Offset;

    private float timer=0;

    public float timeinterival;

    public static Knife instance;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        knifeHolderMove = false;
        instanceKnife();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            knifeHolderMove = true;

        if (Input.GetMouseButtonUp(0))
        {
            knifeHolderMove = false;
        }

        if (knifeHolderMove)
        {

            if (Time.time > timer)
            {
                timer = Time.time + timeinterival;
                CheckCube();
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
                   // print("Shoot");
                   
                   var childobj = transform.GetChild(0);
                   childobj.GetComponent<knifeMOve>().endCubes = hit.point;
                   childobj.SetParent(null);
                   childobj.GetComponent<knifeMOve>().enabled = true;
                   offset();
                   instanceKnife();
                }
            }
        }
    }

    public void instanceKnife()
    {
         var obj = Instantiate(knife, transform.position, transform.rotation);
         obj.transform.SetParent(transform);
    }
    
}
