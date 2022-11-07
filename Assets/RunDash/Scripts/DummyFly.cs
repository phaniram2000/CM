using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DitzeGames.Effects;
public class DummyFly : MonoBehaviour
{
    public List<Rigidbody> rb;
    public ParticleSystem ObstacleParticle;
    public SkinnedMeshRenderer skin;
    void Start()
    {
        for (int i = 0; i < rb.Count; i++)
        {
            rb[i].AddForce(transform.right * 5, ForceMode.Force);
            rb[i].AddForce(-transform.right * 5, ForceMode.Force);
        }
        ObstacleParticle.Play(true);

    }
    private void Update()
    {
        //if (Input.GetMouseButtonDown(1))
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("YellowEnable"))
        {
            skin.material = Player.instance.YellowSkin;
        }
        if (other.gameObject.CompareTag("RedEnable"))
        {
            skin.material = Player.instance.RedSkin;
        }
    }
    // Update is called once per frame

}
