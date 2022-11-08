using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class CenterSurrounder : MonoBehaviour
{
    public GameObject OriginalSurrounderObject;
    public int SurrounderObjectCount;

    private readonly float AppearWaitDuration = 0.01f;
    private Transform SurrounderParentTransform;
    public static CenterSurrounder instance;
    public List<GameObject> players;
    public Animator enemyAnim;
    public Animator playerAnim;
    private string currentAnimaton;
    const string PLAYER_HIPHOPDANCE = "HipHopDancing";
    private void Awake()
    {
        instance = this;
       
    }
    void Start()
    {
        
        SurrounderParentTransform = new GameObject(gameObject.name + " Surrounder Parent").transform;
        StartCoroutine(SurroundStepAnimated());
        //Surround();
    }
    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        playerAnim.Play(newAnimation);
        currentAnimaton = newAnimation;
    }
    IEnumerator SurroundStepAnimated()
    {
        yield return new WaitForSeconds(AppearWaitDuration);

        float AngleStep = 360.0f / SurrounderObjectCount;

        OriginalSurrounderObject.transform.SetParent(SurrounderParentTransform);

        for (int i = 1; i < SurrounderObjectCount; i++)
        {
            GameObject newSurrounderObject = Instantiate(OriginalSurrounderObject);
            newSurrounderObject.transform.RotateAround(transform.position, Vector3.up, AngleStep * i);
            newSurrounderObject.transform.SetParent(SurrounderParentTransform);
            players.Add(newSurrounderObject);
            yield return new WaitForSeconds(AppearWaitDuration);
        }
    }
    public void damagePlayer()
    {
        //print("funCall" +players.Count);
        if (players.Count > 0)
        {
            for (int i = 1; i < 3; i++)
            {
                int index = Random.Range(0, players.Count - 1);
                players[index].GetComponent<Player>().takedamageplayer(10);
                AudioManager.instance.Play("Hit");
                players.Remove(players[index]);
            }
        }
        if (players.Count == 0)
        {
            enemyAnim.Play("Victory");
            uimanagr.instance.lost_panel();
        }
        
        //if(players.Count > 0)
        //{
        //    playerAnim.Play("HipHopDancing");
        //}
    }
    void Surround()
    {
        float AngleStep = 360 / SurrounderObjectCount;
        OriginalSurrounderObject.transform.SetParent(SurrounderParentTransform);

        for (int i = 1; i < SurrounderObjectCount; i++)
        {
            GameObject newSurrounderObject = Instantiate(OriginalSurrounderObject);
            newSurrounderObject.transform.RotateAround(transform.position, Vector3.up, AngleStep * i);
            newSurrounderObject.transform.SetParent(SurrounderParentTransform);
        }

    }
}