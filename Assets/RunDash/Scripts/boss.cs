using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss : MonoBehaviour
{
    public EnemyDestroy _EnemyDestroy;

    public void bossanim()

    {
        _EnemyDestroy.EnemyAnim.enabled = false;
        for (int i = 0; i <_EnemyDestroy.EnemyRb.Count; i++)
        {
          _EnemyDestroy.EnemyRb[i].AddForce(Vector3.forward * 7500, ForceMode.Force);
            // transform.GetComponent<CapsuleCollider>().isTrigger = true;
        }
    }
}
