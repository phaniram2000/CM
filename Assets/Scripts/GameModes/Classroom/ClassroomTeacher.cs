using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ClassroomTeacher : MonoBehaviour, IDialogueShower
{
	[Header("Dialogue"), SerializeField] private TextMeshPro dialogueBox;
	private Tween _dialogTween;
	private Vector3 _initDialogueScale;
	
	TextMeshPro IDialogueShower.DialogueText => dialogueBox;
	Vector3 IDialogueShower.InitDialogueScale => _initDialogueScale;

	[Header("Particle Fx"), SerializeField] private ParticleSystem spank;
	[SerializeField] private Transform spankHolder;

	private CinemachineImpulseSource _impulse;
	private Transform _spankTransform;
	
	private void OnEnable()
	{
		GameEvents.TapToPlay += OnTapToPlay;
	}

	private void OnDisable()
	{
		GameEvents.TapToPlay -= OnTapToPlay;
	}

	private void Start()
	{
		_impulse = GetComponent<CinemachineImpulseSource>();
		_spankTransform = spank.transform;
		
		_initDialogueScale = dialogueBox.transform.parent.localScale;
		dialogueBox.transform.parent.localScale = Vector3.zero;

		_dialogTween = ((IDialogueShower)this).ShowDialogue(
			"You Scored Just 5 marks! <color=#F50016>How Shameful!</color>", 9999f, ((IDialogueShower)this).InitDialogueScale);
	}

	public void SpawnSpankParticleFx()
	{
		_spankTransform.parent = spankHolder;
		_spankTransform.localPosition = Vector3.zero;
		spank.Play();
		_spankTransform.parent = null;
		_impulse.GenerateImpulse();
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("Spank");
		}
	}

	private void OnTapToPlay() => _dialogTween.Kill(true);

	private void OnGameLose(int status)
	{
		if (status == -1)
		{
			((IDialogueShower)this).ShowDialogue(
				"You are a disgrace!", 
				9999f, ((IDialogueShower)this).InitDialogueScale);
			return;
		}
		
		((IDialogueShower)this).ShowDialogue(
			"The exam was only for 100 marks! You can't even cheat properly!", 
			9999f, ((IDialogueShower)this).InitDialogueScale);
	}
}