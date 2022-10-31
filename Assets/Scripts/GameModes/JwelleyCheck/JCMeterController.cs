using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class JCMeterController : MonoBehaviour
{
    public bool IsInPosition { get; private set; }
    
    [SerializeField] private Transform deiredTransform,blinkingMeshTransform;
    
    [SerializeField] private float rotationTime;

    [SerializeField] private float blinkingDelay,blinkingResetDelay;

    [SerializeField] private GameObject rightPlane, wrongPlane;
    
    private Transform _transform;
    private Quaternion initialRot, desiredTapRot;

    private Tweener _rotateTween, _fromTween;
    
    
    private List<GameObject> blinkingMeshList;
    private JCJwelleryItemProperty jcJwelleryItemProperty;

    private int currentSelectedJwelleryBlinkingRange;


    private Sequence showBlinkingSeq,resetBlinkingSeq;

    private void OnEnable()
    {
        JCEvents.JwelleryItemSelected += OnJwelleryItemSelected;
    }

    private void OnDisable()
    {
        JCEvents.JwelleryItemSelected -= OnJwelleryItemSelected;
    }

    

    private void Start()
    {
        _transform = transform;
        initialRot = _transform.rotation;
        desiredTapRot = deiredTransform.rotation;
        
        GetAllBlinkingMeshGameObject();
        
        DisableAllBlinkingIndicators();
        
        DisableAnswerPlanes();
        

    }

    private void GetAllBlinkingMeshGameObject()
    {

        blinkingMeshList = new List<GameObject>();
        
        int childCount = blinkingMeshTransform.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            blinkingMeshList.Add(blinkingMeshTransform.transform.GetChild(i).gameObject);
        }
    }

    public void RotateTo()
    {
        if (_fromTween.IsActive()) _fromTween.Kill();
        CreateRotateToTween();
        
    }

    public void RotateBack()
    {
        if (_rotateTween.IsActive()) _rotateTween.Kill();
        CreateRotateBackTween();
       
    }

    private void CreateRotateToTween()
    {
        _rotateTween = transform.DORotateQuaternion(desiredTapRot, rotationTime)
            .OnStart(() => IsInPosition = false)
            .OnComplete(() =>
            {
                IsInPosition = true;
                ShowBlinkingCheckIndication();
            });
    }

    private void CreateRotateBackTween()
    {
        _fromTween = transform.DORotateQuaternion(initialRot, rotationTime)
            .OnStart(() =>
            {
                ResetBlinkingCheckIndication();
                IsInPosition = false;
            })
            .OnComplete(() => IsInPosition = false);
    }


    private void ShowBlinkingCheckIndication()
    {
        
        resetBlinkingSeq.Kill();
        showBlinkingSeq = DOTween.Sequence();
        
        for (int i = 0; i < currentSelectedJwelleryBlinkingRange; i++)
        {
            
            var index = i;
            showBlinkingSeq.AppendCallback(() => blinkingMeshList[index].SetActive(true));
            showBlinkingSeq.AppendInterval(blinkingDelay);

        }

        showBlinkingSeq.OnComplete(() =>
        {
            CheckAuthanticityOfJwellery();
            JCEvents.InvokeOnJwelleryMeterCheckDone();
        });

    }

    private void ResetBlinkingCheckIndication()
    {
        showBlinkingSeq.Kill();
        resetBlinkingSeq = DOTween.Sequence();
        
        for (int i = currentSelectedJwelleryBlinkingRange-1; i >= 0; i--)
        {

            var index = i;
            resetBlinkingSeq.AppendCallback(() => blinkingMeshList[index].SetActive(false));
            resetBlinkingSeq.AppendInterval(blinkingResetDelay);
        }
        
        DisableAnswerPlanes();
        

    }

    private void DisableAllBlinkingIndicators()
    {
        for (int i = 0; i < blinkingMeshList.Count; i++)
        {
            blinkingMeshList[i].SetActive(false);
            
        }
    }
    
    private void OnJwelleryItemSelected(GameObject obj)
    {
       
        if (!obj.TryGetComponent(out JCJwelleryItemProperty itemProperty)) return;

        jcJwelleryItemProperty = itemProperty;
        currentSelectedJwelleryBlinkingRange = itemProperty.ItemBlinkingRange;
    }

    private void DisableAnswerPlanes()
    {
        rightPlane.SetActive(false);
        wrongPlane.SetActive(false);
    }

    private void EnableRightPlane()
    {
        rightPlane.SetActive(true);
        wrongPlane.SetActive(false);
        
        if(AudioManager.instance)
            AudioManager.instance.Play("DiamondReal");
        
        Vibration.Vibrate(30);
    }

    private void EnableWrongPlane()
    {
        rightPlane.SetActive(false);
        wrongPlane.SetActive(true);
        
        if(AudioManager.instance)
            AudioManager.instance.Play("DiamondFake");
        
        Vibration.Vibrate(30);
    }

    private void CheckAuthanticityOfJwellery()
    {
        if (!jcJwelleryItemProperty) return;
        
        if(jcJwelleryItemProperty.IsReal)
            EnableRightPlane();
        else
            EnableWrongPlane();
    }

}