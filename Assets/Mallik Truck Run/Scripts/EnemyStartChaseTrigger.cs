using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStartChaseTrigger : MonoBehaviour
{
    public List<EnemyCarChasingAI> enemyChasingAIList;
    
    private void Awake()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {   
        if(other.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < enemyChasingAIList.Count; i++)
            {
                enemyChasingAIList[i].gameObject.SetActive(true);
                enemyChasingAIList[i].speed = GameManagerTruck.instance.truckSpeed - 1.5f;
                enemyChasingAIList[i].rotateSpeed = 200;
            }
        }
    }
}
