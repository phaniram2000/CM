using DG.Tweening;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public Animator anim;

    public MeshRenderer ballMesh;
    public Transform MainCamera;
    public GameObject PlayerFinish;
    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("knife"))
        {
            other.gameObject.tag = "Diveknife";
            print("dive");
            print("move");
            var position = MainCamera.position;
           // position = new Vector3(position.x, position.y + 5f, position.z);
            MainCamera.position = position;
            MainCamera.transform.DOMove(new Vector3(position.x, position.y + 2f, position.z), 1f)
                .SetEase(Ease.OutQuint);
           PlayerFinish.GetComponent<Collider>().enabled = false;
        }  
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.name == ("(AI)Player"))
    //     {
    //         UiManager.instance.LosePanel.SetActive(true);
    //     }
    // }
}