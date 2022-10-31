using UnityEngine;
using Dreamteck.Splines;

public class wall : MonoBehaviour
{
    public dummyWall DW;
    public GameObject WallDebris,HitBox;
    Rigidbody rb;
    public bool isWallDestroyed = false;
    // Start is called before the first frame update
    void Start()
    {
        isWallDestroyed = false;
        WallDebris.SetActive(false);
        rb = HitBox.GetComponent<Rigidbody>();
        rb.useGravity = false;
        DW.GetComponent<SplineFollower>().follow = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (DW.iswalltouched == true)
        {
            DW.GetComponent<MeshRenderer>().enabled=false;
            DW.GetComponent<SplineFollower>().follow = false;
            HitBox.SetActive(false);
            rb.useGravity = true;
            WallDebris.SetActive(true);
            isWallDestroyed = true;
        }
    }

}
