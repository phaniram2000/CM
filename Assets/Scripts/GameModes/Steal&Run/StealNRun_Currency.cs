using UnityEngine;

public class StealNRun_Currency : MonoBehaviour
{
   public Collider myCollider;
   public MeshRenderer meshRenderer;
   public ParticleSystem effect;

   private void Awake()
   {
      myCollider = GetComponent<Collider>();
      meshRenderer = GetComponent<MeshRenderer>();
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         AfterCollision();
      }
   }

   private void AfterCollision()
   {
      myCollider.enabled = false;
      meshRenderer.enabled = false;
      if (effect)
      {
         effect.transform.position = transform.position;
         effect.Play();
         AudioManager.instance.Play("Currency");
      }
     
   }
}
