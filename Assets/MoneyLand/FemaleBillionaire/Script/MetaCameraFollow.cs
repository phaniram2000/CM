using UnityEngine;

public class MetaCameraFollow : MonoBehaviour
{
    public static MetaCameraFollow instance;
    public Transform target;
    public Vector3 offset;
    public float speed;
	private Vector3 targetPos;
    private void Awake()
    {
        instance = this;
    }

	private void Start()
    {
        offset = target.position - transform.position;
    }

	private void LateUpdate()
    {
        targetPos = target.position - offset;
        transform.position = Vector3.MoveTowards(new Vector3(transform.position.x,transform.position.y, transform.position.z),new Vector3(targetPos.x, targetPos.y, targetPos.z), speed * Time.deltaTime);
    }
}
