using UnityEngine;
using System.Collections;
using DitzeGames.Effects;
public class DWGDestroyer : MonoBehaviour {

	public float radius;
	public float Glassradius;
	public float force;
	public float Glassforce;
	public float height;
	void OnCollisionEnter(Collision col)
	{
			ExplodeForce();
			//Destroy(GetComponent<DWGDestroyer>());
	}
	
	// Explode force by radius only if a destructible tag is found
	void ExplodeForce()
	{	
		Vector3 explodePos = transform.position + new Vector3(0, 5f, 0);
		Collider[] colliders = Physics.OverlapSphere(explodePos, radius);
	//	Collider[] colliders = Physics.OverlapCapsule(explodePos,explodePos1, radius); 
		foreach (Collider hit in colliders){
			if(hit.GetComponent<Collider>().tag == "Destructible")
			{
				if(hit.GetComponent<Rigidbody>())
				{
					hit.GetComponent<Rigidbody>().isKinematic = false; 
					hit.GetComponent<Rigidbody>().AddExplosionForce(force, explodePos,radius);
					hit.GetComponent<Collider>().isTrigger = true;
					CameraEffects.ShakeOnce(0.4f, 10f);
					AudioManager.instance.Play("Brick");
					AudioManager.instance.Play("BrickHit");
					//print("Hit");
				}
			}
		}
		foreach (Collider hit in colliders){
			if(hit.GetComponent<Collider>().tag == "Glass")
			{
				if(hit.GetComponent<Rigidbody>())
				{
					hit.GetComponent<Rigidbody>().isKinematic = false; 
					hit.GetComponent<Rigidbody>().AddExplosionForce(Glassforce, explodePos,Glassradius);
					hit.GetComponent<Collider>().isTrigger = true;
					CameraEffects.ShakeOnce(0.2f, 5f);
					//print("Hit");
				}
			}
		}
		foreach (Collider hit in colliders){
			if(hit.GetComponent<Collider>().tag == "Wall")
			{
				if(hit.GetComponent<Rigidbody>())
				{
					hit.GetComponent<Rigidbody>().isKinematic = false; 
					hit.GetComponent<Rigidbody>().AddExplosionForce(Glassforce, explodePos,Glassradius);
					hit.GetComponent<Collider>().isTrigger = true;
					CameraEffects.ShakeOnce(0.2f, 5f);
					AudioManager.instance.Play("Brick");
					AudioManager.instance.Play("BrickHit");
					//print("Hit");
				}
			}
		}
	}
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position + new Vector3(0,height,0), radius);
		Gizmos.DrawWireSphere(transform.position + new Vector3(0,height,0), Glassradius);
	}
}
