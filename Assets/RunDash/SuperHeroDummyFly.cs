using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DitzeGames.Effects;
public class SuperHeroDummyFly : MonoBehaviour
{
    public Rigidbody rb;
    public ParticleSystem ObstacleParticle;
    public SkinnedMeshRenderer skin;
    public Animator DummyAnim;
    private string currentAnimaton;

    const string PLAYER_JUMP_DEATH = "Death";
    void Start()
    {
        //for (int i = 0; i < rb.Count; i++)
        //{
        //    rb[i].AddForce(transform.right * 5, ForceMode.Force);
        //    rb[i].AddForce(-transform.right * 5, ForceMode.Force);
        //}
       
        ObstacleParticle.Play(true);
        ChangeAnimationState(PLAYER_JUMP_DEATH);
        rb.AddForce(Vector3.forward * 10000 * Time.deltaTime);
    }

    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;
        DummyAnim.Play(newAnimation);
        currentAnimaton = newAnimation;
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Obstacles"))
        {
            ChangeAnimationState(PLAYER_JUMP_DEATH);
        }
    }
}
