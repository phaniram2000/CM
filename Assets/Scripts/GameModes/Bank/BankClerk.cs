using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BankClerk : MonoBehaviour, IDialogueShower
{
	[SerializeField] private float twistDuration, checkSignTime;

	[SerializeField] private Transform money, moneyDest;
	[SerializeField] private Transform handTarget, handDest;
	
	[SerializeField] private TextMeshPro dialogueBox;
	private Vector3 _initDialogueScale;
	
	TextMeshPro IDialogueShower.DialogueText => dialogueBox;
	Vector3 IDialogueShower.InitDialogueScale => _initDialogueScale;

	private Animator _anim;
	private Rig _spineRig, _handRig, _lookRig;

	private static readonly int IsTyping = Animator.StringToHash("isTyping");
	private static readonly int Angry = Animator.StringToHash("angry");


	private void OnEnable()
	{
		BankEvents.DoneWithInput += OnDoneWithInput;
		GameEvents.GameLose += OnGameLose;
	}

	private void OnDisable()
	{
		BankEvents.DoneWithInput -= OnDoneWithInput;
		GameEvents.GameLose -= OnGameLose;
	}

	private void Start()
	{
		_anim = GetComponent<Animator>();
		var rigBuilder = GetComponent<RigBuilder>();
		_spineRig = rigBuilder.layers[0].rig;
		_handRig = rigBuilder.layers[1].rig;
		_lookRig = rigBuilder.layers[2].rig;
		
		_initDialogueScale = dialogueBox.transform.parent.localScale;
		dialogueBox.transform.parent.localScale = Vector3.zero;
	}

	private Tweener StartTyping()
	{
		_anim.SetBool(IsTyping, true);
		
		if(AudioManager.instance)
			AudioManager.instance.Play("Typing");
		
		return SetSpineToTypingWeight(1f);
	}

	private Tweener StopTyping()
	{
		_anim.SetBool(IsTyping, false);
		
		if(AudioManager.instance)
			AudioManager.instance.Pause("Typing");
		
		return SetSpineToTypingWeight(0f);
	}

	private Tweener SetSpineToTypingWeight(in float weight)
	{
		DOTween.Kill(_spineRig, true);

		return DOTween.To(() => _spineRig.weight,
				value => _spineRig.weight = value,
				weight,
				twistDuration)
			.SetEase(Ease.InOutSine)
			.SetTarget(_spineRig);
	}

	private void Push()
	{
		Tweener TweenHandRigTo(in float weight)
		{
			return DOTween.To(() => _handRig.weight,
					value => _handRig.weight = value,
					weight,
					.35f)
				.SetEase(Ease.InOutSine);
		}

		var initHandPos = handTarget.position;
		var seq = DOTween.Sequence();

		seq.Append(TweenHandRigTo(1));
		seq.AppendCallback(() => money.gameObject.SetActive(true));
		seq.AppendCallback(() => ((IDialogueShower)this).ShowDialogue("Here you go!", 2f, _initDialogueScale));
		seq.Append(handTarget.DOMove(handDest.position, 0.5f));
		seq.Join(money.DOMove(moneyDest.position, 1f));

		seq.AppendInterval(0.5f);

		seq.Append(TweenHandRigTo(0f));
		seq.Join(handTarget.DOMove(initHandPos, 0.5f));
		seq.AppendCallback(() => GameCanvas.game.MakeGameResult());
		
		if(AudioManager.instance)
			AudioManager.instance.Play("MoneySlide");
	}

	private void OnDoneWithInput()
	{
		var seq = DOTween.Sequence();

		var result = GameCanvas.game.CheckGameResult(GameCanvas.game.GetLastResult());
		seq.AppendInterval(1.5f);
		seq.AppendCallback(() => StartTyping());
		seq.AppendCallback(() => ((IDialogueShower)this).ShowDialogue(
			"Give me a second..", checkSignTime, ((IDialogueShower)this).InitDialogueScale));
		seq.AppendInterval(checkSignTime + twistDuration);
		seq.AppendCallback(() => StopTyping());
		seq.AppendInterval(twistDuration);
		seq.AppendInterval(1f);
		switch (result)
		{
			case 0: // win
				seq.AppendCallback(Push);
				return;
			case 1:
				seq.AppendCallback(() => GameCanvas.game.MakeGameResult());
				seq.AppendCallback(() => ((IDialogueShower)this).ShowDialogue("Police! Catch Her!", checkSignTime, _initDialogueScale));
				return;
		}
	}

	private void OnGameLose(int _)
	{
		_anim.SetTrigger(Angry);
		
		Tweener TweenLookRigTo(in float weight)
		{
			return DOTween.To(() => _lookRig.weight,
					value => _lookRig.weight = value,
					weight,
					.35f)
				.SetEase(Ease.InOutSine);
		}

		TweenLookRigTo(1f);
	}
}