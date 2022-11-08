using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighJump : MonoBehaviour
{
    public int jumpforce;
    public int Speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisonEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.instance.jumpForce = jumpforce;
            Player.instance.speed = Speed;
            Time.timeScale = 45 * Time.deltaTime;
            // speed = poseSpeedTrucktoTruck; //level1 and level 2 110 and level 3 and 4
        }
    }
}
