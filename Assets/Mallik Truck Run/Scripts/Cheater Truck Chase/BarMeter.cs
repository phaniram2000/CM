using DG.Tweening;
using UnityEngine;

public class BarMeter : MonoBehaviour
{
    [SerializeField] private Transform slapBarArrow, arrowHolder;
    [SerializeField] private float arrowRotationDuration, rotationInitialPos, rotateEndPos, scale;
    private Tween arrowHolderTween;

    void Start()
    {
        arrowHolder.localRotation = Quaternion.Euler(0, 0, rotationInitialPos);
        arrowHolderTween = arrowHolder.DOLocalRotate(new Vector3(0, 0, rotateEndPos), arrowRotationDuration)
            .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnEnable()
    {
        AudioManager.instance.Play("tick");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            arrowHolderTween.Kill();
            AudioManager.instance.Pause("tick");

            var arrowValue = arrowHolder.transform.localEulerAngles.z;
            if (arrowValue > 33f)
                arrowValue -= 360f;

            arrowValue = Mathf.Abs(arrowValue);
            //print("arrow value: " + arrowValue);
            if (arrowValue > 20)
            {
                KarenCheater.instance.FallDown();
                //GameManager.instance.StartCoroutine(GameManager.instance.LevelFailed(3.5f));
            }
            else
            {
                KarenCheater.instance.StartCoroutine(KarenCheater.instance.JumpToTruck());
            }
        }
    }
}