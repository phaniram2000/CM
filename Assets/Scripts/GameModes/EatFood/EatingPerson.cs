using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EatingPerson : MonoBehaviour
{
	[SerializeField] private Transform foodToMoveTransform;

	[SerializeField] private TMP_Text countText;
	[SerializeField] private int totalEatables = 0;
	[SerializeField] private List<GameObject> eatables;
	[SerializeField] private GameObject leftHandFoodObj;
	[SerializeField] private GameObject rightHandFoodObj;

	[SerializeField] private float intervalTime = 1f;
	[SerializeField] private float eatIntervalTime = 0.5f;

	private Animator _animator;
	private static readonly int ToEatHash = Animator.StringToHash("ToEat");
	private static readonly int VictoryHash = Animator.StringToHash("Victory");
	private static readonly int DefeatHash = Animator.StringToHash("Defeat");
	private static readonly int IdleDefeatHash = Animator.StringToHash("IdleDefeat");

	[SerializeField] private bool isOpponent;
	[SerializeField] private SkinnedMeshRenderer sMesh;
	[SerializeField] private Renderer rend;
	private static readonly int MaterialAlbedo = Shader.PropertyToID("_BaseColor");

	[SerializeField] private GameObject eatingFillCanvas;
	[SerializeField] private Image eatingFillImage;
	[SerializeField] private float fillMultiplier = 1f;
	[SerializeField] private float emptyMultiplier = 1f;
	
	[SerializeField] private Color initialColor;
	[SerializeField] private Color finalColor;
	
	[SerializeField] private Color skinInitialColor;
	[SerializeField] private Color skinFinalColor;
	[SerializeField] private float skinColorFillMultiplier;
	[SerializeField] private float skinColorLerpValue;

	[SerializeField] private GameObject exclamation;
	[SerializeField] private ParticleSystem leftTearsParticleSystem;
	[SerializeField] private ParticleSystem rightTearsParticleSystem;
	[SerializeField] private ParticleSystem vomitParticleSystem;

	[SerializeField]
	[Range(0, 1)] private float showWarningAtLerpValue;

	public bool exclamationEnabled;
	
	private Sequence _mySeq;
	
	private void OnEnable()
	{
		GameEvents.TapToPlay += OnTapToPlay;
		EatingFoodEvents.GotFull += ShowDefeat;
		EatingFoodEvents.AteAllTheFood += ShowVictory;
	}

	private void OnDisable()
	{
		GameEvents.TapToPlay -= OnTapToPlay;
		EatingFoodEvents.GotFull -= ShowDefeat;
		EatingFoodEvents.AteAllTheFood -= ShowVictory;
	}

	private void Start()
	{
		_animator = GetComponent<Animator>();

		if (isOpponent)
		{
			//sMesh = GetComponent<SkinnedMeshRenderer>();
			SetWeight();
		}

		totalEatables = eatables.Count;
		countText.text = totalEatables.ToString();
	}
	
	
	private void OnTapToPlay()
	{
		//EatingRoutine();
		if (isOpponent)
		{
			print("isOpponent");
			//start eating
			EatRoutine();
		}
	}

	public void StartEating()
	{
		_animator.SetBool(ToEatHash,true);
	}
	
	public void StopEating()
	{
		_animator.SetBool(ToEatHash,false);
		DisableHandFoods();
	}

	private void EatRoutine()
	{
		if (!isOpponent) return;
		
		_animator.SetBool(ToEatHash, true);
		_mySeq = DOTween.Sequence();

		for (var index = 0; index < eatables.Count; index++)
		{
			var foodItem = eatables[index];
			_mySeq.AppendInterval(eatIntervalTime);
			_mySeq.AppendCallback(()=>MoveTheFoodToHand(foodItem));
		}
	}

	private void MoveTheFoodToHand(GameObject foodItem)
	{
		foodItem.transform.DOJump(foodToMoveTransform.position,0.1f,1, intervalTime).SetEase(Ease.Linear).OnComplete(() =>
		{
			foodItem.SetActive(false);
			totalEatables--;
			countText.text = totalEatables.ToString();
			if (totalEatables <= 0)
			{
				EatingFoodEvents.InvokeGotFull();
				GameCanvas.game.MakeGameResult(1,1);
			}
		});
	}

	public void Eat()
	{
		if (eatables.Count <= 0) return;
		
		var x = eatables[^1]; //hmm...
		x.transform.DOJump(foodToMoveTransform.position,0.1f,1, intervalTime).SetEase(Ease.Linear).OnComplete(() =>
		{
			x.SetActive(false);
			totalEatables--;
			countText.text = totalEatables.ToString();
		});
		eatables.Remove(x);
		
		if (eatables.Count <= 0)
		{
			GameCanvas.game.MakeGameResult(0,0);
			EatingFoodEvents.InvokeAteAllFood();
			return;
		}
	}

	public void EatWithLeftHand()
	{
		leftHandFoodObj.SetActive(true);
		rightHandFoodObj.SetActive(false);
		
		if(isOpponent)
		{
			if(AudioManager.instance) AudioManager.instance.Play("Eat");
		}
		else
			Vibration.Vibrate(15);
	}
	
	public void EatWithRightHand()
	{
		leftHandFoodObj.SetActive(false);
		rightHandFoodObj.SetActive(true);
		
		if(isOpponent)
		{
			if(AudioManager.instance)
				AudioManager.instance.Play("Eat");
		}
		else		
			Vibration.Vibrate(15);
	}

	private void DisableHandFoods()
	{
		leftHandFoodObj.SetActive(false);
		rightHandFoodObj.SetActive(false);
	}

	private void SetWeight()
	{
		DOTween.To(()=> sMesh.GetBlendShapeWeight(0),
			value => sMesh.SetBlendShapeWeight(0,value), 100, 3f);
	}

	public void IncreaseFillAmount()
	{
		if (eatingFillImage.fillAmount >= 1f) return;

		eatingFillImage.color = Color.Lerp(initialColor, finalColor, eatingFillImage.fillAmount);
		eatingFillImage.fillAmount += Time.deltaTime * fillMultiplier;
	}

	public void MakeSkinRed()
	{
		if (skinColorLerpValue <= 1f)
		{
			skinColorLerpValue += Time.deltaTime * skinColorFillMultiplier;
		}

		if (eatingFillImage.fillAmount >= showWarningAtLerpValue)
		{
			if (!exclamationEnabled)
			{
				exclamationEnabled = true;
				exclamation.SetActive(true);
				
				PlayTears();
			}
		}
		
		sMesh.material.SetColor(MaterialAlbedo,Color.Lerp(skinInitialColor, skinFinalColor, eatingFillImage.fillAmount));

		if (eatingFillImage.fillAmount >= 1f)
		{
			if (!isOpponent)
			{
				EatingFoodEvents.InvokeGotFull();
				GameCanvas.game.MakeGameResult(1,1);
			}
		}
	}

	public void DecreaseFillAmount()
	{
		if (eatingFillImage.fillAmount <= 0f) return;
		
		eatingFillImage.color = Color.Lerp(initialColor, finalColor, eatingFillImage.fillAmount);
		eatingFillImage.fillAmount -= Time.deltaTime * emptyMultiplier;
	}

	public void MakeSkinPale()
	{
		if (skinColorLerpValue >= 0f)
		{
			skinColorLerpValue -= Time.deltaTime * skinColorFillMultiplier;
		}
		
		sMesh.material.SetColor(MaterialAlbedo,Color.Lerp(skinInitialColor, skinFinalColor, eatingFillImage.fillAmount));

		if (eatingFillImage.fillAmount <= showWarningAtLerpValue)
		{
			if (exclamationEnabled)
			{
				exclamationEnabled = false;
				exclamation.SetActive(false);
			
				StopTears();
			}
		}
	}

	private void ShowVictory()
	{
		if (!isOpponent)
		{
			_animator.SetTrigger(VictoryHash);
			StopTears();
			StopVomit();
			exclamation.SetActive(false);
			eatingFillCanvas.SetActive(false);
			print("Here");
			if (AudioManager.instance) AudioManager.instance.Play("NiceFemale");
		}
		else
		{
			_animator.SetTrigger(DefeatHash);
			PlayVomit();
			PlayTears();
			_mySeq.Kill();
			if (AudioManager.instance) AudioManager.instance.Play("Vomit");
		}
	}

	private void ShowDefeat()
	{
		if (!isOpponent)
		{
			_animator.SetTrigger(DefeatHash);
			PlayVomit();
			
			exclamation.SetActive(false);
			eatingFillCanvas.SetActive(false);
			if (AudioManager.instance) AudioManager.instance.Play("Vomit");
		}
		else
		{
			_animator.SetTrigger(VictoryHash);
			_mySeq.Kill();
			if (AudioManager.instance) AudioManager.instance.Play("CoolMale");
		}
	}

	private void PlayTears()
	{
		leftTearsParticleSystem.Play();
		rightTearsParticleSystem.Play();
	}
	
	private void StopTears()
	{
		leftTearsParticleSystem.Stop();
		rightTearsParticleSystem.Stop();
	}

	private void PlayVomit()
	{
		vomitParticleSystem.Play();
		DOVirtual.DelayedCall(2f, StopVomit);
	}

	private void StopVomit() => vomitParticleSystem.Stop();
}
