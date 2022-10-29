using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float damping;
    public static CameraFollow instance;
    [HideInInspector] public bool followPlayer;
    public void Awake()
    {
        instance = this;    
    }   
    private void Start()
    {
        followPlayer = true;
    }
    private void LateUpdate()
    {
        if (followPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * damping);
        }
    }

    /*public IEnumerator LastRotate()
    {
        Vector3 movePos = new Vector3(playerCart.transform.position.x + 2, playerCart.transform.position.y + 2, playerCart.transform.position.z + 6);
        transform.DOMove(movePos, 2);
        yield return new WaitForSeconds(0);
        FindObjectOfType<CartBuilding>().ActivatePlayer();
        InGameManager.instance.CheckPlayerRank();
    }*/
}
