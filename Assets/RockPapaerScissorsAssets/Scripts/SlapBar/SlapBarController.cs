using DG.Tweening;
using UnityEngine;

namespace RPS
{

	public class SlapBarController : MonoBehaviour
	{
		private Transform _transform;
		private Animation _animation;

		[SerializeField] private Transform slapBarArrow, arrowHolder;
		[SerializeField] private float arrowRotationDuration, rotationInitialPos, rotateEndPos, scale;
		[SerializeField] private bool clampUserPower;

		private bool _powerSlapEnable;

		private Tween arrowHolderTween;

		private CharacterRefBank _characterRefBank;



		private void OnEnable()
		{
			RPSGameEvents.PlayerWin += OnPlayerWin;
			RPSGameEvents.PlayerStartGiveSlap += OnPlayerStartGiveSlap;
			RPSGameEvents.PlayerGaveSlap += OnPlayerGaveSlap;
			RPSGameEvents.GameWin += OnGameWin;
			RPSGameEvents.PowerSlapGiven += OnPowerSlapGiven;
		}

		private void OnDisable()
		{
			RPSGameEvents.PlayerWin -= OnPlayerWin;
			RPSGameEvents.PlayerStartGiveSlap -= OnPlayerStartGiveSlap;
			RPSGameEvents.PlayerGaveSlap -= OnPlayerGaveSlap;
			RPSGameEvents.GameWin -= OnGameWin;
			RPSGameEvents.PowerSlapGiven -= OnPowerSlapGiven;
		}


		private void Start()
		{
			_transform = transform;

			//ye game object ko hide nahi karsakta kyuki ye ek enable me event ke upar depend hai to yaha zero karra hu scale,anything else comes in mind then please change........
			_transform.localScale = Vector3.zero;

			//_animation = GetComponent<Animation>();

			if (!_transform.root.TryGetComponent(out CharacterRefBank refBank)) return;

			_characterRefBank = refBank;

		}

		private void OnPlayerWin()
		{

			DOVirtual.DelayedCall(3.2f, () =>
			{
				_transform.DOScale(Vector3.one * scale, 0.7f).SetEase(Ease.OutBack).OnComplete(() =>
				{
					arrowHolder.localRotation = Quaternion.Euler(0, 0, rotationInitialPos);
					arrowHolderTween = arrowHolder.DOLocalRotate(new Vector3(0, 0, rotateEndPos), arrowRotationDuration)
						.SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
					//_animation.Play();
					//_animation["PowerBarAnim3D"].speed = 2f;
					RPSGameEvents.InvokeOnAllowPlayerToSlap();
					RPSAudioManager.instance.Play("SlapBarMoving");
				});
			});


		}

		private void OnPlayerStartGiveSlap()
		{
			//_animation.Stop();
			arrowHolderTween.Kill();
			var arrowValue = arrowHolder.transform.localEulerAngles.z;
			if (!_characterRefBank) return;

			if (!_characterRefBank.NpcController) return;

			//iska soacho kuch
			if (arrowValue > 33f)
				arrowValue -= 360f;

			arrowValue = Mathf.Abs(arrowValue);


			//complete the npc code here.
			//_characterRefBank.NpcController.DamageToNpc = MyHelpers.Remap(0f, 33f, 0.9f, 0.1f, arrowValue);

			print("arrow value: " + arrowValue);
			var damage = 1 - (Mathf.InverseLerp(0, 33, arrowValue));
			print("user slap meter: " + (Mathf.InverseLerp(0, 33, arrowValue)));

			if (clampUserPower)
				damage = MyHelpers.Remap(0.6f, 1f, 0.5f, 0.7f, damage);


			if (_powerSlapEnable)
				_characterRefBank.NpcController.DamageToNpc = 0.85f;
			else
				_characterRefBank.NpcController.DamageToNpc = damage;

			print("Damage to Npc: " + damage);

			RPSAudioManager.instance.Pause("SlapBarMoving");
			RPSAudioManager.instance.Play("SlapBarStop");

		}

		private void OnPlayerGaveSlap()
		{
			_transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutBack);
			if (_powerSlapEnable)
				_powerSlapEnable = false;
		}

		private void OnGameWin()
		{
			gameObject.SetActive(false);
		}

		private void OnPowerSlapGiven()
		{
			_powerSlapEnable = true;
		}
	}

}
