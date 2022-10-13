using DG.Tweening;
using TMPro;
using UnityEngine;

public class Blackmailer : MonoBehaviour
{

	private Animator _animator;

	//private static readonly int ToPeekHash = Animator.StringToHash("ToPeek");
	private static readonly int ToTakePhotosHash = Animator.StringToHash("ToTakePhotos");
	private static readonly int FoundTakingPhotosHash = Animator.StringToHash("FoundTakingPhotos");
	private static readonly int ToEscapeHash = Animator.StringToHash("ToEscape");
	private static readonly int ToRunHash = Animator.StringToHash("ToRun");
	private static readonly int ToCelebrateHash = Animator.StringToHash("ToCelebrate");
	private static readonly int ToMoveNearCoupleHash = Animator.StringToHash("ToMoveNearCouple");
	private static readonly int ToStopHash = Animator.StringToHash("Stop");
	private static readonly int ToLoseHash = Animator.StringToHash("ToLose");

	[SerializeField] private int totalPhotosToTake = 8;
	[SerializeField] private TMP_Text countText;
	[SerializeField] private Transform finalEscapePosition;
	[SerializeField] private GameObject flashImage;
	[SerializeField] private GameObject blackMailCanvas;
	[SerializeField] private Transform firstTransform;
	[SerializeField] private Transform finalTransform;
	[SerializeField] private Transform manTransform;

	private void OnEnable()
	{
		BlackmailingEvents.StartTakingPictures += StartTakingPhotos;
		BlackmailingEvents.StopTakingPictures += StopTakingPhotos;
		BlackmailingEvents.FoundTakingPictures += FoundTakingPictures;
		BlackmailingEvents.TakePicture += OnTakePicture;
		BlackmailingEvents.FinalWin += ToCelebrate;
		BlackmailingEvents.FinalLose += ToLose;
	}

	private void OnDisable()
	{
		BlackmailingEvents.StartTakingPictures -= StartTakingPhotos;
		BlackmailingEvents.StopTakingPictures -= StopTakingPhotos;
		BlackmailingEvents.FoundTakingPictures -= FoundTakingPictures;
		BlackmailingEvents.TakePicture -= OnTakePicture;
		BlackmailingEvents.FinalWin -= ToCelebrate;
		BlackmailingEvents.FinalLose -= ToLose;
	}

	private void Start()
	{
		_animator = GetComponent<Animator>();
		countText.text = totalPhotosToTake.ToString();
	}


	private void StartTakingPhotos()
	{
		_animator.SetBool(ToTakePhotosHash,true);
	}
	

	private void StopTakingPhotos()
	{
		_animator.SetBool(ToTakePhotosHash,false);
	}

	private void FoundTakingPictures()
	{
		_animator.SetBool(FoundTakingPhotosHash, true);
		_animator.SetTrigger(ToEscapeHash);
		DOVirtual.DelayedCall(2f, Run);
	}
	
	private void TakingPicturesSuccessful()
	{
		_animator.SetBool(FoundTakingPhotosHash, false);
		_animator.SetTrigger(ToCelebrateHash);
		DOVirtual.DelayedCall(2f, MoveToCouple);
		BlackmailingEvents.InvokeGotFooled();
		if (AudioManager.instance)
			AudioManager.instance.Play("NiceFemale");
		BlackmailingEvents.InvokeToInterruptTheSequence();

	}

	private void MoveToCouple()
	{
		_animator.SetTrigger(ToMoveNearCoupleHash);
		transform.DOLookAt(firstTransform.position, 0.1f);
		transform.DOMove(firstTransform.position, 1.5f).SetEase(Ease.Linear).OnComplete(() =>
		{
			transform.DOLookAt(finalTransform.position, 0.1f);
			transform.DOMove(finalTransform.position, 2f).SetEase(Ease.Linear).OnComplete(() =>
			{
				_animator.SetTrigger(ToStopHash);
				BlackmailingEvents.InvokeToNextGamePhase();
				transform.DOLookAt(manTransform.position, 0.25f);
			});
		});
	}

	private void ClickForPictures()
	{
		totalPhotosToTake--;
		countText.text = totalPhotosToTake.ToString();

		if (totalPhotosToTake > 0) return;
		
		TakingPicturesSuccessful();
		blackMailCanvas.SetActive(false);
		return;
	}

	private void OnTakePicture() => ClickForPictures();

	private void Run()
	{
		_animator.SetTrigger(ToRunHash);
		transform.DOLookAt(finalEscapePosition.position, 0.1f).SetEase(Ease.Linear);
		transform.DOMove(finalEscapePosition.position, 3f).SetEase(Ease.Linear);
	}

	public void Flash()
	{
		flashImage.SetActive(true);
	}

	public void StopFlash()
	{
		flashImage.SetActive(false);
	}

	private void ToCelebrate()
	{
		_animator.SetTrigger(ToCelebrateHash);
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("NiceFemale");
		}
	}

	private void ToLose()
	{
		_animator.SetTrigger(ToLoseHash);
	}

}
