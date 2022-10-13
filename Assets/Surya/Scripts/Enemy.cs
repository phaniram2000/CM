using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class Enemy : MonoBehaviour
{
    public float speed;
    public PlayerControl NoControle;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            NoControle.enabled = false;
            NoControle.PlayerGo.enabled = false;
            if (AudioManager.instance)
                AudioManager.instance.Play("Crash");
            AudioManager.instance.Pause("Scouter");
        }
    }
}
