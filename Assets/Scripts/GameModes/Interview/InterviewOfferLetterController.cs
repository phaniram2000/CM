using DG.Tweening;
using UnityEngine;

public class InterviewOfferLetterController : MonoBehaviour
{
    [SerializeField] private Transform playerOfferLetterDest;
    [SerializeField] private float moveDuration;


    private void OnEnable()
    {
        InterviewEvents.MoveCheckToPlayer += MoveOfferLetterToPlayer;
    }

    private void OnDisable()
    {
        InterviewEvents.MoveCheckToPlayer -= MoveOfferLetterToPlayer;
    }


    private void MoveOfferLetterToPlayer()
    {
        transform.DOMove(playerOfferLetterDest.position, moveDuration).SetEase(Ease.Linear).OnComplete(()=>InterviewEvents.InvokeOnSwitchToSignCam());
    }
}
