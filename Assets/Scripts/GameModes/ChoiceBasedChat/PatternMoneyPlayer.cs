using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PatternMoneyPlayer : MonoBehaviour
{
	[SerializeField] private Rig headRig;
	[SerializeField] private Canvas lockscreenCanvas, chatCanvas;
	[SerializeField] private RectTransform maskedLockscreen;

	[Header("Pre play Animation"), SerializeField] private Rig spineRig;
	[SerializeField] private Rig handRig;
	[SerializeField] private Transform mobileOnBedSocket, mobileHandSocket, handRigBedDest, handRigPlayDest, handRigTarget;
	
	private Animator _anim;

	private static readonly int Win = Animator.StringToHash("win");
	private static readonly int Lose = Animator.StringToHash("lose");

	private void OnEnable()
	{
		PatternMoneyEvents.CompletePatternStage += OnCompletePatternStage;

		GameEvents.GameWin += OnGameWin;
		GameEvents.GameLose += OnGameLose;
	}

	private void OnDisable()
	{
		PatternMoneyEvents.CompletePatternStage -= OnCompletePatternStage;

		GameEvents.GameWin -= OnGameWin;
		GameEvents.GameLose -= OnGameLose;
	}

	private void Start()
	{
		_anim = GetComponent<Animator>();
		chatCanvas.enabled = lockscreenCanvas.enabled = false;
		PickupPhone();
	}

	private void TurnOffGameplayRigs()
	{
		var temp = 1f;
		
		DOTween.To(() => temp, value => temp = value, 0f, 0.25f)
			.OnUpdate(() =>
			{
				headRig.weight = temp;
				handRig.weight = temp;
			});
	}

	private void TurnOffCanvases() => 
		DOVirtual.DelayedCall(0.35f, () => chatCanvas.enabled = lockscreenCanvas.enabled = false);

	private void PickupPhone()
	{
		var seq = DOTween.Sequence();

		seq.Append(DOTween.To(() => spineRig.weight,
				value => spineRig.weight = handRig.weight = value,
				1f, 1f));

		seq.Join(handRigTarget.DOMove(handRigBedDest.position, 0.5f)
			.SetDelay(0.5f)
			.SetEase(Ease.InOutSine));
		seq.Join(handRigTarget.DORotateQuaternion(handRigBedDest.rotation, 0.5f)
			//.SetDelay(0.5f)
			.SetEase(Ease.InOutSine));
		seq.AppendInterval(0.25f);
		seq.AppendCallback(TransferMobileOwnership);
		seq.AppendCallback(() => handRigTarget.DOMove(handRigPlayDest.position, 0.5f)
			.SetDelay(0.5f)
			.SetEase(Ease.InOutSine)
			.OnStart(() => 
				handRigTarget.DORotateQuaternion(handRigPlayDest.rotation, 0.5f)
					.SetEase(Ease.InOutSine)));
		seq.Append(DOTween.To(() => spineRig.weight,
			value => spineRig.weight = value,
			0f, 1f));
		seq.AppendCallback(() => chatCanvas.enabled = lockscreenCanvas.enabled = true);
	}

	private void TransferMobileOwnership()
	{
		var mobile = mobileOnBedSocket.GetChild(0);
		mobile.parent = mobileHandSocket;
		mobile.DOLocalMove(Vector3.zero, 0.25f);
		mobile.DOLocalRotate(Vector3.zero, 0.25f);
	}

	private void OnCompletePatternStage()
	{
		maskedLockscreen.DOAnchorPos(Vector2.up * 2160, 1f)
			.SetEase(Ease.OutQuint)
			.SetDelay(.5f)
			.OnComplete(() =>
			{
				chatCanvas.sortingOrder = 0;
				lockscreenCanvas.sortingOrder = 3;
			});
	}

	private void OnGameWin()
	{
		_anim.SetTrigger(Win);
		var dir = Camera.main.transform.position - transform.position;
		dir.y = 0;
		transform.DORotateQuaternion(Quaternion.LookRotation(dir), 0.25f);
		TurnOffGameplayRigs();
		TurnOffCanvases();
	}

	private void OnGameLose(int result)
	{
		_anim.SetTrigger(Lose);
		var dir = Camera.main.transform.position - transform.position;
		dir.y = 0;
		transform.DORotateQuaternion(Quaternion.LookRotation(dir), 0.25f);
		TurnOffGameplayRigs();
		TurnOffCanvases();
	}
}