using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class pricetag : MonoBehaviour
{
    public Rig police;
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
        if (other.gameObject.CompareTag("welcome"))
        {
            DOTween.To(() => police.weight, x => police.weight = x, 1, .3f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("welcome"))
        {
            DOTween.To(() => police.weight, x => police.weight = x, 0, .3f);
        }
    }
}
