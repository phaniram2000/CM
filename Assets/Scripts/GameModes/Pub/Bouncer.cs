using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Bouncer : MonoBehaviour, IDialogueShower
{
	[SerializeField] private TextMeshPro dialogueBox;
	private Vector3 _initDialogueScale;
	
	TextMeshPro IDialogueShower.DialogueText => dialogueBox;
	Vector3 IDialogueShower.InitDialogueScale => _initDialogueScale;

	[Header("Move To Let Guest go"), SerializeField] private Ease moveEase;
	[SerializeField] private float rightMove, moveDuration;

	[Header("Take Card"), SerializeField] private Transform idCard;
	[SerializeField] private Transform rigTarget, collectCardTransform;
	[SerializeField] private string preCardHandoverText = "Wait you don't look 21!", overconfidentString = "You don't even look that old.";
	
	private Rig _rig;

	private PubJames _james;

	private Animator _anim;
	private static readonly int StepAside = Animator.StringToHash("sideStep");
	private static readonly int StepBack = Animator.StringToHash("stepBack");
	private static readonly int No = Animator.StringToHash("no");
	private static readonly int Push = Animator.StringToHash("push");

	private void OnEnable()
	{
		PubEvents.MoveQueueAhead += DoStepBack;
		GameEvents.GameWin += OnGameWin;
		GameEvents.GameLose += OnGameLose;
	}

	private void OnDisable()
	{
		PubEvents.MoveQueueAhead -= DoStepBack;
		GameEvents.GameWin -= OnGameWin;
		GameEvents.GameLose -= OnGameLose;
	}

	private void Start()
	{
		_rig = GetComponent<RigBuilder>().layers[0].rig;
		_anim = GetComponent<Animator>();
		
		_initDialogueScale = dialogueBox.transform.parent.localScale;
		dialogueBox.transform.parent.localScale = Vector3.zero;
	}

	public void HandoverCard(PubJames james)
	{
		((IDialogueShower)this).ShowDialogue(
			"Give me a second..", 1f, ((IDialogueShower)this).InitDialogueScale);
		if(AudioManager.instance)
			AudioManager.instance.Play("Give me a sec");
		
		rigTarget.DOMove(collectCardTransform.position, 0.5f)
			.OnStart(() =>
				rigTarget.DORotateQuaternion(collectCardTransform.rotation, 0.5f)
					.OnComplete(() =>
					{
						_james = james;
						var card = james.idCard.transform;
						card.parent = idCard;
						card.DOLocalMove(Vector3.zero, 0.125f);
						card.DOLocalRotate(Vector3.zero, 0.125f);
						
						DOTween.To(() => _rig.weight, value => _rig.weight = value, 0f, 1f)
							.SetDelay(0.5f)
							.OnComplete(() =>
							{
								idCard.gameObject.SetActive(false);
								
								GameCanvas.game.MakeGameResult();
							});
					}));
	}

	public void Talk()
	{
		((IDialogueShower)this).ShowDialogue(
			preCardHandoverText, 2f, ((IDialogueShower)this).InitDialogueScale);
		
		if(AudioManager.instance)
			AudioManager.instance.Play("You dont look");
	}

	public void SideStep()
	{
		_anim.SetTrigger(StepAside);
		transform.DOMove(transform.position + -transform.right * rightMove, moveDuration)
			.SetEase(moveEase);
	}

	private void DoStepBack()
	{
		_anim.SetTrigger(StepBack);
		transform.DOMove(transform.position + transform.right * rightMove, moveDuration)
			.SetEase(moveEase)
			.OnComplete(() =>
			{
				if(PubHelper.GetFirstInQueue == null && PubHelper.GetIfShowPushingCinematic)
					DOVirtual.DelayedCall(2.5f, PubEvents.InvokeStartPostPub);
			});
	}

	public void PushJames()
	{
		MyHelpers.TweenAnimatorLayerWeightTo(_anim, 1, 0f, 0.5f);
		_anim.SetTrigger(Push);

		DOVirtual.DelayedCall(1.25f,
			() => MyHelpers.TweenAnimatorLayerWeightTo(_anim, 1, 1f, 0.5f));
	}

	public void GivePushOnAnimation() => _james.GetPushed();

	private void OnGameWin() => SideStep();

	private void OnGameLose(int result)
	{
		_anim.SetTrigger(No);

		if (result > 0) ((IDialogueShower)this).ShowDialogue(overconfidentString, 9999f, ((IDialogueShower)this).InitDialogueScale);
	}
}