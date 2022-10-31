using UnityEngine;

public class ShootOut_Bamboo : MonoBehaviour
{
   public Collider myCol;
   public GameObject rope;
   public Rigidbody[] sticks;
   private void Awake()
   {
      myCol = GetComponent<Collider>();
      
      sticks = new Rigidbody[transform.childCount - 1];
      
      for (var i = 0; i < transform.childCount; i++)
      {
          if (transform.GetChild(i).name.Contains("Rope"))
          {
              rope = transform.GetChild(i).gameObject;
          }
          else
          {
              sticks[i] = transform.GetChild(i).GetComponent<Rigidbody>();
          }
      }
   }

   public void ActivateSticks()
   {
       myCol.enabled = false;
       rope.SetActive(false);
       foreach (var t in sticks)
       {
           t.isKinematic = false;
           t.GetComponent<Collider>().enabled = true;
           t.AddTorque(t.transform.forward * 10f,ForceMode.Impulse);
           t.AddForce(t.transform.forward * 10f,ForceMode.Impulse);
       }
       ShootOut_GameManager.instance.Vibrate(20);
   }
   
  
}
