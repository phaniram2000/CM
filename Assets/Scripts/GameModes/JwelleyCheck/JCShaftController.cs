using System;
using DG.Tweening;
using UnityEngine;

public class JCShaftController : MonoBehaviour
{
    [SerializeField] private Transform upperShaft;
    [SerializeField] private float upperShaftYDownvalue,upperShaftYUpvalue,crushDuration;
    [SerializeField] private ParticleSystem crushParticleSystem,diamondCircleParticleSystem;


    private GameObject currentJwelleryItem;
    
    private void OnEnable()
    {
        JCEvents.JwelleryReadyToCrush += OnJwelleryReadyToCrush;
    }

    private void OnDisable()
    {
        JCEvents.JwelleryReadyToCrush -= OnJwelleryReadyToCrush;
    }

    private void Start()
    {
        DisableCrushParticle();
    }

    private void ShaftCrushDownAnimation()
    {
        upperShaft.DOLocalMoveY(upperShaftYDownvalue, crushDuration).SetEase(Ease.Linear).OnStart(() =>
        {
            DOVirtual.DelayedCall(crushDuration / 2f, () => currentJwelleryItem.SetActive(false));

        }).OnComplete(() =>
        {
            DOVirtual.DelayedCall(0.2f,()=>ShaftUpAnimation());
            if(AudioManager.instance)
                AudioManager.instance.Play("DiamondCrush");
            //Invoke event to let know crushing done.
            DOVirtual.DelayedCall(0.1f,()=>EnableCrushParticle());
            JCEvents.InvokeOnJwelleryCrushingDone();
        });
    }

    private void ShaftUpAnimation()
    {
        upperShaft.DOLocalMoveY(upperShaftYUpvalue, crushDuration).SetEase(Ease.Linear);

    }


    private void OnJwelleryReadyToCrush(GameObject obj)
    {

        currentJwelleryItem = obj;
        PrepareParitclesMat();
        DOVirtual.DelayedCall(0.3f, () => ShaftCrushDownAnimation());
    }

    private void EnableCrushParticle()
    {
        
        
        crushParticleSystem.gameObject.SetActive(true);
    }

    private void DisableCrushParticle()
    {
        crushParticleSystem.gameObject.SetActive(false);
    }

    private void PrepareParitclesMat()
    {
        diamondCircleParticleSystem.GetComponent<Renderer>().material = currentJwelleryItem.GetComponent<Renderer>().material;
    }



}
