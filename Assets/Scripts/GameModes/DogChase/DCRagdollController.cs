using UnityEngine;

public class DCRagdollController : MonoBehaviour
{
    [SerializeField] private bool shouldRagdoll;
    [SerializeField] private Rigidbody[] rigidbodies;
    [SerializeField] private Collider[] colliders;
    [SerializeField] private float regularForce, throwBackForce, upForce;
		
   

    private void Start()
    {
       
    }

    public void GiveRagdollPunch()
    {
        if(!shouldRagdoll) return;

        var direction = -transform.forward;
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = false;
				
            rb.AddForce(direction  + (Vector3.up * upForce), ForceMode.Impulse);
        }
    }

    private void DisableColliders()
    {
        foreach (var collu in colliders) collu.enabled = false;
    }
}
