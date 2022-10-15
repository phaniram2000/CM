using DG.Tweening;
using UnityEngine;

public class Customer : MonoBehaviour
{
	public int myIdx = -1;
	
	private CustomerRotation _rotator;
	private Animator _anim;
	private float _moveDuration;
	private bool _deferredDelete;
	
	private static readonly int MoveBlend = Animator.StringToHash("moveBlend");

	private float WalkBlend
	{
		get => _anim.GetFloat(MoveBlend);
		set => _anim.SetFloat(MoveBlend, value);
	}

	private void Start()
	{
		if(!_anim) _anim = GetComponent<Animator>();
	}

	public void Init(CustomerRotation customerRotation, int idx, float duration)
	{
		_rotator = customerRotation;
		_deferredDelete = false;
		myIdx = idx;
		_moveDuration = duration;
	}

	public void MakeVisible()
	{
		DOTween.Kill(transform);
		transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
	}

	private Tween MakeInvisible() => transform.DOScale(0f, 0.5f).SetEase(Ease.InBack);

	public void MoveForward()
	{
		if(_deferredDelete) return;
		myIdx--;

		var moveTween = transform.DOMove(_rotator.standingPositions[myIdx].position, _moveDuration)
			.SetEase(Ease.Linear);

		transform.DORotateQuaternion(_rotator.standingPositions[myIdx].rotation, _moveDuration)
			.SetEase(Ease.Linear);

		DOTween.To(
			() => WalkBlend,
			value => WalkBlend = value,
			1f,
			_moveDuration * 0.25f);

		DOTween.To(
				() => WalkBlend,
				value => WalkBlend = value,
				0f,
				_moveDuration * 0.25f)
			.SetDelay(_moveDuration * 0.75f);

		if (myIdx != 0) return;
		
		_deferredDelete = true;
		moveTween.OnComplete(() =>
		{
			MakeInvisible().OnComplete(() =>
			{
				_rotator.RemoveCustomer(this);
				_rotator.MakeAndAddNewCustomer();
			});
		});
	}
}