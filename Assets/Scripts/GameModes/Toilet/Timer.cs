using DG.Tweening;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
	[SerializeField] private float duration, minTextSize, rumbleDuration;
	[SerializeField] private ContinuousImpulse continuousImpulse;
	private TextMeshPro _timerText;
	private Transform _transform;
	private float _initScale;

	private void OnEnable()
	{
		ToiletEvents.TimerStart += OnTimerStart;
		ToiletEvents.DoorSelected += OnDoorSelected;
	}

	private void OnDisable()
	{
		ToiletEvents.TimerStart -= OnTimerStart;
		ToiletEvents.DoorSelected -= OnDoorSelected;
	}

	private void Start()
	{
		_timerText = GetComponent<TextMeshPro>();
		_transform = transform;
		_initScale = _transform.localScale.x;
		_transform.localScale = Vector3.zero;

		continuousImpulse.host = _transform;
	}

	private void ShowTimer()
	{
		_transform.DOScale(Vector3.one * _initScale, 1f)
			.OnComplete(() =>
			{
				var shouldEaseIn = true;
				var countdown = duration;

				Tweener textTweener = null;

				DOTween.To(() => countdown, value => countdown = value, 0f, duration + 0.5f)
					.SetEase(Ease.Linear)
					.OnUpdate(() =>
					{
						_timerText.text = (int)countdown + "..";
						_timerText.color = Color.Lerp(Color.red, Color.green, (countdown / 3));
					});

				textTweener = _transform.DOScale(minTextSize, 0.5f)
					.SetLoops(Mathf.CeilToInt(duration) * 2, LoopType.Yoyo)
					.SetEase(Ease.InBack)
					.OnStepComplete(() =>
					{
						shouldEaseIn = !shouldEaseIn;
						textTweener.SetEase(shouldEaseIn ? Ease.InBack : Ease.OutBack);
					})
					.OnComplete(() =>
					{
						ToiletEvents.InvokeTimerExpiry();
						_transform.DOScale(0f, 0.5f)
							.SetEase(Ease.InBack);
					});
			});

		if (AudioManager.instance)
		{
			AudioManager.instance.Play("CountDown");
		}
	}

	private void OnTimerStart() => ShowTimer();

	private void OnDoorSelected() => DOVirtual.DelayedCall(rumbleDuration, () => { })
		.OnUpdate(continuousImpulse.Rumble);
}