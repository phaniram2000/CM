using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class ShootOut_SubLevels : MonoBehaviour
{
    public ShootOut_Police[] polices;
    public ShootOut_Bamboo bamboo;
    public LineRenderer[] reflectObjs;
    public GameObject enemyBlock;
    public bool CheckThisSubLevel()
    {
        return polices.All(t => t.isDead);
    }

    public void ReflectHandler(bool s)
    {
        if(reflectObjs.Length<=0) return;

        foreach (var tObj in reflectObjs)
        {
            tObj.enabled = s;
            //tObj.GetComponent<Collider>().enabled = s;
        }
        
    }

    public void PoliceShootNow()
    {
        for (int i = 0; i < polices.Length; i++)
        {
            if (polices[i].opponent != null)
            {
                ;
                if(polices[i].opponent.GetComponent<ShootOut_Player>())  polices[i].opponent.GetComponent<ShootOut_Player>().FallDown();
                if(polices[i].opponent.GetComponent<ShootOut_Police>())  polices[i].opponent.GetComponent<ShootOut_Police>().FallDown();
            } 
        }
        
        if(enemyBlock) enemyBlock.SetActive(false);
        
    }

}
