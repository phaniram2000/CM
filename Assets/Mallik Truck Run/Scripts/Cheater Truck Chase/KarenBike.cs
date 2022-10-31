using UnityEngine;

public class KarenBike : MonoBehaviour
{
    public static KarenBike instance;
    
    private Transform chasePoint;
    public float speed;

    private Transform truck;
    private float _lerpTime = 2;
    [HideInInspector] public bool canMove, barMeterOn;
    private Rigidbody bikeRb;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        truck = TruckMovement.instance.transform;
        chasePoint = truck.GetChild(truck.childCount - 1);
        bikeRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(!canMove) return;
        float dist = Vector3.Distance(chasePoint.position, transform.position);
        float finalSpeed = dist / speed;
        transform.position = Vector3.Lerp(transform.position, chasePoint.position, Time.deltaTime / finalSpeed);
        if (dist <= 0.35f && !barMeterOn)
        {
            barMeterOn = true;
            canMove = false;
            TruckMovement.instance.speed = 0;
            TruckMovement.instance.barMeter.SetActive(true);
        }
    }
}